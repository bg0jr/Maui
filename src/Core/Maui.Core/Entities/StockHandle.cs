using System;
using System.Collections.Generic;
using System.Linq;
using Blade;
using Blade.Functional;
using Blade.Collections;
using Maui;

namespace Maui.Entities
{
    [Serializable]
    public class StockHandle
    {
        private TradedStock myHandle = null;

        public StockHandle( TradedStock ts )
        {
            myHandle = ts;

            // TODO: have two factory methods? one to load on demand the other to create in memory?

            // the object context of the given traded stock will be disposed later on.
            // then we cannot access any property which requires the object context - e.g.
            // references that has not been loaded yet

            // - only check if attached to object context
            if ( myHandle.EntityState == System.Data.EntityState.Unchanged || myHandle.EntityState == System.Data.EntityState.Modified )
            {
                if ( myHandle.Stock == null )
                {
                    myHandle.StockReference.Load();
                }
                if ( myHandle.StockExchange == null )
                {
                    myHandle.StockExchangeReference.Load();
                }
                if ( myHandle.StockExchange.Currency == null )
                {
                    myHandle.StockExchange.CurrencyReference.Load();
                }
                if ( myHandle.Stock.Company == null )
                {
                    myHandle.Stock.CompanyReference.Load();
                }
            }
        }

        public bool Contains( string key )
        {
            if ( key.Equals( "traded_stock_id", StringComparison.OrdinalIgnoreCase ) ||
                key.Equals( "stock_id", StringComparison.OrdinalIgnoreCase ) ||
                key.Equals( "name", StringComparison.OrdinalIgnoreCase ) ||
                key.Equals( "symbol", StringComparison.OrdinalIgnoreCase ) ||
                key.Equals( "wpkn", StringComparison.OrdinalIgnoreCase ) ||
                key.Equals( "isin", StringComparison.OrdinalIgnoreCase ) ||
                key.Equals( "exchange.symbol", StringComparison.OrdinalIgnoreCase ) ||
                key.Equals( "stockexchange.symbol", StringComparison.OrdinalIgnoreCase ) )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object this[ string key ]
        {
            get
            {
                if ( key.Equals( "traded_stock_id", StringComparison.OrdinalIgnoreCase ) )
                {
                    return TradedStockId;
                }
                else if ( key.Equals( "stock_id", StringComparison.OrdinalIgnoreCase ) )
                {
                    return StockId;
                }
                else if ( key.Equals( "name", StringComparison.OrdinalIgnoreCase ) )
                {
                    return Name;
                }
                else if ( key.Equals( "symbol", StringComparison.OrdinalIgnoreCase ) )
                {
                    return Symbol;
                }
                else if ( key.Equals( "wpkn", StringComparison.OrdinalIgnoreCase ) )
                {
                    return Wpkn;
                }
                else if ( key.Equals( "isin", StringComparison.OrdinalIgnoreCase ) )
                {
                    return Isin;
                }
                else if ( key.Equals( "exchange.symbol", StringComparison.OrdinalIgnoreCase ) )
                {
                    return StockExchange.Symbol;
                }
                else if ( key.Equals( "stockexchange.symbol", StringComparison.OrdinalIgnoreCase ) )
                {
                    return StockExchange.Symbol;
                }

                throw new IndexOutOfRangeException( "Unknown stock property: " + key );
            }
        }

        public long GetId( string name )
        {
            return (long)this[ name ];
        }

        public long TradedStockId
        {
            get { return myHandle.Id; }
        }

        public long StockId
        {
            get { return TradedStock.Stock.Id; }
        }

        public string Isin
        {
            get { return Stock.Isin; }
        }

        public string Symbol
        {
            get { return TradedStock.Symbol; }
        }

        public string Wpkn
        {
            get { return TradedStock.Wpkn; }
        }

        public string Name
        {
            get { return Company.Name; }
        }

        public TradedStock TradedStock
        {
            get
            {
                return myHandle;
            }
        }

        public StockExchange StockExchange
        {
            get
            {
                return TradedStock.StockExchange;
            }
        }

        public Stock Stock
        {
            get
            {
                return TradedStock.Stock;
            }
        }

        public Company Company
        {
            get
            {
                return Stock.Company;
            }
        }

        public override string ToString()
        {
            return string.Format( "Name: {0}, Isin: {1}, Symbol: {2}, Wpkn: {3}",
                Name, Isin, Symbol, Wpkn );
        }

        public static StockHandle GetOrCreate( params Func<string, string>[] args_in )
        {
            return GetOrCreate<SingleOrThrowPolicy>( args_in );
        }

        public static StockHandle GetOrCreate<TPolicy>( params Func<string, string>[] args_in ) where TPolicy : IScalarPolicy, new()
        {
            var args = Hash.Create( true, args_in );
            return GetOrCreate<TPolicy>( args );
        }

        /// <summary>
        /// Tries to get a traded stock with the given parameters or creates a new one if 
        /// nothing matching could be found in the database.
        /// <remarks>
        /// Supported keys are: Name ( of the company), Isin, Wpkn, Symbol, 
        /// Exchange/StockExchange (Name or Symbol of the stock exchange)
        /// Mandatory keys for new stocks: isin, symbol, exchange
        /// </remarks>
        /// </summary>
        public static StockHandle GetOrCreate<TPolicy>( Dictionary<string, string> args_in ) where TPolicy : IScalarPolicy, new()
        {
            try
            {
                // use separate method to get a better callstack when rethrowing
                return GetOrCreateInternal<TPolicy>( args_in );
            }
            catch ( Exception ex )
            {
                foreach ( var pair in args_in )
                {
                    ex.AddContext( pair.Key, pair.Value );
                }

                throw;
            }
        }

        private static StockHandle GetOrCreateInternal<TPolicy>( Dictionary<string, string> args_in ) where TPolicy : IScalarPolicy, new()
        {
            var args = args_in.ToDictionary( pair => pair.Key.ToLower(), pair => pair.Value );

            var GetPolicy = NMemorize.Cache( () => new TPolicy() );

            using ( var tom = Engine.ServiceProvider.CreateEntityRepository() )
            {
                // lets try to find s.th. in DB first

                IEnumerable<TradedStock> tradedStocks = null;

                if ( args.ContainsKey( "isin" ) )
                {
                    tradedStocks = tom.TradedStocks.FindByIsin( args[ "isin" ] );
                }

                if ( tradedStocks == null && args.ContainsKey( "wpkn" ) )
                {
                    tradedStocks = tom.TradedStocks.FindByWpkn( args[ "wpkn" ] );
                }

                if ( tradedStocks == null && args.ContainsKey( "symbol" ) )
                {
                    tradedStocks = tom.TradedStocks.FindBySymbol( args[ "symbol" ] );
                }

                if ( tradedStocks != null && tradedStocks.Any() )
                {
                    // always filter so that we can check wether all given 
                    // parameters fit well
                    var filteredTradedStocks = tradedStocks.Where( ts =>
                        (!args.ContainsKey( "wpkn" ) || ts.Wpkn == args[ "wpkn" ]) &&
                        (!args.ContainsKey( "symbol" ) || ts.Symbol == args[ "symbol" ]) );

                    if ( filteredTradedStocks.Any() )
                    {
                        return new StockHandle( GetPolicy().Single( filteredTradedStocks ) );
                    }
                }

                // ok we did not find anything so lets create a new stock

                // check required keys
                if ( !args.ContainsKey( "isin" ) )
                {
                    throw new ArgumentException( "Isin required when creating new stocks" );
                }
                if ( !args.ContainsKey( "symbol" ) )
                {
                    throw new ArgumentException( "Symbol required when creating new stocks" );
                }
                if ( !(args.ContainsKey( "exchange" ) || args.ContainsKey( "stockexchange" )) )
                {
                    throw new ArgumentException( "Exchange/StockExchange required when creating new stocks" );
                }
                if ( !(args.ContainsKey( "name" ) || args.ContainsKey( "companyname" )) )
                {
                    throw new ArgumentException( "Name/CompanyName required when creating new stocks" );
                }

                var companyName = GetOrDefault( args, "name" ) ?? GetOrDefault( args, "companyname" );
                var company = tom.Companies.GetOrCreate<TPolicy>( companyName );

                var stock = new Stock( company, args[ "isin" ] );
                tom.Stocks.AddObject( stock );

                var exchange = GetOrDefault( args, "exchange" ) ?? GetOrDefault( args, "stockexchange" );
                var se = tom.StockExchanges.FindBySymbolOrName( exchange );
                if ( se == null )
                {
                    throw new InvalidOperationException( "Could not find StockExchange by symbol or name with: " + exchange );
                }

                var tradedStock = new TradedStock( stock, se )
                {
                    Wpkn = GetOrDefault( args, "wpkn" ),
                    Symbol = GetOrDefault( args, "symbol" )
                };
                tom.TradedStocks.AddObject( tradedStock );

                tom.SaveChanges();

                return new StockHandle( tradedStock );
            }
        }

        /// <summary>
        /// Returns the specified value if it exists, default of the 
        /// value type otherwise.
        /// </summary>
        private static V GetOrDefault<K, V>( IDictionary<K, V> dict, K key )
        {
            if ( dict.ContainsKey( key ) )
            {
                return dict[ key ];
            }

            return default( V );
        }
    }

    /// <summary>
    /// Contains helper methods to deal with hashes and dictionaries.
    /// </summary>
    public static class Hash
    {
  
        /// <summary>
        /// Creates a hash of the specified type using lambda expressions.
        /// <remarks>
        /// This is just to have a ruby like syntax for creating dictionaries.
        /// see: http://blog.bittercoder.com/PermaLink,guid,206e64d1-29ae-4362-874b-83f5b103727f.aspx
        /// </remarks>
        /// </summary>
        public static Dictionary<string, T> Create<T>( params Func<string, T>[] args ) where T : class
        {
            return Create( false, args );
        }

        /// <summary>
        /// Creates a hash of the specified type using lambda expressions.
        /// <remarks>
        /// This is just to have a ruby like syntax for creating dictionaries.
        /// see: http://blog.bittercoder.com/PermaLink,guid,206e64d1-29ae-4362-874b-83f5b103727f.aspx
        /// </remarks>
        /// </summary>
        public static Dictionary<string, T> Create<T>( bool makeKeysLowerCase, params Func<string, T>[] args ) where T : class
        {
            var items = new Dictionary<string, T>();
            foreach ( var func in args )
            {
                var value = func( null );
                var key = func.Method.GetParameters()[ 0 ].Name;
                if ( makeKeysLowerCase )
                {
                    key = key.ToLowerInvariant();
                }
                items.Add( key, value );
            }
            return items;
        }
    }
}
