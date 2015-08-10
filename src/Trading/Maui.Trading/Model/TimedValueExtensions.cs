using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Indicators;

namespace Maui.Trading.Model
{
    public static class TimedValueExtensions
    {
        public static TimedValue<DateTime, Signal> Weight( this TimedValue<DateTime, Signal> self, double factor )
        {
            return new TimedSignal( self.Time, self.Value.Weight( factor ) );
        }
    }
}
