using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Reporting.Charting;

namespace Maui.Trading.Reporting
{
    /// <summary>
    /// Handle for charts.
    /// </summary>
    public class GenericChartSection : AbstractSection, IChartSection
    {
        public GenericChartSection( string name, StockPriceChart chart )
            : base( name )
        {
            if ( chart == null )
            {
                throw new ArgumentNullException( "chart" );
            }

            Chart = chart;
        }

        public StockPriceChart Chart
        {
            get;
            private set;
        }
    }
}
