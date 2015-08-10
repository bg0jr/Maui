using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Utils
{
    public static class Maths
    {
        public static void ValidateValueAgainstRange( double value, double min, double max )
        {
            if ( min <= value && value <= max )
            {
                return;
            }

            throw new ArgumentOutOfRangeException( string.Format( "value {0} must be between: [{1};{2}]", value, min, max ) );
        }

        public static int Compare( double? x, double? y )
        {
            if ( !x.HasValue && !y.HasValue )
            {
                return 0;
            }
            else if ( !x.HasValue && y.HasValue )
            {
                return -1;
            }
            else if ( x.HasValue && !y.HasValue )
            {
                return 1;
            }

            return x.Value.CompareTo( y );
        }
    }
}
