using System;
using System.Data;
using System.Data.Common;
using Maui;

namespace Maui.Data.SQL
{
    public interface IDatabaseSC : IServiceComponent
    {
        string GetDBType( Type type );
        string ConnectionUri { get; }

        DbCommand CreateCommand();
        DbCommand CreateCommand( string sql );

        bool ExistsDatabase();
        void CreateDatabase();

        void Execute( string sql );
        void Execute( DbCommand cmd );
        void Execute( DbCommand cmd, params object[] values );

        /// <summary>
        /// Returns the id of the inserted record
        /// </summary>
        long Insert( string sql );

        /// <summary>
        /// Returns the id of the inserted record
        /// </summary>
        long Insert( DbCommand cmd );

        /// <summary>
        /// Returns the id of the inserted record
        /// </summary>
        long Insert( DbCommand cmd, params object[] values );

        object QueryScalar( string sql );
        object QueryScalar( DbCommand cmd );
        object QueryScalar( DbCommand cmd, params object[] values );

        DataTable Query( string sql );
        DataTable Query( DbCommand cmd );
        DataTable Query( DbCommand cmd, params object[] values );

        bool ExistsTable( string tableName );
        bool ExistsView( string tableName );
        string[] GetTables( bool includeViews );
    }
}
