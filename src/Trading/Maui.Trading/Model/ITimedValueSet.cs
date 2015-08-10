using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Model
{
    public interface ITimedValueSet<TTime, TValue>
        : IRandomAccessSet<TimedValue<TTime, TValue>>
        where TTime : IComparable<TTime>, IEquatable<TTime>
    {
        TimedValue<TTime, TValue> TryGet( TTime time );
        TimedValue<TTime, TValue> this[ TTime time ] { get; }
    }
}
