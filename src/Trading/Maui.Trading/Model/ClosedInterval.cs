using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Model
{
    public class ClosedInterval<T> : IEquatable<ClosedInterval<T>>
        where T : IComparable<T>
    {
        /// <summary>
        /// Creates an empty interval.
        /// </summary>
        public ClosedInterval()
        {
            Min = default( T );
            Max = default( T );
            IsEmpty = true;
        }

        public ClosedInterval( T min, T max )
        {
            Min = min;
            Max = max;
            IsEmpty = false;
        }

        public T Min
        {
            get;
            private set;
        }

        public T Max
        {
            get;
            private set;
        }

        public bool IsEmpty
        {
            get;
            private set;
        }

        public override bool Equals( object other )
        {
            return Equals( other as ClosedInterval<T> );
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return string.Format( "[{0},{1}]", Min, Max );
        }

        public bool Equals( ClosedInterval<T> other )
        {
            if ( other == null )
            {
                return false;
            }

            return Min.Equals( other.Min ) && Max.Equals( other.Max );
        }

        public bool Includes( T value )
        {
            if ( Min.CompareTo( value ) > 0 )
            {
                return false;
            }
            if ( Max.CompareTo( value ) < 0 )
            {
                return false;
            }
            return true;
        }
    }
}
