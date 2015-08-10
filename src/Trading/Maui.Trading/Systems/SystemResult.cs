using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;
using Maui.Trading.Reporting;
using Maui.Trading.Indicators;
using Maui.Entities;

namespace Maui.Trading.Modules
{
    public class SystemResult : IAnalysisResult
    {
        public SystemResult( string system, StockHandle stock, IPriceSeries prices, IndicatorResult indicatorResult )
        {
            System = system;
            Stock = stock;
            Prices = prices;

            Signal = indicatorResult.Signal;
            ExpectedGain = indicatorResult.ExpectedGain;
            GainRiskRatio = indicatorResult.GainRiskRatio;
            Signals = indicatorResult.Signals;
        }

        public string System
        {
            get;
            private set;
        }

        public StockHandle Stock
        {
            get;
            private set;
        }

        public IPriceSeries Prices
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
