using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;

namespace Maui.Trading.Data
{
    public class InterpolateMissingDatesOperator : IPriceSeriesOperator
    {
        public IPriceSeries Apply( IPriceSeries series )
        {
            if ( !series.Any() )
            {
                return series;
            }

            var descriptor = new ObjectDescriptor( "InterpolatedMissingDates" );
            var seriesId = series.Identifier.Modify( descriptor );
            return PriceSeries.FromSortedSet( seriesId, FillMissingDates( series ) );
        }

        private IEnumerable<TimedValue<DateTime, double>> FillMissingDates( IPriceSeries series )
        {
            yield return series.First();

            var expectedPoint = series.First();
            foreach ( var point in series.Skip( 1 ) )
            {
                while ( true )
                {
                    expectedPoint = new TimedValue<DateTime, double>( expectedPoint.Time.AddDays( 1 ), expectedPoint.Value );
                    if ( expectedPoint.Time == point.Time )
                    {
                        break;
                    }

                    if ( expectedPoint.Time.DayOfWeek == DayOfWeek.Saturday ||
                          expectedPoint.Time.DayOfWeek == DayOfWeek.Sunday )
                    {
                        // no trading at weekends usually
                        continue;
                    }

                    // missing price at this date - take over the last one we have
                    yield return expectedPoint;
                }

                expectedPoint = point;
                yield return point;
            }
        }
    }
}
