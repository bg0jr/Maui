using System.Collections.Generic;
using System.Data;
using Maui.Data.Recognition;
using Maui.Entities;
using Maui;
using Maui.Data.Recognition.Spec;

namespace Maui.Tasks
{
    public static class DatumLocatorExtensions
    {
        public static SingleResultValue<T> FetchSingle<T>( this DatumLocator datum, string isin )
        {
            var lut = new Dictionary<string, string>()
            { 
                { "stock.isin", isin }
            };
            var provider = CreateProvider( datum, new EvaluatorPolicy( lut ) );

            return provider.FetchSingle<T>();
        }

        public static SingleResultValue<T> FetchSingle<T>( this DatumLocator datum, StockHandle stock )
        {
            var provider = CreateProvider( datum, new StockLookupPolicy( stock ) );
            return provider.FetchSingle<T>();
        }

        public static DataTable Fetch( this DatumLocator datum, string isin )
        {
            var lut = new Dictionary<string, string>()
            { 
                { "stock.isin", isin }
            };

            var provider = CreateProvider( datum, new EvaluatorPolicy( lut ) );
            return provider.Fetch().ResultTable;
        }

        public static DataTable Fetch( this DatumLocator datum, StockHandle stock )
        {
            var provider = CreateProvider( datum, new StockLookupPolicy( stock ) );
            return provider.Fetch().ResultTable;
        }

        private static IDatumProvider CreateProvider( DatumLocator datum, IFetchPolicy fetchPolicy )
        {
            var factory = new DatumProviderFactory( Engine.ServiceProvider.Browser(), fetchPolicy );
            return factory.Create( datum );
        }
    }
}
