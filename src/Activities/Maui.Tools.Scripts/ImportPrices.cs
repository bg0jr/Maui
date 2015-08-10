using System;
using System.Linq;
using Maui.Data.Recognition.DatumLocators;
using Maui.Dynamics;
using Maui.Dynamics.Presets;
using Maui.Entities;
using Maui.Logging;
using Maui.Shell;
using Maui.Tasks;
using Maui.Tasks.Dynamics;
using Maui.Dynamics.Shell;
using Maui.Shell.Forms;
using Blade.Validation;
using System.ComponentModel.DataAnnotations;
using Blade.Logging;
using Blade.Shell.Forms;

namespace Maui.Tools.Scripts
{
    public class ImportPrices : MslScriptBase
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( ImportPrices ) );

        private IEntityRepository myTom;

        [UserControl, Required, ValidateObject]
        public StockArguments StockArgs
        {
            get;
            set;
        }

        protected override void Interpret()
        {
            DatumDefines.StockPrice.Create();

            var stockList = new StockListBuilder();
            stockList.Add( StockArgs.Stocks );

            if ( !stockList.Stocks.Any() )
            {
                Console.WriteLine( "No stocks given" );
                return;
            }

            using ( myTom = Engine.ServiceProvider.CreateEntityRepository() )
            {
                foreach ( var stock in stockList.Stocks )
                {
                    this.Scope().Stock = stock;

                    Import();
                }
            }
        }

        private void Import()
        {
            Console.WriteLine( "Importing prices for Isin={0}, StockExchange={1}", this.Scope().Stock.Isin, this.Scope().Stock.StockExchange.Symbol );
            this.Import( DatumDefines.StockPrice, true );

            // check if we need to fetch the current price separately
            var lastTradingDay = DateTime.Today;
            while ( lastTradingDay.DayOfWeek == DayOfWeek.Saturday || lastTradingDay.DayOfWeek == DayOfWeek.Sunday )
            {
                lastTradingDay = lastTradingDay.AddDays( -1 );
            }

            // refetch traded stock as the tom context the stock is coming from might be disposed already
            var tradedStock = myTom.GetObjectByKey<TradedStock>( this.Scope().Stock.TradedStock.EntityKey );

            var lastPrice = tradedStock.StockPrices
                .OrderByDescending( price => price.Date )
                .FirstOrDefault();

            if ( lastPrice == null )
            {
                // no prices available for that stock - errors should already be reported on shell
                return;
            }

            if ( lastPrice.Date == lastTradingDay )
            {
                return;
            }

            // we need to fetch the current price separately
            ImportCurrentPrice( lastTradingDay );
        }

        private void ImportCurrentPrice( DateTime lastTradingDay )
        {
            Console.WriteLine( "  importing current price separately" );

            var tradedStock = myTom.GetObjectByKey<TradedStock>( this.Scope().Stock.TradedStock.EntityKey );

            // TODO: we need to pass the stock exchange !!
            var currentPriceProvider = Interpreter.Context.DatumProviderFactory.Create( DatumLocatorDefinitions.CurrentPrice );
            var closePrice = currentPriceProvider.FetchSingle<double>();

            // TODO: we cannot really make sure that the price is from this day
            var price = new StockPrice( tradedStock, lastTradingDay, null, null, null, closePrice.Value, null );
            price.DatumOrigin = myTom.DatumOrigins.GetOrCreate( closePrice.Site.Name );
            price.Timestamp = Maui.TypeConverter.DateToString( DateTime.Today ); // TODO: this should be handled by TOM internally
            myTom.StockPrices.AddObject( price );

            myTom.SaveChanges();
        }
    }
}
