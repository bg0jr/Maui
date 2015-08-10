using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Utils;

namespace Maui.Trading.Indicators
{
    // sorts by: Signal, GainRiskRatio, ExpectedGain
    public class IndicatorResultComparer : IComparer<IndicatorResult>
    {
        private IComparer<Signal> mySignalComparer;

        public IndicatorResultComparer()
            : this( new DefaultSignalComparer() )
        {
        }

        public IndicatorResultComparer( IComparer<Signal> signalComparer )
        {
            mySignalComparer = signalComparer;
        }

        public int Compare( IndicatorResult x, IndicatorResult y )
        {
            int ret = mySignalComparer.Compare( x.Signal, y.Signal );
            if ( ret != 0 )
            {
                return ret;
            }

            ret = Maths.Compare( x.GainRiskRatio, y.GainRiskRatio );
            if ( ret != 0 )
            {
                return ret;
            }

            ret = Maths.Compare( x.ExpectedGain, y.ExpectedGain );
            if ( ret != 0 )
            {
                return ret;
            }

            return 0;
        }
    }
}
