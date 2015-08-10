using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Model
{
    public interface IPriceSeries : ITimedValueSeries<DateTime, double>
    {
    }
}
