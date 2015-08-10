using System;
using System.Linq;
using Maui.Entities;

namespace Maui.Dynamics.Data
{
    public static class QueryTool
    {
        public static ScopedTable Select( this MauiX.IQuery self, TableSchema schema, StockHandle stock )
        {
            return self.Select( schema, stock, DateClause.All );
        }

        public static ScopedTable Select( this MauiX.IQuery self, TableSchema schema, StockHandle stock, DateClause dateClause )
        {
            var mgr = Engine.ServiceProvider.TomScripting().GetManager( schema.Name );

            return mgr.Query( stock.GetId( schema.OwnerIdColumn ), dateClause, OriginClause.Default );
        }

        public static ScopedTable GetPriceData( this MauiX.IQuery self, StockHandle stock, DateClause dateClause )
        {
            return self.Select( Engine.ServiceProvider.TomScripting().GetManager(
                Config.Instance.DailyStockPriceTable ).Schema, stock, dateClause );
        }

        public static StockPriceSeries GetStockPrices( this MauiX.IQuery self, StockHandle stock, DateClause dateClause, Timeframe timeframe )
        {
            var table = self.GetPriceData( stock, dateClause );

            var q = from row in table.Rows
                    select new StockPrice(
                        (long)row[ "id" ],
                        stock.TradedStock,
                        row.GetDate( table.Schema ),
                        row[ "open" ] != DBNull.Value ? (double?)row[ "open" ] : null,
                        row[ "high" ] != DBNull.Value ? (double?)row[ "high" ] : null,
                        row[ "low" ] != DBNull.Value ? (double?)row[ "low" ] : null,
                        (double)row[ "close" ],
                        ( row[ "volume" ] != DBNull.Value ? (int)(long)row[ "volume" ] : 0 )
                         );

            return new StockPriceSeries( stock.TradedStock, q, timeframe );
        }
    }
}
