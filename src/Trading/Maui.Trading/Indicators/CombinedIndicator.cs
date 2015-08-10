using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Reporting;
using Maui.Trading.Model;
using Blade.Collections;
using Maui.Entities;

namespace Maui.Trading.Indicators
{
    public class CombinedIndicator : AbstractIndicator
    {
        private IList<IIndicator> myIndicators;

        // with DefensiveCombinedSignalCreator
        public CombinedIndicator()
            : this( new List<IIndicator>() )
        {
        }

        // with DefensiveCombinedSignalCreator
        public CombinedIndicator( IEnumerable<IIndicator> indicators )
            : this( new DefensiveCombinedSignalCreator(), indicators )
        {
        }

        public CombinedIndicator( ICombinedSignalCreator combinedSignalCreator )
            : this( combinedSignalCreator, new List<IIndicator>() )
        {
        }

        public CombinedIndicator( ICombinedSignalCreator combinedSignalCreator, IEnumerable<IIndicator> indicators )
        {
            CombinedSignalCreator = combinedSignalCreator;
            myIndicators = indicators.ToList();
        }

        public ICombinedSignalCreator CombinedSignalCreator
        {
            get;
            private set;
        }

        public IEnumerable<IIndicator> Indicators
        {
            get
            {
                return myIndicators;
            }
        }

        public void AddIndicator( IIndicator indicator )
        {
            myIndicators.Add( indicator );
        }

        public override IndicatorResult Calculate( IIndicatorContext context )
        {
            var result = new CombinedIndicatorResult( this, context.Stock );

            foreach ( var indicator in myIndicators )
            {
                var r = indicator.Calculate( context );
                result.AddResult( r );
            }

            result.Close();

            return result;
        }

        private class CombinedIndicatorResult : IndicatorResult
        {
            private IList<IndicatorResult> myResults;
            private CombinedIndicator myIndicator;

            public CombinedIndicatorResult( CombinedIndicator indicator, StockHandle stock )
                : base( indicator.Name, stock )
            {
                myIndicator = indicator;
                myResults = new List<IndicatorResult>();
            }

            public IEnumerable<IndicatorResult> DetailedResults
            {
                get
                {
                    return myResults;
                }
            }

            public void AddResult( IndicatorResult result )
            {
                myResults.Add( result );
            }

            public void Close()
            {
                // expected gain/risk: AVG (only calculated between the indicators which
                // return an expected gain)

                ExpectedGain = myResults.Where( r => r.ExpectedGain.HasValue ).Average( r => r.ExpectedGain );
                GainRiskRatio = myResults.Where( r => r.GainRiskRatio.HasValue ).Average( r => r.GainRiskRatio );
                Signal = myIndicator.CombinedSignalCreator.Create( myResults.Select( r => r.Signal ) );
                Report = new CombinedIndicatorReport( this );

                var allHistoricalSignals = DetailedResults.Select( r => r.Signals );
                Signals = myIndicator.CombinedSignalCreator.Create( allHistoricalSignals );
            }
        }

        private class CombinedIndicatorReport : Report
        {
            private CombinedIndicatorResult myResult;

            public CombinedIndicatorReport( CombinedIndicatorResult indicatorResult )
                : base( "IndicatorsSummary", "Indicator details: " + indicatorResult.Stock.Name )
            {
                myResult = indicatorResult;

                Sections.Add( CreateSummarySection() );
                Sections.Add( CreateDetailsSection() );
            }

            private AbstractSection CreateSummarySection()
            {
                var section = new KeyValueSection( "Summary" );

                section.Entries[ "Signal" ] = myResult.Signal;
                section.Entries[ "ExpectedGain" ] = myResult.ExpectedGain;
                section.Entries[ "GainRiskRatio" ] = myResult.GainRiskRatio;

                return section;
            }

            private AbstractSection CreateDetailsSection()
            {
                var section = new IndicatorCollectionSection();

                foreach ( var result in myResult.DetailedResults )
                {
                    var combinedResult = result as CombinedIndicatorResult;
                    if ( combinedResult != null )
                    {
                        section.IndicatorResults.AddRange( combinedResult.DetailedResults );
                    }
                    else
                    {
                        section.IndicatorResults.Add( result );
                    }
                }

                return section;
            }
        }
    }
}
