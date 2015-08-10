using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Maui.Trading.Model
{
    /// <summary>
    /// Modifiers are usually used if only a filter has been applied to the series but not a 
    /// transformator (time axis could have been changed but the value axis did not).
    /// We "derive" from a series if we transform the value axis of the series.
    /// </summary>
    [DebuggerDisplay( "{ShortDesignator}" )]
    public class SeriesIdentifier : IObjectIdentifier
    {
        public static readonly SeriesIdentifier Null = new SeriesIdentifier( NullObjectIdentifier.Null, ObjectDescriptor.Null );

        public SeriesIdentifier( IObjectIdentifier owner, ObjectDescriptor type, params SeriesIdentifier[] sources )
            : this( owner, type, sources, new List<ObjectDescriptor>() )
        {
        }

        public SeriesIdentifier( IObjectIdentifier owner, ObjectDescriptor type, IEnumerable<SeriesIdentifier> sources )
            : this( owner, type, sources, new List<ObjectDescriptor>() )
        {
        }

        public SeriesIdentifier( IObjectIdentifier owner, ObjectDescriptor type, IEnumerable<SeriesIdentifier> sources, IEnumerable<ObjectDescriptor> modifiers )
        {
            Owner = owner;
            Type = type;
            Sources = sources.ToList();
            Modifiers = modifiers;

            ShortDesignator = Owner.ShortDesignator + "." + Type.Name;
            LongDesignator = BuildLongDesignator();
            Guid = LongDesignator.GetHashCode();
        }

        private string BuildLongDesignator()
        {
            var sb = new StringBuilder();

            sb.Append( Owner.LongDesignator );
            sb.Append( "." );

            foreach ( var source in Sources )
            {
                sb.Append( source.LongDesignator );
                sb.Append( "." );
            }

            sb.Append( Type.LongDesignator );

            if ( Modifiers.Any() )
            {
                foreach ( var mod in Modifiers )
                {
                    sb.Append( "." );
                    sb.Append( mod.LongDesignator );
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Usually the stock identifier. Might be the Isin + StockExchange symbol.
        /// Should identify the stock non-ambiguously.
        /// </summary>
        public IObjectIdentifier Owner
        {
            get;
            private set;
        }

        /// <summary>
        /// Origins of this series. For pure price series this is empty.
        /// For indicator series this might contain the price series or
        /// in case of a combined inidcator the source indicators.
        /// </summary>
        public IEnumerable<SeriesIdentifier> Sources
        {
            get;
            private set;
        }

        /// <summary>
        /// The actual type of the series e.g. "prices" or "SMA.38".
        /// </summary>
        public ObjectDescriptor Type
        {
            get;
            private set;
        }

        /// <summary>
        /// Defines different operators/modifiers that have been applied to 
        /// the original series. e.g. "ThinOut", "Range", "MissingDates"
        /// Empty for original series.
        /// </summary>
        public IEnumerable<ObjectDescriptor> Modifiers
        {
            get;
            private set;
        }

        public string ShortDesignator
        {
            get;
            private set;
        }

        public string LongDesignator
        {
            get;
            private set;
        }

        public int Guid
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return LongDesignator;
        }

        public SeriesIdentifier Modify( ObjectDescriptor modifier )
        {
            var mods = Modifiers.ToList();
            mods.Add( modifier );

            return new SeriesIdentifier( Owner, Type, Sources, mods );
        }

        public SeriesIdentifier Derive( ObjectDescriptor type )
        {
            return new SeriesIdentifier( Owner, type, this );
        }
    }
}
