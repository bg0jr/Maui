using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blade.Collections;

namespace Maui.Trading.Model
{
    public class PriceSeriesRange : SeriesRange<DateTime, double>, IPriceSeries
    {
        public PriceSeriesRange( IPriceSeries series, ClosedInterval<int> interval )
            : this( series, interval.Min, interval.Max )
        {
        }

        public PriceSeriesRange( IPriceSeries series, int from, int to )
            :base(series,from,to)
        {
        }
    }
}
