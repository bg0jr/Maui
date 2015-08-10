using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Model
{
    /// <summary>
    /// Due to Xml object binding implementation we need a default constructor and 
    /// a mutable object at the moment.
    /// </summary>
    public class TimedValue<TTime, TValue> : IEquatable<TimedValue<TTime, TValue>>
        where TTime : IComparable<TTime>, IEquatable<TTime>
    {
        public TimedValue()
        {
        }

        public TimedValue( TTime time, TValue value )
        {
            Time = time;
            Value = value;
        }

        public TTime Time
        {
            get;
            set;
        }

        public TValue Value
        {
            get;
            set;
        }

        public override bool Equals( object other )
        {
            return Equals( other as TimedValue<TTime, TValue> );
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return string.Format( "{0}: {1}", Time, Value );
        }

        public bool Equals( TimedValue<TTime, TValue> other )
        {
            if ( other == null )
            {
                return false;
            }

            return Time.Equals( other.Time ) && Value.Equals( other.Value );
        }
    }
}
