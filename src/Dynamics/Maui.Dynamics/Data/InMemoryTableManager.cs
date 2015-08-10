using System;
using System.Data;
using System.Linq.Expressions;
using System.Transactions;
using Blade;
using Blade.Data;
using Blade.Functional;

namespace Maui.Dynamics.Data
{
    // TODO: how to handle DataTable.Dispose() here?!?
    public class InMemoryTableManager : ManagedObject, ITableManager
    {
        private DataTable myTable = null;
        private DataSet myDB = null;
        private Transaction myTransaction = null;
        private long myIdSeq = 0;

        internal InMemoryTableManager( DataSet db, DataTable table )
        {
            myDB = db;
            Table = table;
            Schema = new TableSchema( table.TableName, table.Columns.ToSet(), false );
        }

        internal InMemoryTableManager( DataSet db, TableSchema schema )
        {
            myDB = db;
            Table = null;
            Schema = schema;
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
                    Transaction = null;
                    Table = null;
                }

                myTable = null;
                myDB = null;
                myTransaction = null;
            }
            finally
            {
                base.Dispose( disposing );
            }
        }

        public string Name
        {
            get { return Schema.Name; }
        }

        public TableSchema Schema { get; private set; }

        public void CreateTable()
        {
            if ( Table != null )
            {
                return;
            }

            ValidationResult result = Schema.Validate();
            if ( result != ValidationResult.None )
            {
                throw new ArgumentException( "invalid table description: " + result );
            }

            Table = Schema.NewTempTable();

            myDB.Tables.Add( Table );
        }

        public void Clear()
        {
            myTable.Rows.Clear();
        }

        public ScopedTable Query( long ownerId )
        {
            return new ScopedTable( Schema, Table, null, row => (long)row[ Schema.OwnerIdColumn ] == ownerId );
        }

        public ScopedTable Query( long ownerId, DateClause dateClause )
        {
            return Query( ownerId, dateClause, null );
        }

        public ScopedTable Query( long ownerId, DateClause dateClause, OriginClause originClause )
        {
            Expression<Func<DataRow, bool>> RowFilter = row => (long)row[ Schema.OwnerIdColumn ] == ownerId;

            if ( Schema.DateColumn != null )
            {
                if ( Schema.DateIsYear )
                {
                    RowFilter = RowFilter.And( row => dateClause.IsInRange( row.GetDate( Schema ).Year ) );
                }
                else
                {
                    RowFilter = RowFilter.And( row => dateClause.IsInRange( row.GetDate( Schema ) ) );
                }
            }

            return new ScopedTable( Schema, Table, originClause, RowFilter.Compile() );
        }

        protected DataTable Table
        {
            get
            {
                return myTable;
            }
            set
            {
                if ( myTable == value )
                {
                    return;
                }

                if ( myTable != null )
                {
                    myTable.RowChanged -= OnRowChanged;
                    myTable.RowDeleted -= OnRowChanged;
                }

                myTable = value;

                if ( myTable != null )
                {
                    myTable.RowChanged += OnRowChanged;
                    myTable.RowDeleted += OnRowChanged;
                }
            }
        }

        protected Transaction Transaction
        {
            get
            {
                return myTransaction;
            }
            set
            {
                if ( myTransaction == value )
                {
                    return;
                }

                if ( myTransaction != null )
                {
                    myTransaction.TransactionCompleted -= OnTransactionCompleted;
                }

                myTransaction = value;

                if ( myTransaction != null )
                {
                    myTransaction.TransactionCompleted += OnTransactionCompleted;
                }
            }
        }

        private void OnRowChanged( object sender, DataRowChangeEventArgs e )
        {
            Transaction = Transaction.Current;

            // set ids
            if ( e.Action == DataRowAction.Add )
            {
                e.Row[ Schema.IdColumn ] = myIdSeq++;
            }

            if ( Transaction == null )
            {
                if ( e.Action == DataRowAction.Add || e.Action == DataRowAction.Change || e.Action == DataRowAction.Delete )
                {
                    Table.AcceptChanges();
                }
            }
        }

        private void OnTransactionCompleted( object sender, TransactionEventArgs e )
        {
            if ( e.Transaction.TransactionInformation.Status == TransactionStatus.Committed )
            {
                Table.AcceptChanges();
            }
            else
            {
                Table.RejectChanges();
            }
        }

    }
}
