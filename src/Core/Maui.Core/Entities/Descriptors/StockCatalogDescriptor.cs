using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Maui.Entities.Descriptors
{
    public class StockCatalogDescriptor : ISupportInitialize
    {
        public StockCatalogDescriptor()
        {
            Stocks = new List<StockDescriptor>();
            ReferencedCatalogs = new List<StockCatalogDescriptor>();
        }

        /// <summary>
        /// Reference to an existing stock catalog in the DB.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        public List<StockDescriptor> Stocks
        {
            get;
            private set;
        }

        /// <summary>
        /// Supports merging catalogs similar to ResourceDictionaries.
        /// Use "include" markup to reference the catalogs to merge.
        /// </summary>
        public List<StockCatalogDescriptor> ReferencedCatalogs
        {
            get;
            private set;
        }

        public void BeginInit()
        {
        }

        public void EndInit()
        {
            foreach ( var catalog in ReferencedCatalogs )
            {
                foreach ( var stock in catalog.Stocks )
                {
                    Stocks.Add( stock );
                }
            }
        }
    }
}
