using Maui;

namespace Maui.Dynamics.Data
{
    public interface ITableManager
    {
        string Name { get; }

        TableSchema Schema { get; }

        void CreateTable();

        /// <summary>
        /// Get data by the "owner" id.
        /// </summary>
        ScopedTable Query( long ownerId );

        /// <summary>
        /// Get data by the "owner id" and "date".
        /// If the table has no "date" column the date clause is ignored.
        /// </summary>
        ScopedTable Query( long ownerId, DateClause dateClause );

        /// <summary>
        /// Get data by the "owner id", "date" and "datum origin".
        /// If the table has no "date" column the date clause is ignored.
        /// If the table has no "datum origin" column the origin clause is ignored.
        /// </summary>
        ScopedTable Query( long ownerId, DateClause dateClause, OriginClause originClause );

        /// <summary>
        /// Removes everything from the table.
        /// </summary>
        void Clear();

        //StockPrice FindMostRecent( TradedStockId tradedStockId, StockPriceType type, DatumOriginId datumOriginId );
    }
}
