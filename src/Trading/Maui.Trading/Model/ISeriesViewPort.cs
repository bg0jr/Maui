using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Model
{
    public interface ISeriesViewPort<TTime, TValue, TSeries>
        where TTime : IComparable<TTime>, IEquatable<TTime>
        where TSeries : ITimedValueSeries<TTime, TValue>
    {
        TTime MinTime { get; }
        TTime MaxTime { get; }

        TimedValue<TTime, TValue> MinValue { get; }
        TimedValue<TTime, TValue> MaxValue { get; }

        TSeries Series { get; }
    }

}
