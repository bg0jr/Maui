using Maui.Logging;
using Blade.Logging;

namespace Maui.Data.SQL
{
    public class CreateTableAction : IDBUpdateAction
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( CreateTableAction ) );
        
        public CreateTableAction( string tableName, string createCmd )
        {
            TableName = tableName;
            CreateCmd = createCmd;
        }

        public string TableName
        {
            get;
            private set;
        }

        public string CreateCmd
        {
            get;
            private set;
        }

        public void Execute( IDatabaseSC db )
        {
            if ( db.ExistsTable( TableName ) )
            {
                myLogger.Debug( "Table {0} already exists. Skipping", TableName );
                return;
            }

            myLogger.Debug( "Creating table {0}", TableName );
            db.Execute( CreateCmd );
        }

        public void Rollback( IDatabaseSC db )
        {
            myLogger.Debug( "Dropping table {0}", TableName );
            db.Execute( "drop table " + TableName );
        }
    }
}
