using System;
using System.Linq;
using Maui.Data.Recognition.DatumLocators;
using Maui.Dynamics;
using Maui.Dynamics.Shell;
using Maui.Logging;
using Maui.Shell.Forms;
using Maui.Tasks;
using Blade.Validation;
using System.ComponentModel.DataAnnotations;
using Blade.Logging;
using Blade.Shell.Forms;

namespace Maui.Tools.Scripts
{
    public class Ticker : MslScriptBase
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( Ticker ) );

        [UserControl, Required, ValidateObject]
        public StockArguments StockArgs
        {
            get;
            set;
        }

        protected override void Interpret()
        {
            var stockList = new StockListBuilder();
            stockList.Add( StockArgs.Stocks );

            if ( !stockList.Stocks.Any() )
            {
                Console.WriteLine( "No stocks given" );
                return;
            }

            foreach ( var stock in stockList.Stocks )
            {
                this.Scope().Stock = stock;

                Import();
            }
        }

        private void Import()
        {
            Console.WriteLine( "Fetching prices for Isin={0}, StockExchange={1}", this.Scope().Stock.Isin, this.Scope().Stock.StockExchange.Symbol );

            var currentPriceProvider = Interpreter.Context.DatumProviderFactory.Create( DatumLocatorDefinitions.CurrentPrice );
            var closePrice = currentPriceProvider.FetchSingle<double>();

            Console.WriteLine( "Price: {0}", closePrice.Value );
        }
    }
}
