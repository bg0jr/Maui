using System.Collections.Generic;
using System.Data;
using System.Linq;
using Blade;
using Blade.Data;
using Blade.Collections;
using Maui.Data;
using Maui.Data.SQL;
using Maui;

namespace Maui.Dynamics.Data
{
    public class ScriptingInterface : ManagedObject
    {
        private IDatabaseSC myPersistentDB = null;
        private DataSet myInMemoryDB = null;
        private Dictionary<string, ITableManager> myManagers = null;

        private static string[] STATIC_TABLES = {"stock","traded_stock","company",
            "stock_exchange", "currency", "datum_origin", "sector", "catalog",
            "catalog_contents", 
            "db_metadata",
            "sqlite_sequence" };

        public void Init( ServiceProvider serviceProvider )
        {
            myPersistentDB = serviceProvider.Database();
            myInMemoryDB = new DataSet( "InMemoryDB" );
            myManagers = new Dictionary<string, ITableManager>();
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
                    myInMemoryDB.Tables.ToSet().Foreach( t => t.Dispose() );
                    myInMemoryDB.Tables.Clear();
                    myInMemoryDB.Dispose();

                    foreach ( ITableManager mgr in myManagers.Values )
                    {
                        mgr.TryDispose();
                    }
                    myManagers.Clear();
                }

                myPersistentDB = null;
                myInMemoryDB = null;
                myManagers = null;
            }
            finally
            {
                base.Dispose( disposing );
            }
        }

        public bool ExistsTable( string name )
        {
            return myInMemoryDB.Tables.Contains( name ) || myPersistentDB.ExistsTable( name );
        }

        public ITableManager GetManager( string tableName )
        {
            ITableManager mgr = null;

            if ( myManagers.ContainsKey( tableName ) )
            {
                mgr = myManagers[ tableName ];
            }
            else if ( myInMemoryDB.Tables.Contains( tableName ) )
            {
                mgr = new InMemoryTableManager( myInMemoryDB, myInMemoryDB.Tables[ tableName ] );
                myManagers.Add( tableName, mgr );
            }
            else if ( myPersistentDB.ExistsTable( tableName ) )
            {
                mgr = new PersistentTableManager( myPersistentDB, tableName );
                myManagers.Add( tableName, mgr );
            }

            return mgr;
        }

        public ITableManager GetManager( TableSchema schema )
        {
            ITableManager mgr = null;

            if ( myManagers.ContainsKey( schema.Name ) )
            {
                mgr = myManagers[ schema.Name ];
            }
            else if ( schema.IsPersistent )
            {
                mgr = new PersistentTableManager( myPersistentDB, schema );
                myManagers.Add( schema.Name, mgr );
            }
            else
            {
                mgr = new InMemoryTableManager( myInMemoryDB, schema );
                myManagers.Add( schema.Name, mgr );
            }

            return mgr;
        }

        /// <summary>
        /// Returns all tables in the Tom DB omitting the ones for the static
        /// TOM part (e.g. stocks and companies table)
        /// </summary>
        public IEnumerable<string> GetTables()
        {
            return myPersistentDB.GetTables( false )
                .Where( t => !STATIC_TABLES.Contains( t.ToLower() ) )
                .Concat( myInMemoryDB.Tables.ToSet().Select( table => table.TableName ) );
        }
    }
}
