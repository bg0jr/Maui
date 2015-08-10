using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;
using Maui.Trading.Indicators;

namespace Maui.Trading.Reporting
{
    public class SignalSeriesSection : AbstractSection
    {
        public SignalSeriesSection( string name, ISignalSeries signals )
            : base( name )
        {
            Signals = signals;
        }

        public ISignalSeries Signals
        {
            get;
            private set;
        }
    }
}
