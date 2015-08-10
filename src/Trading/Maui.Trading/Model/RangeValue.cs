using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Model
{
    public class RangeValue : IEquatable<RangeValue>
    {
        // value initialized to "min"
        public RangeValue( int min, int max )
            : this( min, max, min )
        {
        }

        public RangeValue( int min, int max, int value )
        {
            Min = min;
            Max = max;
            Value = value;

            ValidateValueInRange();
        }

        private void ValidateValueInRange()
        {
            if ( Includes( Value ) )
            {
                return;
            }

            throw new ArgumentOutOfRangeException( string.Format( "value {0} must be between: [{1};{2}]", Value, Min, Max ) );
        }

        public int Value
        {
            get;
            private set;
        }

        public int Min
        {
            get;
            private set;
        }

        public int Max
        {
            get;
            private set;
        }

        // value relative to min
        public int RelativeValue
        {
            get
            {
                return Value - Min;
            }
        }

        // Multiplies relative value with factor
        // throws if out of range
        public static RangeValue operator *( RangeValue value, double factor )
        {
            int newValue = (int)( value.RelativeValue * factor );
            return new RangeValue( value.Min, value.Max, value.Min + newValue );
        }

        // throws if out of range
        public static RangeValue operator +( RangeValue value, int valueToAdd )
        {
            return new RangeValue( value.Min, value.Max, value.Value + valueToAdd );
        }

        public bool Includes( int value )
        {
            return Min <= value && value <= Max;
        }

        public bool Equals( RangeValue other )
        {
            if ( other == null )
            {
                return false;
            }

            if ( object.ReferenceEquals( this, other ) )
            {
                return true;
            }

            return Min == other.Min && Max == other.Max && Value == other.Value;
        }

        public override bool Equals( object obj )
        {
            return Equals( obj as RangeValue );
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return string.Format( "{0} [{1},{2}]", Value, Min, Max );
        }

        public static bool operator ==( RangeValue lhs, RangeValue rhs )
        {
            if ( object.ReferenceEquals( lhs, rhs ) )
            {
                return true;
            }

            if ( object.ReferenceEquals( lhs, null ) )
            {
                return false;
            }

            return lhs.Equals( rhs );
        }

        public static bool operator !=( RangeValue lhs, RangeValue rhs )
        {
            return !( lhs == rhs );
        }
    }
}
