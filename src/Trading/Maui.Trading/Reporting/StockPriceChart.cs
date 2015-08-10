using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;
using Maui.Trading.Indicators;
using Maui.Entities;

namespace Maui.Trading.Reporting
{
    public class StockPriceChart
    {
        public StockPriceChart( StockHandle stock, IPriceSeries prices )
            : this( "StockPrices", stock, prices )
        {
        }

        public StockPriceChart( string name, StockHandle stock, IPriceSeries prices )
        {
            Name = name;
            Stock = stock;
            Prices = new PriceSeries( prices );

            IndicatorPoints = new Dictionary<string, IPriceSeries>();
            Signals = SignalSeries.Null;
        }

        public string Name
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

        public IDictionary<string, IPriceSeries> IndicatorPoints
        {
            get;
            private set;
        }

        public ISignalSeries Signals
        {
            get;
            set;
        }

        public bool IsEmpty
        {
            get
            {
                return !Prices.Any() && !IndicatorPoints.Any( e => e.Value.Any() ) && !Signals.Any();
            }
        }
    }
}
