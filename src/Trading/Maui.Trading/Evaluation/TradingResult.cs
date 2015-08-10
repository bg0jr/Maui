using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Entities;
using Maui.Trading.Modules;
using Maui.Trading.Model;
using Maui.Trading.Reporting;

namespace Maui.Trading.Evaluation
{
    public class TradingResult 
    {
        private SystemResult mySystemResult;

        public TradingResult( SystemResult systemResult )
        {
            mySystemResult = systemResult;
        }

        public StockHandle Stock
        {
            get
            {
                return mySystemResult.Stock;
            }
        }

        public IPriceSeries Prices
        {
            get
            {
                return mySystemResult.Prices;
            }
        }

        public ISignalSeries SystemSignals
        {
            get
            {
                return mySystemResult.Signals;
            }
        }

        public Report SystemReport
        {
            get
            {
                return mySystemResult.Report;
            }
        }

        public TimeSpan TradingTimeSpan
        {
            get;
            set;
        }

        public double InitialCash
        {
            get;
            set;
        }

        public double PortfolioValue
        {
            get;
            set;
        }

        public double GainPerAnno
        {
            get
            {
                var totalGain = ( PortfolioValue - InitialCash ) / InitialCash * 100;
                return totalGain / TradingTimeSpan.TotalDays * 365;
            }
        }

        public TradingLog TradingLog
        {
            get;
            set;
        }
    }
}
