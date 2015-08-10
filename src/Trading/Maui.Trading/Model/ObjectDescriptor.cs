using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Maui.Trading.Model
{
    /// <summary>
    /// Describes an object by a name/type and some parameters.
    /// The name/type would not be enough - only in connection with the parameters
    /// the object is non-ambiguously described.
    /// </summary>
    [DebuggerDisplay( "{ShortDesignator}" )]
    public class ObjectDescriptor : IObjectIdentifier
    {
        public static readonly ObjectDescriptor Null = new ObjectDescriptor( string.Empty );

        public ObjectDescriptor( string name, params KeyValuePair<string, object>[] parameters )
            : this( name, new Map<string, object>( parameters ) )
        {
        }

        public ObjectDescriptor( string name, IMap<string, object> parameters )
        {
            Name = name;
            Parameters = parameters;

            ShortDesignator = Name;
            LongDesignator = BuildLongDesignator();
            Guid = LongDesignator.GetHashCode();
        }

        private string BuildLongDesignator()
        {
            var sb = new StringBuilder();

            sb.Append( Name );

            sb.Append( "(" );

            var parameters = Parameters.ToList();
            for ( int i = 0; i < parameters.Count; ++i )
            {
                sb.Append( parameters[ i ].Key );
                sb.Append( "=" );
                sb.Append( parameters[ i ].Value );

                if ( i + 1 < parameters.Count )
                {
                    sb.Append( "," );
                }
            }

            sb.Append( ")" );

            return sb.ToString();
        }

        /// <summary>
        /// Name/type of the object.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        public IMap<string, object> Parameters
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

        public static KeyValuePair<string, object> Param( string key, object value )
        {
            return new KeyValuePair<string, object>( key, value );
        }
    }
}
