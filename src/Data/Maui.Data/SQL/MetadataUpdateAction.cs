using Maui.Logging;

namespace Maui.Data.SQL
{
    public class MetadataUpdateAction : IDBUpdateAction
    {
        public MetadataUpdateAction( string key, string value )
        {
            Key = key;
            Value = value;
        }

        public string Key
        {
            get;
            private set;
        }

        public string Value
        {
            get;
            private set;
        }

        public void Execute( IDatabaseSC db )
        {
            DeleteKey( db );
            InsertKey( db );
        }

        private void DeleteKey( IDatabaseSC db )
        {
            var cmd = string.Format( "DELETE FROM db_metadata where key == '{0}';", Key );
            db.Execute( cmd );
        }

        private void InsertKey( IDatabaseSC db )
        {
            var cmd = string.Format( "INSERT INTO db_metadata VALUES ( '{0}', '{1}' );", Key, Value );
            db.Execute( cmd );
        }

        public void Rollback( IDatabaseSC db )
        {
            DeleteKey( db );
        }
    }
}
