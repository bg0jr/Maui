using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;

namespace Maui.Trading.Indicators
{
    public abstract class AverageBasedCombinedSignal : CombinedSignal
    {
        private double myNeutralSignalsWeight;
        // offset used for "buy"/"sell" signals due to the "neutral" signals in the middle
        private int myNeutralSignalsOffset;

        protected AverageBasedCombinedSignal( double neutralSignalsWeight )
        {
            myNeutralSignalsWeight = neutralSignalsWeight;
            myNeutralSignalsOffset = (int)( Percentage.Hundred.Value * myNeutralSignalsWeight / 2 );
        }

        protected override void OnSignalsSet()
        {
            CalculateStrength();
            CalculateQuality();
        }

        private void CalculateStrength()
        {
            var relevantSignals = Signals
                .Where( s => s.Type != SignalType.None )
                .ToList();

            if ( !relevantSignals.Any() )
            {
                Strength = Percentage.Zero;
                return;
            }

            int combinedStrength = (int)relevantSignals
                .Select( s => GetNormStrength( s ) )
                .Average();

            UpdateFromNormStrength( combinedStrength );
        }

        private int GetNormStrength( Signal signal )
        {
            if ( signal.Type == SignalType.Buy )
            {
                return signal.Strength.RelativeValue + myNeutralSignalsOffset;
            }
            else if ( signal.Type == SignalType.Sell )
            {
                return ( signal.Strength.RelativeValue * -1 ) - myNeutralSignalsOffset;
            }
            else if ( signal.Type == SignalType.Neutral )
            {
                return (int)( signal.Strength.RelativeValue * myNeutralSignalsWeight ) - myNeutralSignalsOffset;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private void UpdateFromNormStrength( int normStrength )
        {
            // upper/lower limit of the normalized strength
            int limit = myNeutralSignalsOffset + Percentage.Hundred.Value;

            // due to "zero" the limits of the following ranges are overlapping. in this case always
            // take the worst signal (neutral instead of buy, sell instead of neutral)

            if ( -limit <= normStrength && normStrength <= -myNeutralSignalsOffset )
            {
                Strength = new Percentage( ( normStrength * -1 ) - myNeutralSignalsOffset );
                Type = SignalType.Sell;
            }
            else if ( -myNeutralSignalsOffset <= normStrength && normStrength <= myNeutralSignalsOffset )
            {
                Strength = new Percentage( (int)( ( normStrength + myNeutralSignalsOffset ) / myNeutralSignalsWeight ) );
                Type = SignalType.Neutral;
            }
            else if ( myNeutralSignalsOffset <= normStrength && normStrength <= limit )
            {
                Strength = new Percentage( normStrength - myNeutralSignalsOffset );
                Type = SignalType.Buy;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private void CalculateQuality()
        {
            int combinedQuality = (int)Signals.Average( s => s.Quality.Value );
            Quality = new Percentage( combinedQuality );
        }
    }
}
