using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Indicators;
using Maui.Trading.Model;

namespace Maui.Trading.Modules.Indicators
{
    public interface ISignalGenerationStrategy
    {
        /// <summary>
        /// Does not return a signal for some timepoint if there is nothing to signal.
        /// Timepoints for which it could not be determined which signal should be generated
        /// then a none-signal is created.
        /// </summary>
        ISignalSeries Generate( IPriceSeries referencePrices, IPriceSeries indicatorPoints );
    }
}
