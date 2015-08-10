using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Indicators;
using Maui.Trading.Model;

namespace Maui.Trading.Modules.Indicators
{
    public class TrendCuttingSignalStrategy : ISignalGenerationStrategy
    {
        public ISignalSeries Generate( IPriceSeries referencePrices, IPriceSeries indicatorPoints )
        {
            var signals = new List<TimedSignal>();

            var prevPrice = referencePrices.First();
            TimedValue<DateTime, double> prevIndicatorPoint = null;
            foreach ( var price in referencePrices.Skip( 1 ) )
            {
                var indicatorPoint = indicatorPoints.TryGet( price.Time );
                if ( indicatorPoint == null )
                {
                    signals.Add( new TimedSignal( price.Time, Signal.None ) );
                    continue;
                }

                if ( prevIndicatorPoint == null )
                {
                    prevIndicatorPoint = indicatorPoint;
                    continue;
                }

                if ( prevPrice.Value < prevIndicatorPoint.Value && price.Value > indicatorPoint.Value )
                {
                    signals.Add( new TimedSignal( indicatorPoint.Time, new BuySignal() ) );
                }
                else if ( prevPrice.Value > prevIndicatorPoint.Value && price.Value < indicatorPoint.Value )
                {
                    signals.Add( new TimedSignal( indicatorPoint.Time, new SellSignal() ) );
                }

                prevPrice = price;
            }

            return new SignalSeries( referencePrices, indicatorPoints.Identifier, signals );
        }
    }
}
