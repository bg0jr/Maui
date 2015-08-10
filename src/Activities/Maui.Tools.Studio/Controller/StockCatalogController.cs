using System;
using System.ComponentModel.Composition;
using Maui.Entities;
using Maui.Tools.Studio.ViewModel;

namespace Maui.Tools.Studio.Controller
{
    public class StockCatalogController : AbstractEntityGroupingController
    {
        private TomViewModel myTom;

        [ImportingConstructor]
        public StockCatalogController( TomViewModel tom, EntityGroupingManager groupingMgr )
            : base( groupingMgr )
        {
            myTom = tom;
        }

        public override void CreateGroup()
        {
            myTom.StockCatalogs.Add( new StockCatalog( TomViewModel.UNDEFINED_ENTITY_NAME ) );
        }

        public override void Delete( object group, object item )
        {
            if ( item == null )
            {
                throw new ArgumentNullException( "item" );
            }

            {
                var catalog = item as StockCatalog;
                if ( catalog != null )
                {
                    myTom.StockCatalogs.Remove( catalog );
                    return;
                }
            }

            {
                var tradedStock = item as TradedStock;
                if ( tradedStock != null )
                {
                    var catalog = (StockCatalog)group;
                    catalog.TradedStocks.Remove( tradedStock );
                    return;
                }
            }

            throw new NotSupportedException( "Cannot delete items of type: " + item.GetType() );
        }
    }
}
