using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Entities;
using Maui;
using System.ComponentModel.Composition;
using Microsoft.Practices.Unity;
using System.Transactions;
using Maui.Logging;
using Blade.Logging;

namespace Maui.Tools.Studio.ViewModel
{
    [Export( typeof( Grouping ) )]
    public class StockCatalogGrouping : Grouping<StockCatalog, TradedStock>
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( StockCatalogGrouping ) );
        
        private TomViewModel myTom;

        [ImportingConstructor]
        public StockCatalogGrouping( TomViewModel tom )
        {
            myTom = tom;
        }

        protected override void Release( StockCatalog group, TradedStock element )
        {
            myLogger.Info( "{0} removed from {1}", element.Symbol, group.Name );

            group.TradedStocks.Remove( element );
        }

        protected override void MoveElementToGroup( StockCatalog group, TradedStock element, StockCatalog oldGroup )
        {
            myLogger.Info( "{0} moved to {1}", element.Symbol, group.Name );

            oldGroup.TradedStocks.Remove( element );
            group.TradedStocks.Add( element );
        }

        protected override void AddElemntToGroup( StockCatalog group, TradedStock element )
        {
            myLogger.Info( "{0} added to {1}", element.Symbol, group.Name );
            
            group.TradedStocks.Add( element );
        }
    }
}
