using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;
using Maui.Entities;

namespace Maui.Trading.Indicators
{
    public class IndicatorContext : IIndicatorContext
    {
        public IndicatorContext()
        {
            GenerateHistoricSignals = false;
            DateUnderAnalysis = DateTime.Today;
        }

        public StockHandle Stock { get; set; }

        // day for which the indicator should generate signals if any
        // default: today
        public DateTime DateUnderAnalysis { get; set; }

        // default: false
        public bool GenerateHistoricSignals
        {
            get;
            set;
        }

        public ICalculatorFactory CalculatorFactory
        {
            get;
            set;
        }
    }
}
