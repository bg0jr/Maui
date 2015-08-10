using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Reporting;
using Maui.Trading.Model;

namespace Maui.Trading.Indicators
{
    public interface IIndicator
    {
        string Name { get; }
        IndicatorResult Calculate( IIndicatorContext context );
    }
}
