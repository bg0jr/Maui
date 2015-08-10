using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;

namespace Maui.Trading.Indicators
{
    public interface ICombinedSignalCreator
    {
        // merge signals
        Signal Create( IEnumerable<Signal> signals );

        // merge historical signals
        ISignalSeries Create( IEnumerable<ISignalSeries> signals );
    }
}
