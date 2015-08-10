using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Model
{
    public class SimplePrice : TimedValue<DateTime, double>
    {
        public SimplePrice()
        {
        }

        public SimplePrice( DateTime period, double value )
            : base( period, value )
        {
        }
    }
}
