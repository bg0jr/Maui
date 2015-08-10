using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Model
{
    public class FundamentalValue : TimedValue<int, double>
    {
        public FundamentalValue()
        {
        }

        public FundamentalValue( int period, double value )
            : base( period, value )
        {
        }
    }
}
