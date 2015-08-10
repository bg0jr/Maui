using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Binding;
using Maui.Trading.Model;
using Maui.Trading.Indicators;
using Maui.Trading.Reporting;
using Blade.Collections;
using Maui.Trading.Binding.Decorators;
using Maui.Entities;

namespace Maui.Trading.Modules.Indicators.TrendFollowing
{
    public class SimpleMovingAverage : SimpleIndicatorBase
    {
        public SimpleMovingAverage( int numDays )
            : base( "SMA." + numDays )
        {
            NumDays = numDays;
            SignalGenerationStrategy = new TrendCuttingSignalStrategy();
        }

        /// <summary>
        /// Number of days used in the average
        /// </summary>
        public int NumDays
        {
            get;
            private set;
        }

        public ISignalGenerationStrategy SignalGenerationStrategy
        {
            get;
            set;
        }

        protected override bool IsEnoughDataAvailable( IIndicatorContext context )
        {
            if ( !base.IsEnoughDataAvailable( context ) )
            {
                return false;
            }
            
            var calculator = context.CalculatorFactory.Create( "sma", NumDays );

            var prices = Prices.ForStock( context.Stock );
            if ( !calculator.ContainsEnoughData( prices ) )
            {
                // really too few data to calculate anything
                return false;
            }

            return true;
        }

        protected override IndicatorResult CalculateResult( IIndicatorContext context )
        {
            var worker = new Worker( this, context );
            var result = worker.Calculate();

            return result;
        }

        private class Worker : WorkerBase<SimpleMovingAverage>
        {
            private IPriceSeries myPrices;

            public Worker( SimpleMovingAverage indicator, IIndicatorContext context )
                : base( indicator, context )
            {
            }

            public IndicatorResult Calculate()
            {
                myPrices = Indicator.Prices.ForStock( Stock );

                var points = CalculatePoints();

                var result = GenerateResult( points );
                return result;
            }

            private IPriceSeries CalculatePoints()
            {
                var calculator = Context.CalculatorFactory.Create( "sma", Indicator.NumDays );

                // always take the complete range here even if we should not generate historical signals
                // because of the signal creation strategy which sill might need some historical data
                return calculator.Calculate( myPrices );
            }

            private IndicatorResult GenerateResult( IPriceSeries points )
            {
                var signals = Indicator.SignalGenerationStrategy.Generate( myPrices, points );

                var reportData = new GenericIndicatorReport.Data()
                {
                    SignalOfDayUnderAnalysis = GetCurrentSignal( signals, points ),
                    Prices = myPrices,
                    Signals = signals
                };
                reportData.Points[ Indicator.Name ] = points;

                var result = new IndicatorResult( Indicator.Name, Stock, reportData.SignalOfDayUnderAnalysis );

                if ( Context.GenerateHistoricSignals )
                {
                    result.Signals = signals;
                }

                result.Report = new SmaReport( Indicator, Stock, reportData );

                return result;
            }
        }

        private class SmaReport : GenericIndicatorReport
        {
            public SmaReport( SimpleMovingAverage indicator, StockHandle stock, Data data )
                : base( indicator.Name, "Simple moving average " + indicator.NumDays + ": " + stock.Name, stock, data )
            {
            }
        }
    }
}
