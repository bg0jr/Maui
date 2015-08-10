using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Entities.Descriptors;

namespace Maui.Tasks
{
    public class DefaultStockProcessingConfig : IStockProcessingConfig
    {
        public DefaultStockProcessingConfig()
        {
            Catalog = new StockCatalogDescriptor();
        }

        public StockCatalogDescriptor Catalog
        {
            get;
            set;
        }
    }
}
