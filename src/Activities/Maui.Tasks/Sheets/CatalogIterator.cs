using System;
using System.Collections.Generic;
using Maui.Entities;
using Maui.Data.Sheets;

namespace Maui.Tasks.Sheets
{
    /// <summary>
    /// Iterator for catalogs plugin.
    /// </summary>
    public class CatalogIterator : IsinIterator
    {
        private string myNameColumn;

        /// <summary/>
        public CatalogIterator( IWorksheet worksheet, int headerRow )
            : base( worksheet, headerRow )
        {
            DetectHeaderColumns();
        }

        /// <summary/>
        protected override void OnHeaderColumn( char column, string header )
        {
            base.OnHeaderColumn( column, header );

            if ( header == "Name" )
            {
                myNameColumn = column.ToString();
            }
        }

        /// <summary/>
        protected override void OnHeaderDetected()
        {
            base.OnHeaderDetected();

            StatusColumn = IsinColumn;
        }

        /// <summary>
        /// Fills the catalog sheet with the given stocks.
        /// </summary>
        public void FillStocks( IEnumerable<StockHandle> stocks )
        {
            int row = HeaderRow + 1;
            foreach ( var stock in stocks )
            {
                Worksheet.SetCell( IsinColumn + row, stock.Isin );
                if ( myNameColumn != null )
                {
                    Worksheet.SetCell( myNameColumn + row, stock.Name );
                }

                row++;
            }
        }
    }
}
