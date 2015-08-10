using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Indicators
{
    /// <summary>
    /// Rates Buy signals higher than Neutral signals higher than sell signals.
    /// If both signals are of the same type the strength is compared.
    /// If both signals are of the same strength the quality is compared.
    /// if both signals are of the same quality the number of "sub signals" (
    /// in case of combined signals) is compared.
    /// </summary>
    public class DefaultSignalComparer : IComparer<Signal>
    {
        public int Compare( Signal x, Signal y )
        {
            int typeCompare = x.Type.CompareTo( y.Type );
            if ( typeCompare != 0 )
            {
                return typeCompare;
            }

            int strengthCompare = x.Strength.CompareTo( y.Strength );
            if ( strengthCompare != 0 )
            {
                return strengthCompare;
            }

            int qualityCompare = x.Quality.CompareTo(  y.Quality );
            if ( qualityCompare != 0 )
            {
                return qualityCompare;
            }

            int subSignalCountX = GetSubSignalCount( x );
            int subSignalCountY = GetSubSignalCount( y );

            return subSignalCountX.CompareTo( subSignalCountY );
        }

        private int GetSubSignalCount( Signal signal )
        {
            var combinedSignal = signal as CombinedSignal;
            if ( combinedSignal == null )
            {
                return 1;
            }

            return combinedSignal.Signals.Count();
        }
    }
}
