using System;
using System.Linq;
using Blade;
using Maui;
using Maui.Entities;
using Maui.Entities.Descriptors;
using Maui.Data.Recognition.DatumLocators;
using Maui.Logging;
using Blade.Logging;

namespace Maui.Tasks
{
    public class StockCreator
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( StockCreator ) );
        
        // TODO: dublicate of Functions.Stocks.GetOrCreateStock() but we first need some
        // kind of protocol (for user and developer) before we can remove this code here
        public StockHandle Create( StockDescriptor stockDescriptor )
        {
            if ( stockDescriptor.Isin.IsNullOrTrimmedEmpty() )
            {
                throw new ArgumentException( "Isin not set" );
            }
            if ( stockDescriptor.StockExchange.IsNullOrTrimmedEmpty() )
            {
                throw new ArgumentException( "Stock exchange not set" );
            }

            myLogger.Notice( "Creating stock: {0}", stockDescriptor.Isin );

            using ( var tom = Engine.ServiceProvider.CreateEntityRepository() )
            {
                var tradedStock = tom.TradedStocks.FindTradedStockByDescription( stockDescriptor );
                if ( tradedStock != null )
                {
                    var sh = new StockHandle( tradedStock );

                    myLogger.Info( "Stock already exists: Company = {0},Isin = {1}, Symbol = {2}, Exchange = {3}",
                        sh.Company.Name, stockDescriptor.Isin, sh.TradedStock.Symbol, stockDescriptor.StockExchange );

                    return sh;
                }

                // TODO: this is somehow duplicate code from StockHandle.GetOrCreate - remove StockHandle.GetOrCreate 

                // ok - so no traded stock available for the given description - but maybe a stock is already there?
                var stock = tom.Stocks.FirstOrDefault( s => s.Isin == stockDescriptor.Isin );
                if ( stock == null )
                {
                    var companyName = stockDescriptor.Name;
                    if ( companyName.IsNullOrTrimmedEmpty() )
                    {
                        companyName = DatumLocatorDefinitions.Standing.CompanyName.FetchSingle<string>( stockDescriptor.Isin ).Value;
                    }

                    // company name is not uniq enough so lets create a new one
                    var company = new Company( companyName );
                    stock = new Stock( company, stockDescriptor.Isin );
                }

                // we got a stock so lets create a traded stock - we already checked that there is none

                // but first we need a stockexchange
                var se = tom.StockExchanges.FindBySymbolOrName( stockDescriptor.StockExchange );
                if ( se == null )
                {
                    throw new InvalidOperationException( "Could not find StockExchange by symbol or name with: " + stockDescriptor.StockExchange );
                }

                var symbol = stockDescriptor.Symbol;
                if ( symbol.IsNullOrTrimmedEmpty() )
                {
                    symbol = DatumLocatorDefinitions.Standing.StockSymbol.FetchSingle<string>( stockDescriptor.Isin ).Value;
                }
                var wpkn = DatumLocatorDefinitions.Standing.Wpkn.FetchSingle<string>( stockDescriptor.Isin ).Value;

                tradedStock = new TradedStock( stock, se );
                tradedStock.Symbol = symbol;
                tradedStock.Wpkn = wpkn;

                tom.TradedStocks.AddObject( tradedStock );

                tom.SaveChanges();

                myLogger.Info( "Created stock with: Company = {0},Isin = {1}, Symbol = {2}, Exchange = {3}",
                    stock.Company.Name, stock.Isin, symbol, stockDescriptor.StockExchange );

                return new StockHandle( tradedStock );
            }
        }
    }
}

