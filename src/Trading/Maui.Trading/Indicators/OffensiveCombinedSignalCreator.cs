using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;

namespace Maui.Trading.Indicators
{
    public class OffensiveCombinedSignalCreator : AbstractCombinedSignalCreator
    {
        public override Signal Create( IEnumerable<Signal> signals )
        {
            return new OffensiveCombinedSignal( signals );
        }
    }
}
