using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;

namespace Maui.Trading.Indicators
{
    public class TimedSignal : TimedValue<DateTime, Signal>
    {
        public TimedSignal()
        {
        }

        public TimedSignal( DateTime time, Signal value )
            : base( time.Date, value )
        {
        }

        public TimedSignal Weight( double factor )
        {
            return new TimedSignal( Time, Value.Weight( factor ) );
        }
    }
}
