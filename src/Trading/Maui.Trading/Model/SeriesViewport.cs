using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Model
{
    public abstract class SeriesViewport<TTime, TValue, TSeries> : ISeriesViewPort<TTime, TValue, TSeries>
        where TTime : IComparable<TTime>, IEquatable<TTime>
        where TSeries : ITimedValueSeries<TTime, TValue>
    {
        private ClosedInterval<TTime> myViewport;
        private TSeries myOriginalSeries;
        private TSeries myRange;

        protected SeriesViewport( TSeries series, ClosedInterval<TTime> range )
        {
            myOriginalSeries = series;
            Series = myOriginalSeries;

            ViewPort = range;
        }

        public TSeries Series
        {
            get
            {
                return myRange;
            }
            private set
            {
                myRange = value;
                CalculateMinMaxValue();
            }
        }

        // optimized for performance
        private void CalculateMinMaxValue()
        {
            if ( Series.Count == 0 )
            {
                return;
            }

            MinValue = Series[ 0 ];
            MaxValue = Series[ 0 ];

            for ( int i = 1; i < Series.Count; ++i )
            {
                var item = Series[ i ];

                if ( CompareValue( MinValue, item ) > 0 )
                {
                    MinValue = item;
                }
                else if ( CompareValue( MaxValue, item ) < 0 )
                {
                    MaxValue = item;
                }
            }
        }

        protected virtual int CompareValue( TimedValue<TTime, TValue> lhs, TimedValue<TTime, TValue> rhs )
        {
            var comparable = lhs.Value as IComparable<TValue>;
            if ( comparable == null )
            {
                throw new NotSupportedException( "TValue is not comparable. Override CompareValue() in derived class" );
            }

            return comparable.CompareTo( rhs.Value );
        }

        public TimedValue<TTime, TValue> MinValue
        {
            get;
            private set;
        }

        public TimedValue<TTime, TValue> MaxValue
        {
            get;
            private set;
        }

        public TTime MinTime
        {
            get
            {
                return Series.Any() ? Series.First().Time : default( TTime ); ;
            }
        }

        public TTime MaxTime
        {
            get
            {
                return Series.Any() ? Series.Last().Time : default( TTime ); ;
            }
        }

        public ClosedInterval<TTime> ViewPort
        {
            get
            {
                return myViewport;
            }
            set
            {
                if ( myViewport != null && myViewport.Equals( value ) )
                {
                    return;
                }

                myViewport = value;

                Series = CreateSeriesRange();
            }
        }

        // optimized for performance
        private TSeries CreateSeriesRange()
        {
            if ( myOriginalSeries.Count == 0 )
            {
                return Series;
            }

            int min = -1;
            int max = -1;

            // search from the end to begin because most of the viewport ranges start from the end
            for ( int i = myOriginalSeries.Count - 1; i >= 0; --i )
            {
                var item = myOriginalSeries[ i ];

                if ( item.Time.CompareTo( ViewPort.Max ) > 0 )
                {
                    // still outside range
                    continue;
                }

                if ( item.Time.CompareTo( ViewPort.Min ) < 0 )
                {
                    // again outside range
                    break;
                }

                // overwrite till we are outside range again
                min = i;

                if ( max == -1 && item.Time.CompareTo( ViewPort.Max ) <= 0 )
                {
                    // set to index of first time inside range
                    max = i;
                }
            }

            if ( min < 0 || max < 0 )
            {
                return CreateSeriesRange( myOriginalSeries, new ClosedInterval<int>() );
            }
            else
            {
                return CreateSeriesRange( myOriginalSeries, ClosedInterval.FromMinMax( min, max ) );
            }
        }

        protected abstract TSeries CreateSeriesRange( TSeries series, ClosedInterval<int> interval );
    }
}
