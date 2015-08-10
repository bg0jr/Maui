using System.Linq;
using System.Transactions;
using Blade;
using Maui.Entities;
using Maui;
using Maui.Logging;
using Maui.Shell;
using Blade.Logging;
using Blade.Shell;

namespace Maui.Data.Scripts
{
    public class ImportStockExchanges : ScriptBase
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( ImportStockExchanges ) );
        
        protected override void Run()
        {
            using ( var tom = Engine.ServiceProvider.CreateEntityRepository() )
            {
                if ( tom.StockExchanges.Any() )
                {
                    myLogger.Info( "StockExchanges already imported. Skipping ..." );
                    return;
                }

                using ( var trans = new TransactionScope() )
                {
                    var euro = tom.Currencies.First( c => c.Name == "Euro" );
                    var dollar = tom.Currencies.First( c => c.Name == "Dollar" );
                    var pounds = tom.Currencies.First( c => c.Name == "Pounds" );
                    var swissFrancs = tom.Currencies.First( c => c.Symbol == "CHF" );

                    tom.StockExchanges.AddObject( new StockExchange( name: "Xetra", symbol: "DE", currency: euro ) );
                    tom.StockExchanges.AddObject( new StockExchange( name: "Frankfurt", symbol: "F", currency: euro ) );
                    tom.StockExchanges.AddObject( new StockExchange( name: "Stuttgart", symbol: "SG", currency: euro ) );
                    tom.StockExchanges.AddObject( new StockExchange( name: "Munich", symbol: "MU", currency: euro ) );
                    tom.StockExchanges.AddObject( new StockExchange( name: "Berlin-Bremen", symbol: "BE", currency: euro ) );
                    tom.StockExchanges.AddObject( new StockExchange( name: "Düsseldorf", symbol: "DU", currency: euro ) );
                    tom.StockExchanges.AddObject( new StockExchange( name: "Hamburg", symbol: "HM", currency: euro ) );
                    tom.StockExchanges.AddObject( new StockExchange( name: "Hannover", symbol: "HA", currency: euro ) );

                    tom.StockExchanges.AddObject( new StockExchange( name: "American Stock Exchange", symbol: "AMEX", currency: dollar ) );
                    tom.StockExchanges.AddObject( new StockExchange( name: "Chicago Mercantile Exchange", symbol: "CME", currency: dollar ) );
                    tom.StockExchanges.AddObject( new StockExchange( name: "National Association of Securities Dealers Automated Quotations", symbol: "NASDAQ", currency: dollar ) );
                    tom.StockExchanges.AddObject( new StockExchange( name: "New York Mercantile Exchange", symbol: "NYMEX", currency: dollar ) );
                    tom.StockExchanges.AddObject( new StockExchange( name: "New York Stock Exchange", symbol: "NYSE", currency: dollar ) );

                    tom.StockExchanges.AddObject( new StockExchange( name: "London Stock Exchange", symbol: "L", currency: pounds ) );
                    tom.StockExchanges.AddObject( new StockExchange( name: "London Metal Exchange", symbol: "LME", currency: pounds ) );
                    tom.StockExchanges.AddObject( new StockExchange( name: "Paris", symbol: "PA", currency: euro ) );
                    tom.StockExchanges.AddObject( new StockExchange( name: "Amsterdam", symbol: "AS", currency: euro ) );
                    tom.StockExchanges.AddObject( new StockExchange( name: "Wien", symbol: "VI", currency: euro ) );
                    tom.StockExchanges.AddObject( new StockExchange( name: "Mailand", symbol: "MI", currency: euro ) );
                    tom.StockExchanges.AddObject( new StockExchange( name: "Schweiz", symbol: "SW", currency: swissFrancs ) );
                    tom.StockExchanges.AddObject( new StockExchange( name: "Virt-X", symbol: "VX", currency: euro ) );
                    tom.StockExchanges.AddObject( new StockExchange( name: "EuroTLX", symbol: "TI", currency: euro ) );
                    tom.StockExchanges.AddObject( new StockExchange( name: "Paris", symbol: "PA", currency: euro ) );
                    tom.StockExchanges.AddObject( new StockExchange( name: "Paris", symbol: "PA", currency: euro ) );

                    // 'Hong Kong Stock Exchange', 'SEHK', 0);"); // HKSE, HKEX
                    // 'Tokyo Stock Exchange', 'TSE', 0);");
                    // 'Shanghai Stock Exchange', 'SSE', 0);");
                    // 'Toronto Stock Exchange', 'TSX', 0);");

                    tom.SaveChanges();
                    trans.Complete();
                }
            }
        }
    }
}
