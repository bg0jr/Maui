using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;

namespace Maui.Trading.Indicators
{
    public interface ISeriesCalculator
    {
        string Name { get; }
        bool ContainsEnoughData( IPriceSeries prices );
        IPriceSeries Calculate( IPriceSeries prices );
    }
}
