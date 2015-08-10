using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Indicators;
using Maui.Trading.Reporting;
using Maui.Trading.Modules;
using Maui.Trading.Model;

namespace Maui.Trading.Evaluation
{
    public class StockRankingReport : Report
    {
        private const int MaxHistoricalSignals = 3;

        private DateTime myDateUnderAnalysis;
        private TableSection myRankingSection;

        public StockRankingReport( string systemName, DateTime dateUnderUnderAnalysis )
            : base( "StockRanking", "Stock ranking evaluation" )
        {
            myDateUnderAnalysis = dateUnderUnderAnalysis;

            AddOverviewSection( systemName );

            myRankingSection = new TableSection( "Ranking", new TableHeader( "Stock", "Signal", "Gain/Risk ratio", "Expected gain", "Historical signals" ) );
            myRankingSection.View = new OrderBySystemResultView( myRankingSection );
            Sections.Add( myRankingSection );
        }

        private void AddOverviewSection( string systemName )
        {
            var section = new KeyValueSection( "Overview" );
            section.Entries[ "Trading system" ] = systemName;
            section.Entries[ "Date under analysis" ] = myDateUnderAnalysis.ToShortDateString();

            Sections.Add( section );
        }

        public void AddResult( SystemResult result )
        {
            var row = new ResultRow( myRankingSection, result );
            row[ "Stock" ] = result.Stock;
            row[ "Signal" ] = new ValueWithDetails( result.Signal, new DetailedReportAdapter( result.Stock.Isin, result.Report ) );
            row[ "Gain/Risk ratio" ] = result.GainRiskRatio;
            row[ "Expected gain" ] = result.ExpectedGain;
            row[ "Historical signals" ] = GetHistoricalSignals( result );

            myRankingSection.Rows.Add( row );
        }

        private IEnumerable<TimedValue<DateTime, Signal>> GetHistoricalSignals( SystemResult result )
        {
            if ( !result.Signals.Any() )
            {
                return null;
            }

            var historicalSignals = result.Signals;
            if ( historicalSignals.Last().Time == myDateUnderAnalysis )
            {
                historicalSignals = new SignalSeriesRange( historicalSignals, ClosedInterval.FromOffsetLength( 0, historicalSignals.Count - 1 ) );
            }

            return historicalSignals
                .Reverse()
                .Take( MaxHistoricalSignals )
                .ToList();
        }

        private class ResultRow : TableRow
        {
            public ResultRow( TableSection table, SystemResult result )
                : base( table )
            {
                Result = result;
            }

            public SystemResult Result
            {
                get;
                private set;
            }
        }

        private class OrderBySystemResultView : AbstractTableView
        {
            public OrderBySystemResultView( TableSection table )
                : base( table )
            {
            }

            public override IEnumerable<TableRow> Rows
            {
                get
                {
                    return Table.Rows
                        .Cast<ResultRow>()
                        .OrderByDescending( row => row.Result, new SystemResultComparer() )
                        .ToList();
                }
            }
        }

    }
}
