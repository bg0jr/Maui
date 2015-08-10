using Maui.Logging;

namespace Maui.Data.SQL
{
    public class DBUpdateAction : IDBUpdateAction
    {
        public DBUpdateAction( string updateCmd, string rollbackCmd )
        {
            UpdateCmd = updateCmd;
            RollbackCmd = rollbackCmd;
        }

        public string UpdateCmd
        {
            get;
            private set;
        }

        public string RollbackCmd
        {
            get;
            private set;
        }

        public void Execute( IDatabaseSC db )
        {
            db.Execute( UpdateCmd );
        }

        public void Rollback( IDatabaseSC db )
        {
            db.Execute( RollbackCmd );
        }
    }
}
