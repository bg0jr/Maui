using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Maui.Trading.Utils;
using Maui.Trading.Binding;
using Maui.Trading.Indicators;

namespace Maui.Trading.Model
{
    public class SignalSeries : TimedValueSeries<DateTime, Signal>, ISignalSeries
    {
        public static new readonly ISignalSeries Null = new SignalSeries();

        public SignalSeries( IPriceSeries prices, SeriesIdentifier identifier, IEnumerable<TimedValue<DateTime, Signal>> set )
            : this( prices, identifier, set, true )
        {
        }

        public SignalSeries( ISignalSeries series )
            : base( series )
        {
            Reference = series.Reference;
        }

        protected SignalSeries( IPriceSeries prices, SeriesIdentifier identifier, IEnumerable<TimedValue<DateTime, Signal>> set, bool sortRequired )
            : base( identifier, set, sortRequired )
        {
            Reference = prices;
        }

        protected SignalSeries()
        {
            Reference = PriceSeries.Null;
        }

        public IPriceSeries Reference
        {
            get;
            private set;
        }

        public static ISignalSeries FromSortedSet( IPriceSeries prices, SeriesIdentifier identifier, IEnumerable<TimedValue<DateTime, Signal>> series )
        {
            return new SignalSeries( prices, identifier, series, false );
        }
    }
}
