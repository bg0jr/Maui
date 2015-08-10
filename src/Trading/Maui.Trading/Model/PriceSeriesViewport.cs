using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Model
{
    public class PriceSeriesViewport : SeriesViewport<DateTime, double, IPriceSeries>
    {
        public static readonly PriceSeriesViewport Null = new PriceSeriesViewport();

        public PriceSeriesViewport( IPriceSeries series )
            : this( series, TimeRange.All )
        {
        }

        public PriceSeriesViewport( IPriceSeries series, ClosedInterval<DateTime> range )
            : base( series, range )
        {
        }

        public PriceSeriesViewport()
            : this( PriceSeries.Null )
        {
        }

        protected override IPriceSeries CreateSeriesRange( IPriceSeries series, ClosedInterval<int> interval )
        {
            if ( interval.IsEmpty )
            {
                return PriceSeries.Null;
            }

            return new PriceSeriesRange( series, interval );
        }
    }
}
