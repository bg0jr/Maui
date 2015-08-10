using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Indicators;

namespace Maui.Trading.Model
{
    public class SignalSeriesRange : SeriesRange<DateTime, Signal>, ISignalSeries
    {
        private ISignalSeries mySeries;

        public SignalSeriesRange( ISignalSeries series, ClosedInterval<int> interval )
            : this( series, interval.Min, interval.Max )
        {
        }

        public SignalSeriesRange( ISignalSeries series, int from, int to )
            : base( series, from, to )
        {
            mySeries = series;
        }

        public IPriceSeries Reference
        {
            get { return mySeries.Reference; }
        }
    }
}
