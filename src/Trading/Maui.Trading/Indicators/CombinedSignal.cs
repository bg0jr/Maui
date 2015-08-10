using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;
using Maui.Trading.Utils;

namespace Maui.Trading.Indicators
{
    public abstract class CombinedSignal : Signal, IEquatable<CombinedSignal>
    {
        private IEnumerable<Signal> mySignals;

        protected CombinedSignal( IEnumerable<Signal> signals )
        {
            Signals = signals;
        }

        // create non-initialized instance
        protected CombinedSignal()
        {
        }

        public IEnumerable<Signal> Signals
        {
            get
            {
                return mySignals;
            }
            protected set
            {
                if ( mySignals != null )
                {
                    throw new NotSupportedException( "Signals should be immutable" );
                }

                mySignals = GetRawSignals( value );
                OnSignalsSet();
            }
        }

        protected abstract void OnSignalsSet();

        // get all signals. if combined signals are inside extract the raw signals
        private IEnumerable<Signal> GetRawSignals( IEnumerable<Signal> signals )
        {
            var allSignals = new List<Signal>();

            foreach ( var signal in signals )
            {
                if ( signal is CombinedSignal )
                {
                    var combinedSignal = (CombinedSignal)signal;
                    allSignals.AddRange( GetRawSignals( combinedSignal.Signals ) );
                }
                else
                {
                    allSignals.Add( signal );
                }
            }

            return allSignals;
        }

        public override Signal Weight( double factor )
        {
            Maths.ValidateValueAgainstRange( factor, 0, 1 );

            var weightedSignals = Signals.Select( s => s.Weight( factor ) );

            var signal = CreateCombinedTemplate();
            signal.Signals = weightedSignals;

            return signal;
        }

        private CombinedSignal CreateCombinedTemplate()
        {
            return (CombinedSignal)CreateTemplate();
        }

        public bool Equals( CombinedSignal other )
        {
            return base.Equals( other as Signal );
        }
    }
}
