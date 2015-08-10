using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;

namespace Maui.Trading.Data
{
    /// <summary>
    /// Reduces the number of points to the given limit using average function.
    /// </summary>
    public class ReducePointsOperator : IPriceSeriesOperator
    {
        private int myMaxPoints;

        public ReducePointsOperator( int maxPoints )
        {
            myMaxPoints = maxPoints;
        }

        public IPriceSeries Apply( IPriceSeries series )
        {
            double groupInterval = (double)series.Count / (double)myMaxPoints;
            if ( groupInterval <= 1 )
            {
                return series;
            }

            var descriptor = new ObjectDescriptor( "ThinOut", ObjectDescriptor.Param( "MaxCount", myMaxPoints ) );
            var seriesId = series.Identifier.Modify( descriptor );
            return PriceSeries.FromSortedSet( seriesId, GroupPointsByAverage( series, (int)Math.Ceiling( groupInterval ) ) );
        }

        private static IEnumerable<TimedValue<DateTime, double>> GroupPointsByAverage( IPriceSeries series, int averageInterval )
        {
            var pointGroup = new List<TimedValue<DateTime, double>>();
            foreach ( var point in series )
            {
                pointGroup.Add( point );

                if ( pointGroup.Count == averageInterval )
                {
                    yield return Average( pointGroup );
                    pointGroup.Clear();
                }
            }

            if ( pointGroup.Any() )
            {
                yield return Average( pointGroup );
            }
        }

        private static TimedValue<DateTime, double> Average( IEnumerable<TimedValue<DateTime, double>> series )
        {
            var minTime = series.First().Time;
            var maxTime = series.Last().Time;
            double numDays = ( maxTime - minTime ).TotalDays;

            var date = minTime.AddDays( numDays / 2 );
            var price = series.Average( p => p.Value );

            return new SimplePrice( date, price );
        }
    }
}
