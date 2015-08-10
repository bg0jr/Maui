using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Model
{
    public static class NullObject
    {
        public static IEnumerable<TimedValue<TTime, TValue>> Null<TTime, TValue>( this IEnumerable<TimedValue<TTime, TValue>> self )
            where TTime : IComparable<TTime>, IEquatable<TTime>
        {
            return new List<TimedValue<TTime, TValue>>();
        }

        public static IEnumerable<TimedValue<TTime, TValue>> ToNotNull<TTime, TValue>( this IEnumerable<TimedValue<TTime, TValue>> self )
            where TTime : IComparable<TTime>, IEquatable<TTime>
        {
            return self == null ? self.Null() : self;
        }
    }
}
