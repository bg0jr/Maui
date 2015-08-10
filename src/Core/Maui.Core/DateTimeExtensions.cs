using System;
using Blade;
using System.Globalization;

namespace Maui
{
    /// <summary/>
    public static class DateTimeExt
    {
        /// <summary>
        /// Returns the most recent workday. If it is to early for trading
        /// this day. The workday before is returned.
        /// Trading time is: 9-17 
        /// </summary>
        /// <returns>always returns a date without time</returns>
        public static DateTime GetMostRecentTradingDay( this DateTime date )
        {
            var d = date.GetMostRecentWorkDay();
            if ( d.Hour < 9 || d.Hour > 17 )
            {
                d = d.AddDays( -1 ).GetMostRecentWorkDay();
            }

            return d;
        }

        /// <summary/>
        public static string ToIsoDateString( this DateTime date )
        {
            return string.Format( "{0:0000}-{1:00}-{2:00}", date.Year, date.Month, date.Day );
        }
    }
}
