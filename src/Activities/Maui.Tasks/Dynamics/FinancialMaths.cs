using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualBasic;

namespace Maui.Tasks.Dynamics
{
    /// <summary>
    /// http://www.digitalproducts.de/Zinseszins.php
    /// </summary>
    public sealed class FinancialMaths
    {
        /// <summary>
        /// Calculates "rate per cent".
        /// </summary>
        /// <param name="years">period of time used for calculation</param>
        /// <param name="startValue">start value (e.g. price)</param>
        /// <param name="endValue">end value (e.g. forecast high price)</param>
        /// <returns>in percent</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1702" )]
        static public double RatePerCent( int years, double startValue, double endValue )
        {
            return (double)Microsoft.VisualBasic.Financial.Rate( years, 0, -startValue, endValue, DueDate.EndOfPeriod, 0.1d ) * 100;
        }

        private FinancialMaths()
        {
        }
    }
}
