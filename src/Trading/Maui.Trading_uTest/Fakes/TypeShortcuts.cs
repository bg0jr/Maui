using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;

namespace Maui.Trading.UnitTests.Fakes
{
    public class TV : TimedValue<int, int>
    {
        public TV( int time, int value )
            : base( time, value )
        {
        }
    }
}
