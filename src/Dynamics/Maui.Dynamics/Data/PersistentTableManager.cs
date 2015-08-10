using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Blade;
using Blade.Data;
using Blade.Collections;
using Maui;
using Maui.Data.SQL;

namespace Maui.Dynamics.Data
{
    // TODO: how to handle DataTable.Dispose() here?!?
    /// <summary>
    /// This TableManager implements transaction. Anyhow, after a 
    /// transaction rollback all local/temp selection results are 
    /// invalid and need to be re-fetched.
    /// </summary>
    public class PersistentTableManager : ManagedObject, ITableManager
    {
        private IDatabaseSC myDB = null;
        private TableSchema mySchema = null;
        private DbCommand myFindByOwnerIdCmd = null;
        private DbCommand myFindByOwnerIdAndFromToCmd = null;
        private DbCommand myInsertCmd = null;
        private DbCommand myUpdateCmd = null;
        private DbCommand myRemoveCmd = null;

        internal PersistentTableManager( IDatabaseSC db, string name )
        {
            myDB = db;
            Name = name;
        }

        internal PersistentTableManager( IDatabaseSC db, TableSchema schema )
        {
            myDB = db;
            Name = schema.Name;
            mySchema = schema;
        }

        protected override void Dispose( bool disposing )
        {
            try
            {
                if ( IsDisposed )
                {
                    return;
                }

                if ( disposing )
                {
                    myFindByOwnerIdCmd.TryDispose();
                    myFindByOwnerIdAndFromToCmd.TryDispose();
                    myInsertCmd.TryDispose();
                    myUpdateCmd.TryDispose();
                    myRemoveCmd.TryDispose();
                }

                myDB = null;
                mySchema = null;
                myFindByOwnerIdAndFromToCmd = null;
                myInsertCmd = null;
                myUpdateCmd = null;
                myRemoveCmd = null;
            }
            finally
            {
                base.Dispose( disposing );
            }
        }

        public string Name { get; private set; }

        public TableSchema Schema
        {
            get
            {
                if ( mySchema == null )
                {
                    DbCommand cmd = myDB.CreateCommand();
                    cmd.CommandText = "SELECT * FROM " + Name + " LIMIT 1";

                    using ( DataTable table = myDB.Query( cmd ) )
                    {
                        mySchema = new TableSchema( Name, table.Columns.ToSet().
                            Select( c => new DataColumn( c.ColumnName, c.DataType ) ), true );
                    }
                }
                return mySchema;
            }
        }

        public void CreateTable()
        {
            if ( myDB.ExistsTable( Name ) )
            {
                return;
            }

            ValidationResult result = Schema.Validate();
            if ( result != ValidationResult.None )
            {
                throw new ArgumentException( "invalid table description: " + result );
            }

            StringBuilder sb = new StringBuilder();
            sb.Append( "CREATE TABLE " );
            sb.Append( Schema.Name );
            sb.Append( " ( " );

            var last = Schema.Columns.Last();
            foreach ( var column in Schema.Columns )
            {
                sb.Append( column.ColumnName );
                sb.Append( "  " );
                sb.Append( myDB.GetDBType( column.DataType ) );

                if ( column.ColumnName == Schema.IdColumn )
                {
                    sb.Append( " PRIMARY KEY AUTOINCREMENT" );
                }
                else
                {
                    sb.Append( ( column.AllowDBNull ? " NULL" : " NOT NULL" ) );
                }

                if ( column != last )
                {
                    sb.Append( "," );
                }
            }
            sb.Append( ");" );

            myDB.Execute( sb.ToString() );
        }

        // TODO: cleanup - use command
        public void Clear()
        {
            myDB.Execute( "DELETE * FROM " + Name );
        }

        public ScopedTable Query( long ownerId )
        {
            DataTable table = myDB.Query( FindByOwnerIdCmd, ownerId );
            table.RowChanged += OnRowChanged;
            table.RowDeleted += OnRowChanged;

            return new ScopedTable( Schema, table, null );
        }

        public ScopedTable Query( long ownerId, DateClause dateClause )
        {
            return Query( ownerId, dateClause, null );
        }

        public ScopedTable Query( long ownerId, DateClause dateClause, OriginClause originClause )
        {
            DataTable table = null;
            if ( Schema.DateColumn != null )
            {
                if ( Schema.DateIsYear )
                {
                    table = myDB.Query( FindByOwnerIdAndFromToCmd, ownerId, dateClause.From.Year, dateClause.To.Year );
                }
                else
                {
                    table = myDB.Query( FindByOwnerIdAndFromToCmd, ownerId,
                        Maui.TypeConverter.DateToString( dateClause.From ), Maui.TypeConverter.DateToString( dateClause.To ) );
                }
            }
            else
            {
                table = myDB.Query( FindByOwnerIdCmd, ownerId );
            }

            table.RowChanged += OnRowChanged;
            table.RowDeleted += OnRowChanged;

            return new ScopedTable( Schema, table, originClause );
        }

        // handles "modified" and "deleted"
        private void OnRowChanged( object sender, DataRowChangeEventArgs e )
        {
            if ( !( e.Action == DataRowAction.Add || e.Action == DataRowAction.Change || e.Action == DataRowAction.Delete ) )
            {
                return;
            }

            if ( e.Row.RowState == DataRowState.Added )
            {
                var id = myDB.Insert( InsertCmd, Schema.ColumnsWithoutId
                    .Select( col => e.Row[ col.ColumnName ] ).ToArray() );

                e.Row.Table.RowChanged -= OnRowChanged;
                e.Row[ Schema.IdColumn ] = id;
                e.Row.Table.RowChanged += OnRowChanged;
            }
            else if ( e.Row.RowState == DataRowState.Modified )
            {
                // TODO: we need to validate whether "ownerId" and/or "date" column
                // has been changed

                myDB.Execute( UpdateCmd, Schema.ValueColumns
                    .Select( col => e.Row[ col.ColumnName ] )
                    .Concat( e.Row[ Schema.IdColumn ] ).ToArray() );
            }
            else
            {
                // handle "deleted" ( we cannot access deleted rows)
                // TODO: is there a more efficient way?
                DataTable changes = e.Row.Table.GetChanges( DataRowState.Deleted );
                if ( changes != null )
                {
                    // so now we can access the data again
                    changes.RejectChanges();

                    foreach ( DataRow row in changes.Rows )
                    {
                        myDB.Execute( RemoveCmd, row[ Schema.IdColumn ] );
                    }
                }
            }

            e.Row.AcceptChanges();
        }

        #region DbCommands

        private DbCommand FindByOwnerIdCmd
        {
            get
            {
                if ( myFindByOwnerIdCmd == null )
                {
                    myFindByOwnerIdCmd = myDB.CreateCommand( "SELECT * FROM " + Name + " WHERE " +
                        Schema.OwnerIdColumn + " == ?;" );
                    myFindByOwnerIdCmd.InitParameters();
                }

                return myFindByOwnerIdCmd;
            }
        }

        private DbCommand FindByOwnerIdAndFromToCmd
        {
            get
            {
                if ( myFindByOwnerIdAndFromToCmd == null )
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append( "SELECT * FROM " );
                    sb.Append( Name );
                    sb.Append( " WHERE " );
                    sb.Append( Schema.OwnerIdColumn );
                    sb.Append( " == ? and " );
                    sb.Append( Schema.DateColumn );
                    sb.Append( " >= ? and " );
                    sb.Append( Schema.DateColumn );
                    sb.Append( " <= ?; " );

                    myFindByOwnerIdAndFromToCmd = myDB.CreateCommand( sb.ToString() );
                    myFindByOwnerIdAndFromToCmd.InitParameters();
                }

                return myFindByOwnerIdAndFromToCmd;
            }
        }

        private DbCommand InsertCmd
        {
            get
            {
                if ( myInsertCmd == null )
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append( "INSERT INTO " );
                    sb.Append( Name );
                    sb.Append( " VALUES (" );

                    var idCol = Schema.IdColumn;
                    var last = Schema.Columns.Last();
                    foreach ( var column in Schema.Columns )
                    {
                        sb.Append( ( column.ColumnName == idCol ? "NULL" : "?" ) );

                        if ( column != last )
                        {
                            sb.Append( "," );
                        }
                    }
                    sb.Append( ");" );

                    myInsertCmd = myDB.CreateCommand( sb.ToString() );
                    myInsertCmd.InitParameters();
                }

                return myInsertCmd;
            }
        }

        private DbCommand RemoveCmd
        {
            get
            {
                if ( myRemoveCmd == null )
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append( "DELETE FROM " );
                    sb.Append( Name );

                    sb.Append( " WHERE " );
                    sb.Append( " id ==?;" );

                    myRemoveCmd = myDB.CreateCommand( sb.ToString() );
                    myRemoveCmd.InitParameters();
                }

                return myRemoveCmd;
            }
        }

        private DbCommand UpdateCmd
        {
            get
            {
                if ( myUpdateCmd == null )
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append( "UPDATE " );
                    sb.Append( Name );
                    sb.Append( " SET " );

                    var last = Schema.ValueColumns.Last();
                    foreach ( var column in Schema.ValueColumns )
                    {
                        sb.Append( column.ColumnName );
                        sb.Append( " = ? " );

                        if ( column != last )
                        {
                            sb.Append( "," );
                        }
                    }

                    sb.Append( " WHERE " );
                    sb.Append( " id ==?;" );

                    myUpdateCmd = myDB.CreateCommand( sb.ToString() );
                    myUpdateCmd.InitParameters();
                }

                return myUpdateCmd;
            }
        }
        #endregion
    }
}
