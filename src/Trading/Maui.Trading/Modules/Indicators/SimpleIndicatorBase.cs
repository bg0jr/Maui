using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Indicators;
using Maui.Trading.Model;
using Maui.Trading.Binding;
using Maui.Trading.Binding.Decorators;
using Maui.Entities;

namespace Maui.Trading.Modules.Indicators
{
    public abstract class SimpleIndicatorBase : AbstractIndicator
    {
        protected SimpleIndicatorBase( string name )
            : base( name )
        {
        }

        [DataSource]
        public IPriceSeriesDataSource Prices
        {
            get;
            set;
        }
        
        public override IndicatorResult Calculate( IIndicatorContext context )
        {
            if ( !IsEnoughDataAvailable( context ) )
            {
                return GenerateMissingDataResult( context );
            }

            return CalculateResult( context );
        }

        protected virtual bool IsEnoughDataAvailable( IIndicatorContext context )
        {
            var prices = Prices.ForStock( context.Stock );
            
            if ( !prices.Any() )
            {
                return false;
            }

            if ( prices.Last().Time < context.DateUnderAnalysis && !context.GenerateHistoricSignals )
            {
                // we should only generate signals for DUA but there is no data available for it
                return false;
            }

            return true;
        }

        private IndicatorResult GenerateMissingDataResult( IIndicatorContext context )
        {
            var result = new IndicatorResult( Name, context.Stock, Signal.None );
            result.Report = new MissingDataReport( Name );

            return result;
        }

        protected abstract IndicatorResult CalculateResult( IIndicatorContext context );

        protected class WorkerBase<TIndicator>
        {
            protected WorkerBase( TIndicator indicator, IIndicatorContext context )
            {
                Indicator = indicator;
                Context = context;
            }

            protected TIndicator Indicator
            {
                get;
                private set;
            }

            protected IIndicatorContext Context
            {
                get;
                private set;
            }

            protected StockHandle Stock
            {
                get
                {
                    return Context.Stock;
                }
            }

            protected Signal GetCurrentSignal( ISignalSeries signals, IPriceSeries indicatorPoints )
            {
                if ( indicatorPoints.Last().Time < Context.DateUnderAnalysis )
                {
                    // no data for the DUA
                    return Signal.None;
                }

                // we had data so default is "neutral"
                Signal currentSignal = new NeutralSignal();

                var currentTimedSignal = signals.FirstOrDefault( s => s.Time == Context.DateUnderAnalysis );
                if ( currentTimedSignal != null )
                {
                    // found a "better" signal for the DUA - take this one
                    currentSignal = currentTimedSignal.Value;
                }

                return currentSignal;
            }
        }
    }
}
