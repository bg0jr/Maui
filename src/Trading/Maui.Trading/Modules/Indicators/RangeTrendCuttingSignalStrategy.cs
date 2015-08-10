using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Indicators;
using Maui.Trading.Model;

namespace Maui.Trading.Modules.Indicators
{
    public class RangeTrendCuttingSignalStrategy : ISignalGenerationStrategy
    {
        public RangeTrendCuttingSignalStrategy()
        {
            MinDaysAfterCutForSignal = 0;
            KeepSignalForMaxDays = 0;
        }

        /// <summary>
        /// Defines how much days after trend cut the indicator must stay above/below the prices
        /// to generate a signal
        /// </summary>
        public int MinDaysAfterCutForSignal
        {
            get;
            set;
        }

        /// <summary>
        /// Defines for how long the signal is kept.
        /// </summary>
        public int KeepSignalForMaxDays
        {
            get;
            set;
        }

        public ISignalSeries Generate( IPriceSeries referencePrices, IPriceSeries indicatorPoints )
        {
            var signals = new List<TimedSignal>();
            var signalDaysInterval = ClosedInterval.FromOffsetLength( MinDaysAfterCutForSignal, KeepSignalForMaxDays );

            var prevPrice = referencePrices.First();
            TimedValue<DateTime,double> prevIndicatorPoint = null;
            TimedSignal activeSignal = null;
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
                    var signal = new TimedSignal( indicatorPoint.Time, new BuySignal() );

                    if ( signalDaysInterval.IsEmpty )
                    {
                        signals.Add( signal );
                    }
                    else
                    {
                        activeSignal = signal;
                    }
                }
                else if ( prevPrice.Value > prevIndicatorPoint.Value && price.Value < indicatorPoint.Value )
                {
                    var signal = new TimedSignal( indicatorPoint.Time, new SellSignal() );

                    if ( signalDaysInterval.IsEmpty )
                    {
                        signals.Add( signal );
                    }
                    else
                    {
                        activeSignal = signal;
                    }
                }

                if ( activeSignal != null )
                {
                    // we have a cut signal -> handle it
                    int daysSinceCut = (int)Math.Round( ( price.Time - activeSignal.Time ).TotalDays );

                    // if we are in defined range -> add the signal
                    if ( signalDaysInterval.Includes( daysSinceCut ) )
                    {
                        signals.Add( new TimedSignal( indicatorPoint.Time, activeSignal.Value ) );
                    }

                    if ( daysSinceCut > signalDaysInterval.Max )
                    {
                        // left the interval -> reset the signal
                        activeSignal = null;
                    }
                }

                prevPrice = price;
                prevIndicatorPoint = indicatorPoint;
            }

            return new SignalSeries( referencePrices, indicatorPoints.Identifier, signals );
        }
    }
}
