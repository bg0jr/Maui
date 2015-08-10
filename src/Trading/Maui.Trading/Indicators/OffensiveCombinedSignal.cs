using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;

namespace Maui.Trading.Indicators
{
    public class OffensiveCombinedSignal : AverageBasedCombinedSignal
    {
        public OffensiveCombinedSignal( params Signal[] signals )
            : this( signals.ToList() )
        {
        }

        public OffensiveCombinedSignal( IEnumerable<Signal> signals )
            : base( 0.5 )
        {
            Signals = signals;
        }

        public OffensiveCombinedSignal()
            : base( 0.5 )
        {
        }

        protected override Signal CreateTemplate()
        {
            return new OffensiveCombinedSignal();
        }
    }
}
