using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Reporting;
using Maui.Trading.Model;
using Maui.Entities;

namespace Maui.Trading.Evaluation
{
    public class BackTestingStockReport : DetailedReport
    {
        public BackTestingStockReport( TradingResult tradingResult )
            : base( tradingResult.Stock.Isin, "BackTestingDetails", "BackTesting details" )
        {
            Sections.Add( CreateOverviewSection( tradingResult ) );
            Sections.Add( CreateChartSection( tradingResult ) );
            Sections.Add( CreateTableSection( tradingResult ) );
        }

        private AbstractSection CreateOverviewSection( TradingResult tradingResult )
        {
            var section = new KeyValueSection( "Overview" );
            section.Entries[ "Stock" ] = tradingResult.Stock;
            section.Entries[ "Inital cash" ] = tradingResult.InitialCash;
            section.Entries[ "Portfolio value" ] = tradingResult.PortfolioValue;
            section.Entries[ "Gain per anno" ] = tradingResult.GainPerAnno;

            return section;
        }

        private GenericChartSection CreateChartSection( TradingResult tradingResult )
        {
            var chart = new StockPriceChart( "TradingProtocol", tradingResult.Stock, tradingResult.Prices );

            var trades = tradingResult.TradingLog.Orders
                .Select( o => o.Timestamp )
                .ToList();
            var signals = tradingResult.SystemSignals
                .Where( s => trades.Contains( s.Time ) );
            chart.Signals = new SignalSeries( tradingResult.Prices, new SeriesIdentifier( new StockObjectIdentifier( tradingResult.Stock ), new ObjectDescriptor( "Trading" ) ), signals );

            AddIndicators( chart, tradingResult.SystemReport );

            var section = new GenericChartSection( "Chart", chart );
            return section;
        }

        private void AddIndicators( StockPriceChart chart, Report report )
        {
            var visitor = new ChartSectionVisitor();
            var walker = new ReportWalker( visitor );
            walker.Visit( report );

            foreach ( var section in visitor.Sections.OfType<GenericChartSection>() )
            {
                foreach ( var entry in section.Chart.IndicatorPoints )
                {
                    // could be that the chart is already added
                    // sample: indicator 1 = SMA.200, indicator 2 = SMA.10xSMA.200 (double cross over). then SMA.200 would be added two times
                    if ( !chart.IndicatorPoints.ContainsKey( entry.Key ) )
                    {
                        chart.IndicatorPoints.Add( entry.Key, entry.Value );
                    }
                }
            }
        }

        private AbstractSection CreateTableSection( TradingResult tradingResult )
        {
            var header = new TableHeader( "Date", "Action", "Price", "Quantity", "Value" );
            var section = new TableSection( "Orders", header );

            foreach ( var order in tradingResult.TradingLog.Orders )
            {
                var row = section.NewRow( order.Timestamp, order.Type, order.Price, order.Quantity, order.NettoValue );
                section.Rows.Add( row );
            }

            return section;
        }

        private class ResultAdapter : IAnalysisResult
        {
            private TradingResult myResult;

            public ResultAdapter( TradingResult result )
            {
                myResult = result;
            }

            public StockHandle Stock
            {
                get { return myResult.Stock; }
            }

            public Report Report
            {
                get { return myResult.SystemReport; }
            }
        }
    }
}
