using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Indicators;
using Maui.Trading.Binding;
using Maui.Trading.Model;
using Maui.Trading.Binding.Decorators;

namespace Maui.Trading.Reporting
{
    public class StandalonePriceChartGenerator : IReportGenerator
    {
        [DataSource]
        public IPriceSeriesDataSource Prices
        {
            get;
            set;
        }

        public Report Generate( IAnalysisResult result )
        {
            var stock = result.Stock;
            var report = new Report( "ClosedPricesChart", "Closed price chart: " + stock.Name );

            var chart = new StockPriceChart( stock, Prices.ForStock( stock ) );
            var chartSection = new GenericChartSection( "Closed prices", chart );

            report.Sections.Add( chartSection );

            return report;
        }
    }
}
