using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;
using Maui.Trading.Reporting;
using Maui.Entities;

namespace Maui.Trading.Indicators
{
    public class IndicatorResult : IAnalysisResult
    {
        public IndicatorResult( string indicator, StockHandle stock, Signal signal )
            : this( indicator, stock )
        {
            Signal = signal;
        }

        protected IndicatorResult( string indicator, StockHandle stock )
        {
            Indicator = indicator;
            Stock = stock;
            Signals = SignalSeries.Null;
        }

        public string Indicator
        {
            get;
            private set;
        }

        public StockHandle Stock
        {
            get;
            private set;
        }

        public Signal Signal
        {
            get;
            protected set;
        }

        public double? ExpectedGain
        {
            get;
            set;
        }

        public double? GainRiskRatio
        {
            get;
            set;
        }

        public Report Report
        {
            get;
            set;
        }

        // also contain the current signal
        public ISignalSeries Signals
        {
            get;
            set;
        }
    }
}
