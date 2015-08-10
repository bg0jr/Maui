using System;
using System.Collections.Generic;
using System.Linq;
using Blade;
using Maui.Entities;
using Maui.Entities.Descriptors;
using Maui.Logging;
using Maui.Shell;
using Maui.Tasks;
using Maui.Shell.Forms;
using Blade.Validation;
using System.ComponentModel.DataAnnotations;
using Blade.Logging;
using Blade.Shell.Forms;
using Blade.Shell;

namespace Maui.Tools.Scripts
{
    public class CreateStock : ScriptBase
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( CreateStock ) );

        [UserControl, Required, ValidateObject]
        public StockArguments StockArgs
        {
            get;
            set;
        }

        protected override void Run()
        {
            var creator = new StockCreator();
            var tradedStocks = new List<TradedStock>();
            foreach ( var stock in StockArgs.Stocks )
            {
                try
                {
                    tradedStocks.Add( creator.Create( stock ).TradedStock );
                }
                catch ( Exception ex )
                {
                    myLogger.Error( ex, "Failed to create stock: {0}", stock.Isin );
                }
            }

            AddStocksToCatalog( tradedStocks, StockArgs.Catalog.Name );
        }

        private void AddStocksToCatalog( List<TradedStock> stocks, string catalog )
        {
            if ( catalog.IsNullOrTrimmedEmpty() )
            {
                myLogger.Info( "No catalog name specified" );
                return;
            }

            using ( var tom = Engine.ServiceProvider.CreateEntityRepository() )
            {
                var stockCatalog = GetOrCreateStockCatalog( tom, catalog );

                foreach ( var stock in stocks )
                {
                    // refetch traded stock as the tom context the stock is coming from might be disposed already
                    var tradedStock = tom.GetObjectByKey<TradedStock>( stock.EntityKey );
                    stockCatalog.TradedStocks.Add( tradedStock );
                }

                tom.SaveChanges();
            }
        }

        private StockCatalog GetOrCreateStockCatalog( IEntityRepository tom, string catalogName )
        {
            var stockCatalog = tom.StockCatalogs.FirstOrDefault( catalog => catalog.Name.EqualsI( catalogName ) );
            if ( stockCatalog == null )
            {
                stockCatalog = new StockCatalog( catalogName );
                tom.StockCatalogs.AddObject( stockCatalog );
            }

            return stockCatalog;
        }
    }
}
