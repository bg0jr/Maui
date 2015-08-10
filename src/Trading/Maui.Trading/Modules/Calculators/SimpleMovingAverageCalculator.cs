using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Indicators;
using Maui.Trading.Model;

namespace Maui.Trading.Modules.Calculators
{
    public class SimpleMovingAverageCalculator : ISeriesCalculator
    {
        public SimpleMovingAverageCalculator( int numDays )
        {
            NumDays = numDays;

            Name = "SMA." + NumDays;
        }

        public string Name
        {
            get;
            private set;
        }

        public int NumDays
        {
            get;
            private set;
        }

        public bool ContainsEnoughData( IPriceSeries prices )
        {
            return prices.Count() >= NumDays;
        }
        
        public IPriceSeries Calculate( IPriceSeries prices )
        {
            var points = new List<SimplePrice>();

            for ( int i = NumDays; i < prices.Count; ++i )
            {
                var pricesRange = new PriceSeriesRange( prices, ClosedInterval.FromOffsetLength( i - NumDays, NumDays ) );

                double value = pricesRange.Average( p => p.Value );

                var point = new SimplePrice( prices[ i ].Time, value );
                points.Add( point );
            }

            var descriptor = new ObjectDescriptor( "SMA", ObjectDescriptor.Param( "NumDays", NumDays ) );
            var seriesId = prices.Identifier.Derive( descriptor );
            return new PriceSeries( seriesId, points );
        }
    }
}
