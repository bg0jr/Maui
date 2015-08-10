using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Entities.Descriptors;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Blade.Shell.Forms;

namespace Maui.Shell.Forms
{
    public class StockArguments : ISupportInitialize
    {
        [CatalogArgument]
        public StockCatalogDescriptor Catalog
        {
            get;
            set;
        }

        [IsinArgument]
        public string Isin
        {
            get;
            set;
        }

        [StockExchangeArgument]
        public string StockExchange
        {
            get;
            set;
        }

        [NotEmpty]
        public IEnumerable<StockDescriptor> Stocks
        {
            get
            {
                return Catalog.Stocks;
            }
        }

        public void BeginInit()
        {
            Catalog = new StockCatalogDescriptor();
        }

        public void EndInit()
        {
            if ( Isin != null )
            {
                Catalog.Stocks.Add( new StockDescriptor( Isin, StockExchange ) );
            }
        }
    }
}
