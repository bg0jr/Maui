using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;
using Blade;
using Blade.IO;
using Blade.Collections;
using Blade.Reflection;
using Blade.Text;
using Maui;
using Maui.Logging;
using Blade.Logging;

namespace Maui.Configuration
{
    /// <summary>
    /// Implemented as singleton but needs to be initialized if used
    /// first time.
    /// <remarks>
    /// TODO: move to table in DB
    /// </remarks>
    /// </summary>
    public class ConfigurationSC : ManagedObject, IServiceComponent
    {
        private Dictionary<string, string> myConfig = null;
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( ConfigurationSC ) );

        private Dictionary<string, string> myGlobalVars = null;

        private ConfigurationSC()
        {
            myConfig = new Dictionary<string, string>();
            SchemaSet = new XmlSchemaSet();

            myGlobalVars = new Dictionary<string, string>();
            myGlobalVars[ "MauiHome" ] = MauiEnvironment.Root;
        }

        private static ConfigurationSC myInstance = null;
        public static ConfigurationSC Instance
        {
            get
            {
                if ( myInstance == null )
                {
                    myInstance = new ConfigurationSC();
                }
                return myInstance;
            }
        }

        public XmlSchemaSet SchemaSet { get; private set; }

        public void Init( ServiceProvider serviceProvider )
        {
            var files = Directory.GetFiles( OS.CombinePaths( MauiEnvironment.Config, "xsd" ), "*.xsd" );
            foreach ( var file in files )
            {
                SchemaSet.Add( null, file );
            }
        }

        protected override void Dispose( bool disposing )
        {
            try
            {
                if ( IsDisposed )
                {
                    return;
                }

                if ( disposing )
                {
                    // TODO: we will need this later to store changed values into db
                }
            }
            finally
            {
                base.Dispose( disposing );
            }
        }

        /// <summary>
        /// Imports the given path into the configuration.
        /// If the path points to a file the file is imported.
        /// If the path points to a directory all files in the
        /// directory will be imported (recursivly and in alphabetical order, 
        /// "defaults" directory is always loaded first if available).
        /// <remarks>
        /// The files need to be valid Maui (xml) config files.
        /// </remarks>
        /// </summary>
        public void Import( string path, bool overwriteExisting )
        {
            if ( Directory.Exists( path ) )
            {
                string[] directories = Directory.GetDirectories( path );
                directories.Foreach( d => Import( d, overwriteExisting ) );

                string[] files = Directory.GetFiles( path, "*.xml" );
                files.Foreach( f => ImportFile( f, overwriteExisting ) );
            }
            else if ( File.Exists( path ) )
            {
                ImportFile( path, overwriteExisting );
            }
            else
            {
                myLogger.Warning( "No such directory: {0}", path );
            }
        }

        public string this[ string key ]
        {
            get
            {
                if ( !myConfig.ContainsKey( key ) )
                {
                    throw new KeyNotFoundException( "Key not found in configuration: " + key );
                }

                return myConfig[ key ];
            }
            set
            {
                myConfig[ key ] = value;
            }
        }

        public bool Contains( string key )
        {
            return myConfig.ContainsKey( key );
        }

        public bool Contains( Type type, string key )
        {
            return myConfig.ContainsKey( type.FullName + "." + key );
        }

        public string GetValue( Type type, string key )
        {
            return this[ type.FullName + "." + key ];
        }

        public void SetValue( Type type, string key, string value )
        {
            this[ type.FullName + "." + key ] = value;
        }

        public DelegateAccessor<string> CreateAccessor( Type type, string key )
        {
            return new DelegateAccessor<string>( () => GetValue( type, key ), value => SetValue( type, key, value ) );
        }

        public DelegateAccessor<string> CreateAccessor( string key )
        {
            return new DelegateAccessor<string>( () => this[ key ], value => this[ key ] = value );
        }

        /// <summary>
        /// Sets the given key and value if there is not already a value
        /// set for the key.
        /// </summary>
        public void SetDefault( string key, string value )
        {
            if ( !myConfig.ContainsKey( key ) )
            {
                this[ key ] = value;
            }
        }

        /// <summary>
        /// Sets the given key and value if there is not already a value
        /// set for the key.
        /// </summary>
        public void SetDefault( Type type, string key, string value )
        {
            SetDefault( type.FullName + "." + key, value );
        }

        public IEnumerable<string> GetKeys( Type type )
        {
            return myConfig.Keys.Where( k => k.StartsWith( type.FullName ) );
        }

        public IEnumerable<string> Keys
        {
            get
            {
                return myConfig.Keys;
            }
        }

        public string TryGetValue( string key )
        {
            return ( myConfig.ContainsKey( key ) ? this[ key ] : null );
        }

        public string TryGetValue( Type type, string key )
        {
            key = type.FullName + "." + key;
            return TryGetValue( key );
        }

        private void ImportFile( string file, bool overwriteExisting )
        {
            myLogger.Info( "Importing config file: {0}", file );

            XDocument doc = XDocument.Load( file );
            doc.Validate( SchemaSet, null );

            XAttribute attr = doc.Root.Attribute( "namespace" );
            string ns = ( attr != null ? attr.Value : null );

            foreach ( XElement property in doc.Root.Elements( doc.Root.Name.Namespace + "property" ) )
            {
                string key = property.Attribute( "key" ).Value.Trim();
                if ( ns != null )
                {
                    key = ns + "." + key;
                }

                if ( myConfig.ContainsKey( key ) && !overwriteExisting )
                {
                    // we already know this property and we have not to overwrite it
                    continue;
                }

                string value = null;
                if ( property.Attribute( "value" ) != null )
                {
                    value = Evaluate( property.Attribute( "value" ).Value );
                }
                else
                {
                    value = property.Value;
                }

                myConfig[ key ] = value;
            }
        }

        private string Evaluate( string str )
        {
            return Evaluator.Evaluate( str,
                key => myGlobalVars.ContainsKey( key ) ? myGlobalVars[ key ] : null );
        }
    }
}
