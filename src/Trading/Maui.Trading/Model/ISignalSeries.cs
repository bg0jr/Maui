using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Indicators;

namespace Maui.Trading.Model
{
    public interface ISignalSeries : ITimedValueSeries<DateTime, Signal>
    {
        // the price series the signals are based on - might also be indicator points
        IPriceSeries Reference { get; }
    }
}
