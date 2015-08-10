using System;
using System.Linq;
using Maui;
using Maui.Data.Recognition;
using Maui.Dynamics.Data;
using Maui.Dynamics;

namespace Maui.Tasks.Dynamics
{
    public static class Prices
    {
        public static double Close()
        {
            var mgr = Interpreter.Context.DailyStockPriceManager;

            long ownerId = Interpreter.Context.Scope.Stock.GetId( mgr.Schema.OwnerIdColumn );

            var date = DateTime.Now.GetMostRecentTradingDay();

            Func<ScopedTable> Fetch = () => mgr.Query( ownerId, new DateClause( date ), OriginClause.Default );

            var rows = Fetch().Rows.ToList();

            if ( rows.Count == 0 )
            {
                // is there a data provider available
                var dataProvider = Interpreter.Context.DatumProviderFactory.Create( "stock_price" );
                if ( dataProvider != null )
                {
                    // run import
                    using ( var guard = new NestedScopeGuard() )
                    {
                        // set new "from" and "to" so that outer values are not modified
                        guard.Scope.From = date;
                        guard.Scope.To = date;

                        ImportFunction.Import( null, "stock_price" );
                    }
                }
            }

            // retry getting data
            rows = Fetch().Rows.ToList();

            if ( rows.Count == 0 )
            {
                // TODO: if we want some user input then we need a data provider for that
                Console.Write( "Pls enter a close price: " );
                return double.Parse( Console.ReadLine() );
            }
            else if ( rows.Count > 1 )
            {
                throw new Exception( "Don't know how to handle multiple values" );
            }

            return (double)rows[ 0 ][ "close" ];
        }
    }
}

