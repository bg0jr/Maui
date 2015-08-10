using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Model
{
    public class TimeRange : ClosedInterval<DateTime>
    {
        public static readonly TimeRange All = new TimeRange( DateTime.MinValue, DateTime.MaxValue );

        public TimeRange( DateTime min, DateTime max )
            : base( min, max )
        {
        }
    }
}
