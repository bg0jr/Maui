using System;
using System.Data;
using Blade.Reflection;
using Maui.Data.Recognition;
using Maui.Entities;
using Maui.Dynamics.Data;
using Maui.Logging;
using Maui.Data.Recognition.Providers;
using Maui.Dynamics;
using Blade.Logging;

namespace Maui.Tasks.Dynamics
{
    public static class FetchFunction
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( FetchFunction ) );

        public static SingleResultValue<T> FetchSingle<T>( this IMslScript script, string isin, Datum datum )
        {
            return FetchSingle<T>( script, isin, datum.Name );
        }

        public static SingleResultValue<T> FetchSingle<T>( this IMslScript script, string isin, string datum )
        {
            var provider = Interpreter.Context.DatumProviderFactory.Create( datum );
            if ( provider == null )
            {
                throw new Exception( "No data provider found for datum '" + datum + "'" );
            }

            // as we have no real stock yet (we might in process of fetching standing data
            // to get such stock) so we have to overwrite default lookup behaviour
            using ( var trans = new ObjectTransaction() )
            {
                trans.Register( new ScopeTransactionAdapter( Interpreter.Context.Scope ) );

                Interpreter.Context.Scope[ "stock.isin" ] = isin;

                return provider.FetchSingle<T>();
            }
        }

        public static DataTable Fetch( this IMslScript script, StockHandle stock, Datum datum )
        {
            return Fetch( script, stock, datum.Name );
        }

        public static DataTable Fetch( this IMslScript script, StockHandle stock, string datum )
        {
            var provider = Interpreter.Context.DatumProviderFactory.Create( datum );
            if ( provider == null )
            {
                throw new Exception( "No data provider found for datum '" + datum + "'" );
            }

            using ( var guard = new NestedScopeGuard() )
            {
                guard.Scope.Stock = stock;

                return provider.Fetch().ResultTable;
            }
        }
    }
}
