using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using Blade.Collections;
using Maui.Entities.Descriptors;

namespace Maui.Entities
{
    public static class TomQueries
    {
        public static IEnumerable<TradedStock> FindByIsin( this IObjectSet<TradedStock> tradedStocks, string isin )
        {
            return tradedStocks.Where( ts => ts.Stock.Isin == isin ).ToList();
        }

        public static IEnumerable<TradedStock> FindByWpkn( this IObjectSet<TradedStock> tradedStocks, string wpkn )
        {
            return tradedStocks.Where( s => s.Wpkn == wpkn ).ToList();
        }

        public static IEnumerable<TradedStock> FindBySymbol( this IObjectSet<TradedStock> tradedStocks, string symbol )
        {
            return tradedStocks.Where( s => s.Symbol == symbol ).ToList();
        }

        /// <summary>
        /// Returns the StockExchange with the given description.
        /// First searched by symbol, then by name.
        /// Returns null if nothing has been found.
        /// </summary>
        public static StockExchange FindBySymbolOrName( this IObjectSet<StockExchange> stockExchanges, string symbolOrName )
        {
            var exchange = stockExchanges.FirstOrDefault( se => se.Symbol == symbolOrName );
            if ( exchange != null )
            {
                return exchange;
            }

            return stockExchanges.FirstOrDefault( se => se.Name == symbolOrName );
        }

        public static Company GetOrCreate( this IObjectSet<Company> companies, string name )
        {
            return companies.GetOrCreate<SingleOrThrowPolicy>( name );
        }

        public static Company GetOrCreate<TPolicy>( this IObjectSet<Company> companies, string name ) where TPolicy : IScalarPolicy, new()
        {
            var companiesWithName = companies.Where( c => c.Name.Contains( name ) ).ToList();
            if ( companiesWithName != null && companiesWithName.Any() )
            {
                return new TPolicy().Single( companiesWithName );
            }

            var company = new Company( name );
            companies.AddObject( company );

            return company;
        }

        public static DatumOrigin GetOrCreate( this IObjectSet<DatumOrigin> origins, string name )
        {
            var origin = origins.FirstOrDefault( o => o.Name.Equals( name, StringComparison.OrdinalIgnoreCase ) );
            if ( origin != null )
            {
                return origin;
            }

            origin = new DatumOrigin( name );
            origins.AddObject( origin );

            return origin;
        }

        public static TradedStock FindTradedStockByDescription( this IObjectSet<TradedStock> tradedStocks, StockDescriptor stockDescriptor )
        {
            // TODO: shouldnt we use single here?
            var tradedStock = tradedStocks
                .Where( ts => ts.Stock.Isin == stockDescriptor.Isin )
                .FirstOrDefault( ts => ts.StockExchange.Symbol == stockDescriptor.StockExchange || ts.StockExchange.Name == stockDescriptor.StockExchange );
            return tradedStock;
        }

    }
}
