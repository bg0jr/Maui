using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Entities.Descriptors;

namespace Maui.Tasks
{
    public interface IStockProcessingConfig
    {
        StockCatalogDescriptor Catalog
        {
            get;
            set;
        }
    }
}
