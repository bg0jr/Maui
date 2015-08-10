using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Indicators;
using Maui.Trading.Binding;
using Maui.Trading.Binding.Decorators;
using Maui.Trading.Model;
using Maui.Entities;

namespace Maui.Trading.Modules.Indicators.TrendFollowing
{
    public class DoubleCrossover : SimpleIndicatorBase
    {
        public DoubleCrossover( ISeriesCalculator shortTerm, ISeriesCalculator longTerm )
            : base( shortTerm.Name + "x" + longTerm.Name )
        {
            ShortTerm = shortTerm;
            LongTerm = longTerm;

            SignalGenerationStrategy = new TrendCuttingSignalStrategy();
        }

        public ISignalGenerationStrategy SignalGenerationStrategy
        {
            get;
            set;
        }

        public ISeriesCalculator ShortTerm
        {
            get;
            private set;
        }

        public ISeriesCalculator LongTerm
        {
            get;
            private set;
        }


        protected override bool IsEnoughDataAvailable( IIndicatorContext context )
        {
            if ( !base.IsEnoughDataAvailable( context ) )
            {
                return false;
            }

            var prices = Prices.ForStock( context.Stock );

            if ( !ShortTerm.ContainsEnoughData( prices ) || !LongTerm.ContainsEnoughData( prices ) )
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

        private class Worker : WorkerBase<DoubleCrossover>
        {
            private IPriceSeries myPrices;

            public Worker( DoubleCrossover indicator, IIndicatorContext context )
                : base( indicator, context )
            {
            }

            public IndicatorResult Calculate()
            {
                myPrices = Indicator.Prices.ForStock( Stock );

                var shortTermPoints = Indicator.ShortTerm.Calculate( myPrices );
                var longTermPoints = Indicator.LongTerm.Calculate( myPrices );

                var result = GenerateResult( shortTermPoints, longTermPoints );
                return result;
            }

            private IndicatorResult GenerateResult( IPriceSeries shortTermPoints, IPriceSeries longTermPoints )
            {
                var signals = Indicator.SignalGenerationStrategy.Generate( shortTermPoints, longTermPoints );

                var reportData = new GenericIndicatorReport.Data()
                {
                    SignalOfDayUnderAnalysis = GetCurrentSignal( signals, longTermPoints ),
                    Prices = myPrices,
                    Signals = signals
                };
                reportData.Points[ Indicator.ShortTerm.Name ] = shortTermPoints;
                reportData.Points[ Indicator.LongTerm.Name ] = longTermPoints;

                var result = new IndicatorResult( Indicator.Name, Stock, reportData.SignalOfDayUnderAnalysis );

                if ( Context.GenerateHistoricSignals )
                {
                    result.Signals = signals;
                }

                result.Report = new DoubleCrossoverReport( Indicator, Stock, reportData );

                return result;
            }
        }

        private class DoubleCrossoverReport : GenericIndicatorReport
        {
            public DoubleCrossoverReport( DoubleCrossover indicator, StockHandle stock, Data data )
                : base( indicator.Name, "Double Crossover " + indicator.Name + ": " + stock.Name, stock, data )
            {
            }
        }
    }
}
