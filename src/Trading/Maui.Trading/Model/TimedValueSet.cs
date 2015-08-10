using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Maui.Trading.Utils;

namespace Maui.Trading.Model
{
    /// <summary>
    /// Readonly set of TimedValues. Must not necessarily be sorted.
    /// For each time there is only one value allowed.
    /// </summary>
    public class TimedValueSet<TTime, TValue> : ITimedValueSet<TTime, TValue>
        where TTime : IComparable<TTime>, IEquatable<TTime>
    {
        private readonly IList<TimedValue<TTime, TValue>> mySet;

        // index based on "TTime" for performance reasons
        private Dictionary<TTime, TimedValue<TTime, TValue>> myTimeIndex;

        /// <summary/>
        public static readonly TimedValueSet<TTime, TValue> Null = new TimedValueSet<TTime, TValue>();

        /// <summary/>
        public TimedValueSet( IEnumerable<TimedValue<TTime, TValue>> set )
        {
            mySet = set.ToNotNull().ToList();

            BuildUpIndex();
        }

        private void BuildUpIndex()
        {
            myTimeIndex = new Dictionary<TTime, TimedValue<TTime, TValue>>();
            foreach ( var item in mySet )
            {
                myTimeIndex.Add( item.Time, item );
            }
        }

        /// <summary/>
        protected TimedValueSet()
            : this( Null )
        {
        }

        /// <summary/>
        public IEnumerator<TimedValue<TTime, TValue>> GetEnumerator()
        {
            return mySet.GetEnumerator();
        }

        /// <summary/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary/>
        public TimedValue<TTime, TValue> TryGet( TTime key )
        {
            TimedValue<TTime, TValue> value = null;
            myTimeIndex.TryGetValue( key, out value );
            return value;
        }

        /// <summary/>
        public TimedValue<TTime, TValue> this[ TTime key ]
        {
            get
            {
                return myTimeIndex[ key ];
            }
        }

        /// <summary/>
        public TimedValue<TTime, TValue> this[ int index ]
        {
            get
            {
                return mySet[ index ];
            }
        }

        /// <summary/>
        public int Count
        {
            get
            {
                return mySet.Count;
            }
        }
    }
}
