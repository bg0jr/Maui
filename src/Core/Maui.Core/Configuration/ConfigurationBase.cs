using System.Linq;
using Blade.Collections;
using System;
using System.Globalization;

namespace Maui.Configuration
{
    public abstract class ConfigurationBase
    {
        private string myScope = null;
        protected ConfigurationSC myConfigSC = null;

        protected ConfigurationBase( string ns )
        {
            myConfigSC = ConfigurationSC.Instance;
            myScope = ns + ".";
        }

        public string this[ string key ]
        {
            get
            {
                return myConfigSC[ myScope + key ];
            }
            set
            {
                myConfigSC[ myScope + key ] = value;
            }
        }

        public bool Contains( string key )
        {
            return myConfigSC.Contains( myScope + key );
        }

        /// <summary>
        /// Return the configuration value for the given key. If the
        /// key doesn't exist, it returns the optional defaultValue.
        /// </summary>
        public string Get( string key, string defaultValue )
        {
            return myConfigSC.Contains( myScope + key ) ? this[ key ] : defaultValue;
        }

        public string Get( string key )
        {
            return Get( key, null );
        }

        public T Get<T>( string key, T defaultValue )
        {
            var value = Get( key );
            return value != null ? (T)Convert.ChangeType( value, typeof( T ), CultureInfo.InvariantCulture ) : defaultValue;
        }

        /// <summary>
        /// Set a default value to the given item. Must be called by GT itself to
        /// give reasonable default values to most of configurations items.
        /// </summary>
        public void SetDefault( string key, string value )
        {
            myConfigSC.SetDefault( myScope + key, value );
        }

        /// <summary>
        /// Return the value of the first item that does have a non-zero value.
        /// </summary>
        public string GetFirst( params string[] keys )
        {
            return myConfigSC.Keys.Select( k => this[ k ] ).FirstOrDefault( v => v != null, string.Empty );
        }
    }

    public abstract class ConfigurationBase<T> : ConfigurationBase
    {
        protected ConfigurationBase()
            : base( typeof( T ).ToString() )
        {
        }
    }
}
