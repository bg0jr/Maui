using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Data.Sheets
{
    /// <summary>
    /// Represents one section in the workbook config.
    /// <remarks>
    /// It is a snapshot. Modifications will have no effect.
    /// </remarks>
    /// </summary>
    public class ConfigSection
    {
        /// <summary/>
        public ConfigSection( string name )
        {
            Name = name;
            Properties = new List<KeyValuePair<string, string>>();
        }

        /// <summary/>
        public string Name { get; private set; }

        /// <summary/>
        public List<KeyValuePair<string, string>> Properties { get; private set; }

        /// <summary/>
        public void SetProperty( string name, string value )
        {
            var pair = Properties.FirstOrDefault( p => p.Key == name );
            if ( !pair.Equals( default( KeyValuePair<string, string> ) ) )
            {
                Properties.Remove( pair );
            }

            Properties.Add( new KeyValuePair<string, string>( name, value ) );
        }

        /// <summary>
        /// Gets the value of the property with the given name. Returns null
        /// if the property does not exist.
        /// </summary>
        public string GetProperty( string name )
        {
            var pair = Properties.FirstOrDefault( p => p.Key == name );
            if ( pair.Equals( default( KeyValuePair<string, string> ) ) )
            {
                return null;
            }

            return pair.Value;
        }

        /// <summary/>
        public void SetProperties( IEnumerable<KeyValuePair<string, string>> properties )
        {
            foreach ( var pair in properties )
            {
                SetProperty( pair.Key, pair.Value );
            }
        }
    }
}
