using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Indicators;
using Maui.Trading.Model;
using Maui.Trading.Reporting;
using Maui.Trading.Binding;
using Maui.Trading.Binding.Decorators;
using Maui.Entities;

namespace Maui.Trading.Modules
{
    public class TradingSystem
    {
        private ChartGenerator myChartGenerator;
        private CombinedIndicator myIndicator;

        public TradingSystem( string name )
        {
            Name = name;

            myIndicator = new CombinedIndicator();
            AdditionalReportGenerators = new List<IReportGenerator>();
            myChartGenerator = new ChartGenerator();
        }

        public string Name
        {
            get;
            private set;
        }

        public IIndicator Indicator
        {
            get
            {
                return myIndicator;
            }
        }

        public void AddIndicator( IIndicator indicator )
        {
            myIndicator.AddIndicator( indicator );
        }

        public void AddIndicator( double weight, IIndicator indicator )
        {
            AddIndicator( new WeightedIndicator( indicator, weight ) );
        }

        public IList<IReportGenerator> AdditionalReportGenerators
        {
            get;
            private set;
        }

        [DataSource]
        private IPriceSeriesDataSource Prices
        {
            get;
            set;
        }

        public SystemResult Evaluate( StockHandle stock, DateTime dateUnderAnalysis )
        {
            var indicatorResult = EvaluateIndicator( stock, dateUnderAnalysis );

            var systemResult = new SystemResult( Name, stock, Prices.ForStock( stock ), indicatorResult );

            var report = CreateReport( dateUnderAnalysis, systemResult );
            report.SystemDetails.Indicator = indicatorResult;

            systemResult.Report = report;

            AddReports( report );

            report.Sections.Add( myChartGenerator.Generate( systemResult ) );

            return systemResult;
        }

        private IndicatorResult EvaluateIndicator( StockHandle stock, DateTime dateUnderAnalysis )
        {
            var context = CreateIndicatorContext( dateUnderAnalysis );
            context.Stock = stock;

            return Indicator.Calculate( context );
        }

        protected virtual BasicSystemReport CreateReport( DateTime dateUnderAnalysis, SystemResult systemResult )
        {
            return new BasicSystemReport( Name, dateUnderAnalysis, systemResult );
        }

        private void AddReports( BasicSystemReport report )
        {
            foreach ( var generator in AdditionalReportGenerators )
            {
                var additionalReport = generator.Generate( report.SystemResult );
                report.AdditionalReports.Add( additionalReport );
            }
        }

        protected virtual IndicatorContext CreateIndicatorContext( DateTime dateUnderAnalysis )
        {
            var context = new IndicatorContext();
            context.DateUnderAnalysis = dateUnderAnalysis;

            return context;
        }

        // TODO: move to report
        private class ChartGenerator
        {
            public GenericChartSection Generate( SystemResult result )
            {
                var chart = new StockPriceChart( result.System, result.Stock, result.Prices );
                chart.Signals = result.Signals;

                var section = new GenericChartSection( "Chart", chart );
                return section;
            }
        }
    }
}
