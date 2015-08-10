using System;
using System.Collections.Generic;
using System.Linq;
using Blade.Collections;
using Maui.Entities;

namespace Maui.Dynamics.Data
{
    /// <summary>
    /// Abstract series of elements which also have a date.
    /// <remarks>
    /// The series is always ordered by date.
    /// </remarks>
    /// </summary>
    public abstract class AbstractTimeSeries<T, TElement> : IArray<T> where T : ITimeframedValue<TElement>
    {
        /// <summary/>
        protected class LazyIndex
        {
            /// <summary/>
            public LazyIndex( DateTime date )
            {
                Date = date;
                StrictIndex = -1;
                Successor = -1;
                Predecessor = -1;
            }

            /// <summary/>
            public DateTime Date { get; private set; }

            /// <summary/>
            public int StrictIndex { get; set; }
            /// <summary/>
            public int Successor { get; set; }
            /// <summary/>
            public int Predecessor { get; set; }

            /// <summary/>
            public int StrictOrPreceding
            {
                get
                {
                    return StrictIndex != -1 ? StrictIndex : Predecessor;
                }
            }

            /// <summary/>
            public int StrictOrFollowing
            {
                get
                {
                    return StrictIndex != -1 ? StrictIndex : Successor;
                }
            }
        }

        /// <summary/>
        public AbstractTimeSeries( IEnumerable<T> values )
        {
            var preparedValues = values
                .Distinct( new TimeframedValueEqualityComparer<T, TElement>() )
                .OrderBy( v => v.Date );

            Items = new ActiveList<T>( preparedValues );
        }

        private IArray<T> Items { get; set; }

        /// <summary/>
        public IEnumerable<TElement> GetValues( DateTime from, DateTime to )
        {
            return Items
                .Where( item => from <= item.Date && item.Date <= to )
                .Select( item => item.Value );
        }

        /// <summary/>
        public T First
        {
            get
            {
                return Items[ 0 ];
            }
        }

        /// <summary/>
        public T Last
        {
            get
            {
                return Items[ Items.Count - 1 ];
            }
        }

        /// <summary/>
        public bool Contains( T item )
        {
            return Contains( item.Date );
        }

        /// <summary/>
        public virtual bool Contains( DateTime date )
        {
            return IndexOf( date ) != -1;
        }

        /// <summary/>
        public int Count
        {
            get { return Items.Count; }
        }

        /// <summary/>
        public int IndexOf( T item )
        {
            return IndexOf( item.Date );
        }

        /// <summary/>
        public virtual int IndexOf( DateTime date )
        {
            return BinarySearch( this, date ).StrictIndex;
        }

        /// <summary/>
        public T this[ int index ]
        {
            get { return Items[ index ]; }
        }

        /// <summary/>
        public T this[ DateTime date ]
        {
            get
            {
                int idx = IndexOf( date );
                return idx == -1 ? default( T ) : this[ idx ];
            }
        }

        /// <summary/>
        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <summary/>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <summary>
        /// Returns the index of a date greater or equal to the given one.
        /// If there is no such date, -1 is returned.
        /// </summary>
        public int IndexOfNearestFollowing( DateTime date )
        {
            return BinarySearch( this, date ).StrictOrFollowing;
        }

        /// <summary>
        /// Returns the index of a date less or equal to the given one.
        /// If there is no such date, -1 is returned.
        /// </summary>
        public int IndexOfNearestPreceding( DateTime date )
        {
            return BinarySearch( this, date ).StrictOrPreceding;
        }

        /// <summary>
        /// Returns the index of a date with the smallest diff to the given one.
        /// If there is no such date, -1 is returned.
        /// <remarks>
        /// If preceding and following have same distance and no exact match has
        /// been found then the folloing index is returned.
        /// </remarks>
        /// </summary>
        public int IndexOfNearest( DateTime date )
        {
            var result = BinarySearch( this, date );
            if ( result.StrictIndex != -1 )
            {
                return result.StrictIndex;
            }
            if ( result.Successor == -1 )
            {
                return result.Predecessor;
            }
            if ( result.Predecessor == -1 )
            {
                return result.Successor;
            }

            // no exact date but both preceding and following are set
            var diffToPreceding = ( date - this[ result.Predecessor ].Date ).Duration();
            var diffToFollowing = ( date - this[ result.Successor ].Date ).Duration();

            if ( diffToPreceding < diffToFollowing )
            {
                return result.Predecessor;
            }
            else
            {
                return result.Successor;
            }
        }

        /// <summary/>
        public T FindByNearestFollowingDate( DateTime date )
        {
            return GetOrDefault( IndexOfNearestFollowing( date ) );
        }

        /// <summary/>
        public T FindByNearestPrecedingDate( DateTime date )
        {
            return GetOrDefault( IndexOfNearestPreceding( date ) );
        }

        /// <summary/>
        public T FindByNearestDate( DateTime date )
        {
            return GetOrDefault( IndexOfNearest( date ) );
        }

        /// <summary/>
        protected T GetOrDefault( int idx )
        {
            if ( idx == -1 )
            {
                return default( T );
            }

            return this[ idx ];
        }

        /// <summary/>
        protected virtual LazyIndex BinarySearch( IArray<T> array, DateTime date )
        {
            var result = new LazyIndex( date );

            int first = 0;
            int last = array.Count;

            while ( first <= last )
            {
                var middle = (int)( ( first + last ) / 2 );
                if ( middle == array.Count )
                {
                    // we reached the upper border
                    result.Predecessor = array.Count - 1;
                    result.StrictIndex = -1;
                    return result;
                }

                var rc = array[ middle ].Date.CompareTo( date );
                if ( rc == 0 )
                {
                    // we exactly found the date
                    result.StrictIndex = middle;
                    result.Predecessor = middle - 1;
                    result.Successor = ( middle + 1 == array.Count ? -1 : middle + 1 );
                    return result;
                }
                else if ( rc < 0 )
                {
                    first = middle + 1;
                }
                else
                {
                    last = middle - 1;
                }
            }

            if ( first == 0 )
            {
                // we reached the lower border
                result.Successor = 0;
                result.StrictIndex = -1;
            }
            else
            {
                // we stuck somewhere but nothing found
                result.Predecessor = first < last ? first : last;
                result.Successor = first > last ? first : last;
            }

            return result;
        }
    }

}
