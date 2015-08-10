using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Dynamics.Types;
using Maui.Entities;

namespace Maui.Dynamics
{
    public static class StockCatalogExtensions
    {
        public static void ForEach( this StockCatalog catalog, Action body )
        {
            catalog.ForEach( stock => body() );
        }

        public static void ForEach( this StockCatalog catalog, Action<StockHandle> body )
        {
            if ( catalog == null )
            {
                throw new Exception( "Catalog required" );
            }

            using ( var guard = new NestedScopeGuard() )
            {
                foreach ( var tsh in catalog.TradedStocks )
                {
                    guard.Scope.Stock = new StockHandle( tsh );

                    body( guard.Scope.Stock );
                }
            }
        }
    }
}
