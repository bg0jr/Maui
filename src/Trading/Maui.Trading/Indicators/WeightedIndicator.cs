using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Reporting;
using Maui.Trading.Model;
using Blade.Collections;

namespace Maui.Trading.Indicators
{
    public class WeightedIndicator : IIndicator
    {
        private IIndicator myIndicator;

        /// <summary>
        /// Weight must be within ]0;1]
        /// </summary>
        public WeightedIndicator( IIndicator indicator, double weight )
        {
            CheckRange( weight );

            Weight = weight;
            myIndicator = indicator;
        }

        private void CheckRange( double weight )
        {
            if ( 0 < weight && weight <= 1 )
            {
                return;
            }

            throw new ArgumentOutOfRangeException( string.Format( "weight {0} must be between: ]0;1]", weight ) );
        }

        // take over the name of the inner indicator as this is actually an "invisible" decorator
        public string Name
        {
            get
            {
                return myIndicator.Name;
            }
        }

        public double Weight
        {
            get;
            private set;
        }

        public IndicatorResult Calculate( IIndicatorContext context )
        {
            var result = myIndicator.Calculate( context );
            return new WeightedIndicatorResult( this, result );
        }

        private class WeightedIndicatorResult : IndicatorResult
        {
            public WeightedIndicatorResult( WeightedIndicator indicator, IndicatorResult result )
                : base( indicator.Name, result.Stock )
            {
                Signal = result.Signal.Weight( indicator.Weight );

                ExpectedGain = result.ExpectedGain * indicator.Weight;
                GainRiskRatio = result.GainRiskRatio * indicator.Weight;

                Report = result.Report;

                Signals = result.Signals.Derive( new ObjectDescriptor( "Weighted" ), s => s.Weight( indicator.Weight ) );
            }
        }
    }
}
