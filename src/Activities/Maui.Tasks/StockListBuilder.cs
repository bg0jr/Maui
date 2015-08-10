using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Entities;
using Maui;
using Maui.Entities.Descriptors;
using Blade;
using System.IO;
using System.Xml.Linq;

namespace Maui.Tasks
{
    // TODO: adding new stocks only works with MSL interpreter at the moment 
    public class StockListBuilder
    {
        private List<StockHandle> myStocks;

        public StockListBuilder()
        {
            myStocks = new List<StockHandle>();
        }

        public IEnumerable<StockHandle> Stocks
        {
            get
            {
                return myStocks;
            }
        }

        public void Add( StockHandle stock )
        {
            myStocks.Add( stock );
        }

        public void Add( IEnumerable<StockHandle> stocks )
        {
            Add( stocks.ToArray() );
        }

        public void Add( params StockHandle[] stocks )
        {
            foreach ( var stock in stocks )
            {
                Add( stock );
            }
        }

        public void Add( StockDescriptor stockDescriptor )
        {
            var stock = FindStock( stockDescriptor );
            Add( stock );
        }

        private StockHandle FindStock( StockDescriptor stockDescriptor )
        {
            using ( var tom = Engine.ServiceProvider.CreateEntityRepository() )
            {
                var tradedStock = tom.TradedStocks.FindTradedStockByDescription( stockDescriptor );
                if ( tradedStock != null )
                {
                    return new StockHandle( tradedStock );
                }

                var creator = new StockCreator();
                return creator.Create( stockDescriptor );
            }
        }

        public void Add( IEnumerable<StockDescriptor> stocks )
        {
            Add( stocks.ToArray() );
        }

        public void Add( params StockDescriptor[] stocks )
        {
            foreach ( var stock in stocks )
            {
                Add( stock );
            }
        }

        public void Add( StockCatalogDescriptor catalog )
        {
            ResolveCatalogReference( catalog );

            foreach ( var stock in catalog.Stocks )
            {
                Add( stock );
            }
        }

        private void ResolveCatalogReference( StockCatalogDescriptor catalog )
        {
            if ( string.IsNullOrEmpty( catalog.Name ) )
            {
                return;
            }

            using ( var tom = Engine.ServiceProvider.CreateEntityRepository() )
            {
                var stockCatalog = tom.StockCatalogs.FirstOrDefault( sc => sc.Name.Equals( catalog.Name, StringComparison.OrdinalIgnoreCase ) );
                if ( stockCatalog == null )
                {
                    throw new Exception( "No such catalog in MauiDB: " + catalog.Name );
                }

                foreach ( var tradedStock in stockCatalog.TradedStocks )
                {
                    var stock = new StockHandle( tradedStock );
                    Add( stock );
                }
            }
        }
    }
}
