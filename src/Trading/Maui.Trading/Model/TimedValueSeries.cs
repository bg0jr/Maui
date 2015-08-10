using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Maui.Trading.Utils;
using System.Diagnostics;

namespace Maui.Trading.Model
{
    [DebuggerDisplay( "Id={Identifier.ShortDesignator}, Count={Count}" )]
    public class TimedValueSeries<TTime, TValue>
        : TimedValueSet<TTime, TValue>, ITimedValueSeries<TTime, TValue>
        where TTime : IComparable<TTime>, IEquatable<TTime>
    {
        public static new readonly TimedValueSeries<TTime, TValue> Null = new TimedValueSeries<TTime, TValue>();

        public TimedValueSeries( SeriesIdentifier identifier, IEnumerable<TimedValue<TTime, TValue>> set )
            : this( identifier, set, true )
        {
        }

        public TimedValueSeries( ITimedValueSeries<TTime, TValue> sortedSeries )
            : this( sortedSeries.Identifier, sortedSeries, false )
        {
        }

        protected TimedValueSeries( SeriesIdentifier identifier, IEnumerable<TimedValue<TTime, TValue>> set, bool sortRequired )
            : base( SortOnDemand( set, sortRequired ) )
        {
            if ( identifier == null )
            {
                throw new ArgumentNullException( "identifer" );
            }

            Identifier = identifier;
        }

        private static IEnumerable<TimedValue<TTime, TValue>> SortOnDemand( IEnumerable<TimedValue<TTime, TValue>> set, bool sortRequired )
        {
            if ( set == null )
            {
                return set.Null();
            }

            return sortRequired ? set.OrderBy( item => item.Time ) : set;
        }

        protected TimedValueSeries()
        {
            Identifier = SeriesIdentifier.Null;
        }

        public SeriesIdentifier Identifier
        {
            get;
            private set;
        }
    }
}
