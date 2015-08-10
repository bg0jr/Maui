using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Indicators;
using Maui.Trading.Binding;
using Maui.Trading.Model;
using Blade.Collections;
using Maui.Trading.Reporting.Charting;

namespace Maui.Trading.Reporting
{
    public class CombinedIndicatorChartGenerator : IReportGenerator
    {
        public CombinedIndicatorChartGenerator()
        {
            ChartMergeOperators = new List<IChartMergeOperator>();
        }

        public IList<IChartMergeOperator> ChartMergeOperators
        {
            get;
            private set;
        }

        public Report Generate( IAnalysisResult result )
        {
            var report = new Report( "CombinedCharts", "Combined charts: " + result.Stock.Name );

            ApplyChartMergeOperators( result, report );
            AddStandaloneCharts( result, report );

            return report;
        }

        private void ApplyChartMergeOperators( IAnalysisResult result, Report report )
        {
            var mergedSections = ChartMergeOperators
                .Select( op => op.Merge( result ) )
                .Where( section => section != null )
                .ToList();

            report.Sections.AddRange( mergedSections );
        }

        private void AddStandaloneCharts( IAnalysisResult result, Report report )
        {
            var visitor = new ChartSectionVisitor();
            var walker = new ReportWalker( visitor );
            walker.Visit( result.Report );

            var standaloneCharts = visitor.Sections
                .Where( section => !HandledByChartMergeOperators( section ) )
                .ToList();

            report.Sections.AddRange( standaloneCharts );
        }

        private bool HandledByChartMergeOperators( AbstractSection section )
        {
            var chartSection = (IChartSection)section;
            return ChartMergeOperators.Any( op => op.HandledByMergeOperator( chartSection ) );
        }
    }
}
