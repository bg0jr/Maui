using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui;

namespace Maui.Tasks.Dynamics
{
    public static class Maths
    {
        public static bool InRange( this double value, double min, double max )
        {
            return min <= value && value <= max;
        }

        public static double RatePerCent( int years, double startValue, double endValue )
        {
            return FinancialMaths.RatePerCent( years, startValue, endValue );
        }
    }
}
