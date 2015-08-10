using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Indicators;

namespace Maui.Trading.Model
{
    /// <summary>
    /// MinValue, MaxValue return the signals with the Min/Max price.
    /// </summary>
    public class SignalSeriesViewport : SeriesViewport<DateTime, Signal, ISignalSeries>
    {
        public static readonly SignalSeriesViewport Null = new SignalSeriesViewport();

        public SignalSeriesViewport( ISignalSeries series )
            : this( series, TimeRange.All )
        {
        }

        public SignalSeriesViewport( ISignalSeries series, ClosedInterval<DateTime> range )
            : base( series, range )
        {
        }

        public SignalSeriesViewport()
            : this( SignalSeries.Null )
        {
        }

        protected override int CompareValue( TimedValue<DateTime, Signal> lhs, TimedValue<DateTime, Signal> rhs )
        {
            return Series.Reference[ lhs.Time ].Value.CompareTo( Series.Reference[ rhs.Time ].Value );
        }

        protected override ISignalSeries CreateSeriesRange( ISignalSeries series, ClosedInterval<int> interval )
        {
            if ( interval.IsEmpty )
            {
                return SignalSeries.Null;
            }

            return new SignalSeriesRange( series, interval );
        }
    }
}
