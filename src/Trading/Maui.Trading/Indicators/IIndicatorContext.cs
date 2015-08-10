using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;
using Maui.Entities;

namespace Maui.Trading.Indicators
{
    public interface IIndicatorContext
    {
        StockHandle Stock { get; }

        DateTime DateUnderAnalysis { get; }

        bool GenerateHistoricSignals { get; }

        ICalculatorFactory CalculatorFactory { get; }
    }
}
