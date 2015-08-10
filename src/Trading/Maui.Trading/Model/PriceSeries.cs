using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Maui.Trading.Utils;
using Maui.Trading.Binding;

namespace Maui.Trading.Model
{
    public class PriceSeries : TimedValueSeries<DateTime, double>, IPriceSeries
    {
        public static new readonly IPriceSeries Null = new PriceSeries();

        public PriceSeries( SeriesIdentifier identifier, IEnumerable<TimedValue<DateTime, double>> set )
            : this( identifier, set, true )
        {
        }

        public PriceSeries( IPriceSeries series )
            : base( series )
        {
        }

        protected PriceSeries( SeriesIdentifier identifier, IEnumerable<TimedValue<DateTime, double>> set, bool sortRequired )
            : base( identifier, set, sortRequired )
        {
        }

        protected PriceSeries()
        {
        }

        public static IPriceSeries FromSortedSet( SeriesIdentifier identifier, IEnumerable<TimedValue<DateTime, double>> series )
        {
            return new PriceSeries( identifier, series, false );
        }
    }
}
