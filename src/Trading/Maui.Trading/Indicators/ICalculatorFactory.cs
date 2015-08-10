using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Indicators
{
    public interface ICalculatorFactory
    {
        bool CanCreate( string name );
        ISeriesCalculator Create( string name, params object[] ctorArgs );
    }
}
