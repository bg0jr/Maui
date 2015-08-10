using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;
using Maui.Trading.Utils;

namespace Maui.Trading.Indicators
{
    public abstract class Signal : IEquatable<Signal>
    {
        public static readonly Signal None = new NoSignal();

        protected Signal( SignalType type )
            : this( type, Percentage.Hundred )
        {
        }

        protected Signal( SignalType type, Percentage strength )
            : this( type, strength, Percentage.Hundred )
        {
        }

        protected Signal( SignalType type, Percentage strength, Percentage quality )
        {
            if ( type == SignalType.None )
            {
                throw new ArgumentException( "SignalType.None is not allowed. Use Signal.None instead" );
            }

            Type = type;
            Strength = strength;
            Quality = quality;
        }

        /// <summary>
        /// Creates a non-initialized signal
        /// </summary>
        protected Signal()
        {
        }

        public SignalType Type
        {
            get;
            protected set;
        }

        public Percentage Strength
        {
            get;
            protected set;
        }

        /// <summary>
        /// Indicates the quality of the signal (e.g. is combined signal and some data was
        /// missing the quality of the signal gets reduced, if all data was available
        /// and the data is trustworthiness the quality is at max)
        /// The Quality of non-combined signals should be at max. It could be used for non-combined
        /// signals also for indicators to indicate that the data the signal is based on
        /// is not trustworthiness (e.g. values are estimated or values are pre-calculated
        /// from some internet site)
        /// </summary>
        public Percentage Quality
        {
            get;
            protected set;
        }

        public virtual Signal Weight( double factor )
        {
            Maths.ValidateValueAgainstRange( factor, 0, 1 );

            var signal = CreateTemplate();
            signal.Type = Type;
            signal.Strength = new Percentage( (int)( Strength.Value * factor ) );
            signal.Quality = Quality;

            return signal;
        }

        /// <summary>
        /// Creates a non-initialized instance of this signal type.
        /// </summary>
        protected abstract Signal CreateTemplate();

        public override bool Equals( object obj )
        {
            return Equals( obj as Signal );
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return string.Format( "{0}-{1}-{2}", Type, Strength, Quality );
        }

        public bool Equals( Signal other )
        {
            if ( other == null )
            {
                return false;
            }

            if ( object.ReferenceEquals( this, other ) )
            {
                return true;
            }

            return Type == other.Type &&
                Strength == other.Strength &&
                Quality == other.Quality;
        }
    }
}
