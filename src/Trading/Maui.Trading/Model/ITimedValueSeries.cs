using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Model
{
    public interface ITimedValueSeries<TTime, TValue>
        : ITimedValueSet<TTime, TValue>, IIdentifiableSet<SeriesIdentifier, TimedValue<TTime, TValue>>, ISortedSet<TimedValue<TTime, TValue>>
        where TTime : IComparable<TTime>, IEquatable<TTime>
    {
    }
}
