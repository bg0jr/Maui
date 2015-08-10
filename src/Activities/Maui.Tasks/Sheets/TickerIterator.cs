using System;
using Maui.Data.Sheets;

namespace Maui.Tasks.Sheets
{
    /// <summary>
    /// Iterator for ticker plugin.
    /// The header has to contain at least "Isin", "CurrentPrice" and "Error"
    /// </summary>
    public class TickerIterator : IsinIterator
    {
        private string myCurrentPriceColumn;

        /// <summary/>
        public TickerIterator( IWorksheet worksheet, int headerRow )
            : base( worksheet, headerRow )
        {
        }

        protected override void OnHeaderColumn( char column, string header )
        {
            base.OnHeaderColumn( column, header );

            if ( header == "CurrentPrice" )
            {
                myCurrentPriceColumn = column.ToString();
            }
            else if ( ShowsCurrentDate( header ) )
            {
                myCurrentPriceColumn = column.ToString();
            }
        }

        protected override void OnHeaderDetected()
        {
            base.OnHeaderDetected();

            if ( myCurrentPriceColumn == null )
            {
                throw new ArgumentException( "Current price column not found" );
            }

            StatusColumn = myCurrentPriceColumn;
        }

        private bool ShowsCurrentDate( string header )
        {
            DateTime headerDate;
            if ( !DateTime.TryParse( header, out headerDate ) )
            {
                return false;
            }

            return headerDate.Date == DateTime.Today;
        }

        /// <summary/>
        public void SetCurrentPrice( double price )
        {
            Worksheet.SetCell( myCurrentPriceColumn + CurrentDataRow, price );
        }
    }
}
