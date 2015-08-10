using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Globalization;
using System.Transactions;
using Blade;
using Maui.Logging;
using Maui;
using System.IO;
using Blade.Logging;

namespace Maui.Data.SQL.SQLite
{
    // TODO: we need a possiblity to insert a bulk of data
    //       - use transaction and commit in the end
    //       - use SQLiteParameter
    public class SQLiteDatabaseSC : ManagedObject, IDatabaseSC
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( SQLiteDatabaseSC ) );

        private string myDBFile = null;
        private SQLiteConnection myConnection = null;
        private Transaction myTransaction = null;
        private SQLiteCommand myLastRowIdCmd = null;
        private SQLiteCommand myTableExistsCmd = null;
        private SQLiteCommand myViewExistsCmd = null;
        private SQLiteCommand myListTablesCmd = null;
        private SQLiteCommand myListTablesAndViewsCmd = null;

        public SQLiteDatabaseSC( string file )
        {
            myDBFile = file;

            myLastRowIdCmd = new SQLiteCommand( "SELECT last_insert_rowid() AS Id;" );
            myTableExistsCmd = new SQLiteCommand( "SELECT name FROM sqlite_master WHERE type = 'table' AND name = ?;" );
            myViewExistsCmd = new SQLiteCommand( "SELECT name FROM sqlite_master WHERE type = 'view' AND name = ?;" );
            myListTablesCmd = new SQLiteCommand( "SELECT name FROM sqlite_master WHERE type = 'table';" );
            myListTablesAndViewsCmd = new SQLiteCommand( "SELECT name FROM sqlite_master WHERE type = 'table' OR type = 'view';" );
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
                    myLastRowIdCmd.Dispose();
                    myTableExistsCmd.Dispose();
                    myListTablesCmd.Dispose();
                    myListTablesAndViewsCmd.Dispose();

                    Close();
                }

                myDBFile = null;
                myConnection = null;
                myTransaction = null;
                myLastRowIdCmd = null;
                myTableExistsCmd = null;
                myListTablesCmd = null;
                myListTablesAndViewsCmd = null;
            }
            finally
            {
                base.Dispose( disposing );
            }
        }

        public void Init( ServiceProvider serviceProvider )
        {
        }

        public string GetDBType( Type type )
        {
            if ( type == typeof( int ) || type == typeof( long ) )
            {
                return "INTEGER";
            }
            else if ( type == typeof( float ) )
            {
                return "REAL";
            }
            else if ( type == typeof( double ) )
            {
                return "DOUBLE";
            }
            else if ( type == typeof( string ) || type == typeof( DateTime ) )
            {
                return "TEXT";
            }
            else if ( type == typeof( bool ) )
            {
                return "BOOL";
            }
            else
            {
                return null;
            }
        }

        public bool ExistsDatabase()
        {
            return System.IO.File.Exists( myDBFile ) && ExistsTable( "db_metadata" );
        }

        public string ConnectionUri
        {
            get { return myDBFile; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic" )]
        public DbCommand CreateCommand()
        {
            return new SQLiteCommand();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic" )]
        public DbCommand CreateCommand( string sql )
        {
            return new SQLiteCommand( sql );
        }

        /// <summary>
        /// Call this outside of any transaction.
        /// </summary>
        public void CreateDatabase()
        {
            myLogger.Info( "Creating database at {0}", myDBFile );

            Open( true );

            Execute( "CREATE TABLE db_metadata (" +
                    "key TEXT NOT NULL," +
                    "value TEXT NULL" +
                ");" );

            Close();
        }

        public void Execute( string sql )
        {
            if ( string.IsNullOrEmpty( sql ) )
            {
                throw new ArgumentException( "parameter must not be null or empty", "sql" );
            }

            Open( false );

            using ( SQLiteCommand cmd = new SQLiteCommand( myConnection ) )
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
        }

        public void Execute( DbCommand cmd )
        {
            if ( cmd == null )
            {
                throw new ArgumentNullException( "cmd" );
            }

            Open( false );

            if ( cmd.Connection != myConnection )
            {
                cmd.Connection = myConnection;
            }

            cmd.ExecuteNonQuery();
        }

        public void Execute( DbCommand cmd, params object[] values )
        {
            if ( cmd == null )
            {
                throw new ArgumentNullException( "cmd" );
            }

            if ( values == null )
            {
                throw new ArgumentNullException( "values" );
            }

            for ( int i = 0; i < values.Length; ++i )
            {
                if ( cmd.Parameters.Count == i )
                {
                    cmd.Parameters.Add( cmd.CreateParameter() );
                }

                cmd.Parameters[ i ].Value = values[ i ];
            }

            Execute( cmd );
        }

        /// <summary>
        /// Returns the id of the inserted record
        /// </summary>
        public long Insert( string sql )
        {
            if ( string.IsNullOrEmpty( sql ) )
            {
                throw new ArgumentException( "parameter must not be null or empty", "sql" );
            }

            Open( false );

            if ( myLastRowIdCmd.Connection != myConnection )
            {
                myLastRowIdCmd.Connection = myConnection;
            }

            using ( SQLiteCommand cmd = new SQLiteCommand( myConnection ) )
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                return Convert.ToInt64( myLastRowIdCmd.ExecuteScalar(), CultureInfo.InvariantCulture );
            }
        }

        /// <summary>
        /// Returns the id of the inserted record
        /// </summary>
        public long Insert( DbCommand cmd )
        {
            if ( cmd == null )
            {
                throw new ArgumentNullException( "cmd" );
            }

            Open( false );

            if ( myLastRowIdCmd.Connection != myConnection )
            {
                myLastRowIdCmd.Connection = myConnection;
            }

            if ( cmd.Connection != myConnection )
            {
                cmd.Connection = myConnection;
            }

            cmd.ExecuteNonQuery();

            return Convert.ToInt64( myLastRowIdCmd.ExecuteScalar(), CultureInfo.InvariantCulture );
        }

        public long Insert( DbCommand cmd, params object[] values )
        {
            if ( cmd == null )
            {
                throw new ArgumentNullException( "cmd" );
            }

            if ( values == null )
            {
                throw new ArgumentNullException( "values" );
            }

            for ( int i = 0; i < values.Length; ++i )
            {
                if ( cmd.Parameters.Count == i )
                {
                    cmd.Parameters.Add( cmd.CreateParameter() );
                }

                cmd.Parameters[ i ].Value = values[ i ];
            }

            return Insert( cmd );
        }

        public object QueryScalar( string sql )
        {
            if ( string.IsNullOrEmpty( sql ) )
            {
                throw new ArgumentException( "parameter must not be null or empty", "sql" );
            }

            Open( false );

            using ( SQLiteCommand cmd = new SQLiteCommand( myConnection ) )
            {
                cmd.CommandText = sql;

                return cmd.ExecuteScalar();
            }
        }

        public object QueryScalar( DbCommand cmd )
        {
            if ( cmd == null )
            {
                throw new ArgumentNullException( "cmd" );
            }

            Open( false );

            if ( cmd.Connection != myConnection )
            {
                cmd.Connection = myConnection;
            }

            return cmd.ExecuteScalar();
        }

        public object QueryScalar( DbCommand cmd, params object[] values )
        {
            if ( cmd == null )
            {
                throw new ArgumentNullException( "cmd" );
            }

            if ( values == null )
            {
                throw new ArgumentNullException( "values" );
            }

            for ( int i = 0; i < values.Length; ++i )
            {
                if ( cmd.Parameters.Count == i )
                {
                    cmd.Parameters.Add( cmd.CreateParameter() );
                }

                cmd.Parameters[ i ].Value = values[ i ];
            }

            return QueryScalar( cmd );
        }

        public DataTable Query( string sql )
        {
            if ( string.IsNullOrEmpty( sql ) )
            {
                throw new ArgumentException( "parameter must not be null or empty", "sql" );
            }

            Open( false );

            // TODO: use: 
            // DbDataReader reader = cmd.ExecuteReader();
            // table.Load( reader );

            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter( sql, myConnection );

            DataTable table = new DataTable();
            table.Locale = CultureInfo.InvariantCulture;
            dataAdapter.Fill( table );

            return table;
        }

        public DataTable Query( DbCommand cmd )
        {
            if ( cmd == null )
            {
                throw new ArgumentNullException( "cmd" );
            }

            SQLiteCommand sqliteCmd = cmd as SQLiteCommand;
            if ( cmd == null )
            {
                throw new ArgumentException( "command has invalid type: " + cmd.GetType() );
            }

            Open( false );

            if ( sqliteCmd.Connection != myConnection )
            {
                sqliteCmd.Connection = myConnection;
            }

            // TODO: use: 
            // DbDataReader reader = cmd.ExecuteReader();
            // table.Load( reader );

            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter( sqliteCmd );

            DataTable table = new DataTable();
            table.Locale = CultureInfo.InvariantCulture;
            dataAdapter.Fill( table );

            return table;
        }

        public DataTable Query( DbCommand cmd, params object[] values )
        {
            if ( cmd == null )
            {
                throw new ArgumentNullException( "cmd" );
            }

            if ( values == null )
            {
                throw new ArgumentNullException( "values" );
            }

            for ( int i = 0; i < values.Length; ++i )
            {
                if ( cmd.Parameters.Count == i )
                {
                    cmd.Parameters.Add( cmd.CreateParameter() );
                }

                cmd.Parameters[ i ].Value = values[ i ];
            }

            return Query( cmd );
        }

        public bool ExistsTable( string name )
        {
            object obj = QueryScalar( myTableExistsCmd, name );
            return obj != null;
        }

        public bool ExistsView( string name )
        {
            object obj = QueryScalar( myViewExistsCmd, name );
            return obj != null;
        }

        public string[] GetTables( bool includeViews )
        {
            DbCommand cmd = ( includeViews ? myListTablesAndViewsCmd : myListTablesCmd );
            DataTable table = Query( cmd );

            string[] tables = new string[ table.Rows.Count ];
            for ( int i = 0; i < table.Rows.Count; ++i )
            {
                tables[ i ] = (string)table.Rows[ i ][ 0 ];
            }

            table.Dispose();

            return tables;
        }

        #region Protected/private members

        protected void Open( bool createNew )
        {
            if ( !IsOpen() )
            {
                string connStr = String.Format( CultureInfo.InvariantCulture, "Data Source={0};New={1};Enlist=False;Version=3", myDBFile, createNew );
                myConnection = new SQLiteConnection( connStr );

                myConnection.Open();
            }

            // enlist connection into transaction if any available
            Transaction trans = Transaction.Current;
            if ( trans != null && trans != myTransaction )
            {
                myTransaction = trans;
                myConnection.EnlistTransaction( myTransaction );
            }
        }

        protected void Close()
        {
            if ( myTransaction != null )
            {
                myTransaction.Dispose();
                myTransaction = null;
            }

            if ( myConnection != null )
            {
                myConnection.Dispose();
                myConnection = null;
            }
        }

        protected bool IsOpen()
        {
            return ( myConnection != null && myConnection.State == ConnectionState.Open );
        }

        #endregion
    }
}
