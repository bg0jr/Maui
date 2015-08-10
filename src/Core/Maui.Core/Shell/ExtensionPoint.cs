using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Blade;
using Maui;
using Maui.Shell.Forms;

namespace Maui.Shell
{
    public class ExtensionPoint
    {
        public ExtensionPoint( Type supportedType )
        {
            SupportedType = supportedType;
        }

        public Type SupportedType
        {
            get;
            private set;
        }

        public Type GetSingleImplementation( string assemblyLocation, string type )
        {
            if ( string.IsNullOrEmpty( assemblyLocation ) )
            {
                throw new ArgumentException( "Assembly not set" );
            }

            var assembly = Assembly.Load( assemblyLocation );

            var implType = GetImplementationType( assembly, type );

            if ( !SupportedType.IsAssignableFrom( implType ) )
            {
                throw new Exception( "Type connected to the extension point does not match expected System.Type" )
                    .AddContext( "ExpectedType", SupportedType )
                    .AddContext( "ConnectedType", implType );
            }

            return implType;
        }

        private Type GetImplementationType( Assembly assembly, string implementationTypeName )
        {
            if ( !string.IsNullOrEmpty( implementationTypeName ) )
            {
                return assembly.GetType( implementationTypeName );
            }

            var implType = assembly.GetTypes()
                .SingleOrDefault( t => SupportedType.IsAssignableFrom( t ) );

            if ( implType == null )
            {
                throw new Exception( "ExtensionPoint not properly configured: no single implementation found" )
                    .AddContext( "Assembly", assembly.FullName )
                    .AddContext( "ExpectedType", SupportedType );
            }

            return implType;
        }
    }
}
