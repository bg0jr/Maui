using Maui.Logging;

namespace Maui.Data.SQL
{
    public interface IDBUpdateAction
    {
        void Execute( IDatabaseSC db );
        void Rollback( IDatabaseSC db );
    }
}
