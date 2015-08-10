using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;

namespace Maui.Trading.Indicators
{
    public class DefensiveCombinedSignal : AverageBasedCombinedSignal
    {
        public DefensiveCombinedSignal( params Signal[] signals )
            : this( signals.ToList() )
        {
        }

        public DefensiveCombinedSignal( IEnumerable<Signal> signals )
            : base( 1 )
        {
            Signals = signals;
        }

        public DefensiveCombinedSignal()
            : base( 1 )
        {
        }

        protected override Signal CreateTemplate()
        {
            return new DefensiveCombinedSignal();
        }
    }
}
