using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;

namespace Maui.Trading.Indicators
{
    public abstract class AbstractCombinedSignalCreator : ICombinedSignalCreator
    {
        public abstract Signal Create( IEnumerable<Signal> signals );

        // takes meta data from the first series
        public ISignalSeries Create( IEnumerable<ISignalSeries> signals )
        {
            if ( !signals.Any() )
            {
                return SignalSeries.Null;
            }

            var dateSignalsMap = CreateDateSignalsMap( signals );

            int expectedSignalCount = signals.Count();
            InterpolateMissingSignals( dateSignalsMap, expectedSignalCount );

            var combinedSignals = GenerateCombinedSignals( dateSignalsMap );

            var masterInputSeries = signals.FirstOrDefault( s => s.Reference.Identifier.Type.Name == DatumNames.Prices );
            if ( masterInputSeries == null )
            {
                // then lets take the reference with max points
                var maxPoints = signals.Max( s => s.Count);
                masterInputSeries = signals.First( s => s.Count == maxPoints );
                // throw new Exception( "We could not find any signal series which seem to reference raw prices" );
            }
            var identifier = new SeriesIdentifier( masterInputSeries.Identifier.Owner, new ObjectDescriptor( "Combined" ), signals.Select( s => s.Identifier ).ToArray() );

            var series = new SignalSeries( masterInputSeries.Reference, identifier, combinedSignals );

            return series;
        }

        // create a map of all signals per date
        // key: date, value: set of signals for this date
        private static Dictionary<DateTime, IList<Signal>> CreateDateSignalsMap( IEnumerable<IEnumerable<TimedValue<DateTime, Signal>>> signals )
        {
            var dateSignalsMap = new Dictionary<DateTime, IList<Signal>>();
            foreach ( var innerSignals in signals )
            {
                var dates = innerSignals
                    .Select( timedSignal => timedSignal.Time )
                    .Distinct();

                if ( dates.Count() < innerSignals.Count() )
                {
                    throw new ArgumentException( "Multiple signals per date are not allowed in inner signal set" );
                }

                foreach ( var signal in innerSignals )
                {
                    if ( !dateSignalsMap.ContainsKey( signal.Time ) )
                    {
                        dateSignalsMap[ signal.Time ] = new List<Signal>();
                    }

                    dateSignalsMap[ signal.Time ].Add( signal.Value );
                }
            }
            return dateSignalsMap;
        }

        // fill every entry with "none"-signals so that the count of signals
        // matches the count of signal sets passed in so that the CombinedSignal 
        // is computed correctly (regarding signal strength)
        private static void InterpolateMissingSignals( Dictionary<DateTime, IList<Signal>> dateSignalsMap, int expectedSignalCount )
        {
            foreach ( var entry in dateSignalsMap )
            {
                while ( entry.Value.Count < expectedSignalCount )
                {
                    entry.Value.Add( Signal.None );
                }
            }
        }

        // generate combined signals per date
        private IEnumerable<TimedSignal> GenerateCombinedSignals( Dictionary<DateTime, IList<Signal>> dateSignalsMap )
        {
            foreach ( var entry in dateSignalsMap )
            {
                var combinedSignal = new TimedSignal( entry.Key, Create( entry.Value ) );
                yield return combinedSignal;
            }
        }
    }
}
