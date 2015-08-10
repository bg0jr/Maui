using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;
using Maui.Trading.Utils;

namespace Maui.Trading.Indicators
{
    public class PerfMonSeriesCalculatorDecorator : ISeriesCalculator
    {
        private ISeriesCalculator myCalculator;

        public PerfMonSeriesCalculatorDecorator( ISeriesCalculator calculator )
        {
            myCalculator = calculator;
        }

        public string Name
        {
            get
            {
                return myCalculator.Name;
            }
        }

        public bool ContainsEnoughData( IPriceSeries prices )
        {
            return myCalculator.ContainsEnoughData( prices );
        }

        public IPriceSeries Calculate( IPriceSeries prices )
        {
            return myCalculator.Calculate( prices );
        }
    }
}
