using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;

namespace Maui.Trading.Reporting
{
    /// <summary>
    /// Holds points of an indicator (e.g. MovingAverage).
    /// Name of the section should match name of the indicator.
    /// </summary>
    public class IndicatorPointsSection : AbstractSection
    {
        public IndicatorPointsSection( string name, IPriceSeries series )
            : base( name )
        {
            Series = series;
        }

        public IPriceSeries Series
        {
            get;
            private set;
        }
    }
}
