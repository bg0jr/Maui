using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blade.Collections;
using System.Collections;

namespace Maui.Trading.Model
{
    public class SeriesRange<TTime, TValue> : ITimedValueSeries<TTime, TValue>
        where TTime : IComparable<TTime>, IEquatable<TTime>
    {
        private ITimedValueSeries<TTime, TValue> mySeries;
        private int myFrom;
        private int myTo;

        public SeriesRange( ITimedValueSeries<TTime, TValue> series, ClosedInterval<int> interval )
            : this( series, interval.Min, interval.Max )
        {
        }

        public SeriesRange( ITimedValueSeries<TTime, TValue> series, int from, int to )
        {
            mySeries = series;
            SetRangeIndices( from, to );

            var descriptor = new ObjectDescriptor( "Range",
                ObjectDescriptor.Param( "from", myFrom ), ObjectDescriptor.Param( "to", myTo ) );
            Identifier = mySeries.Identifier.Modify( descriptor );
        }

        private void SetRangeIndices( int from, int to )
        {
            if ( !mySeries.Any() )
            {
                myFrom = -1;
                myTo = -1;
                Count = 0;

                return;
            }

            var range = ClosedInterval.FromOffsetLength( 0, mySeries.Count );
            if ( !range.Includes( from ) )
            {
                throw new ArgumentOutOfRangeException( "from", from + " is out of range" );
            }
            if ( !range.Includes( to ) )
            {
                throw new ArgumentOutOfRangeException( "to", to + " is out of range" );
            }

            myFrom = from;
            myTo = to;

            Count = myTo - myFrom + 1;
        }

        public SeriesIdentifier Identifier
        {
            get;
            private set;
        }

        public IEnumerator<TimedValue<TTime, TValue>> GetEnumerator()
        {
            return Range.GetEnumerator();
        }

        private IEnumerable<TimedValue<TTime, TValue>> Range
        {
            get
            {
                if ( mySeries.Count > 0 )
                {
                    for ( int i = myFrom; i <= myTo; ++i )
                    {
                        yield return mySeries[ i ];
                    }
                }
                else
                {
                    yield break;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get;
            private set;
        }

        public TimedValue<TTime, TValue> this[ int index ]
        {
            get { return mySeries[ myFrom + index ]; }
        }

        public TimedValue<TTime, TValue> this[ TTime time ]
        {
            get { return Range.Single( item => item.Time.Equals( time ) ); }
        }

        public TimedValue<TTime, TValue> TryGet( TTime time )
        {
            return Range.SingleOrDefault( item => item.Time.Equals( time ) );
        }
    }
}
