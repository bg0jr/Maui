using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Modules;
using Maui.Trading.Indicators;
using Maui.Trading.Utils;

namespace Maui.Trading.Modules
{
    public class SystemResultComparer : IComparer<SystemResult>
    {
        private IComparer<Signal> mySignalComparer;

        public SystemResultComparer()
            : this( new DefaultSignalComparer() )
        {
        }

        public SystemResultComparer( IComparer<Signal> signalComparer )
        {
            mySignalComparer = signalComparer;
        }

        public int Compare( SystemResult x, SystemResult y )
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
