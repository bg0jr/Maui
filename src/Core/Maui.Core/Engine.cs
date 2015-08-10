using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Blade;
using Blade.IO;
using Maui;
using Maui.Logging;
using Blade.Logging;

namespace Maui
{
    public enum EngineStatus
    {
        Stopped,
        Booted,
        Running,
    }

    public static class Engine
    {
        private static ILogger myLogger = LoggerFactory.GetLogger( typeof( Engine ) );
        private static Stack<AbstractEngineInitializer> myInitializers = new Stack<AbstractEngineInitializer>();

        public static ServiceProvider ServiceProvider { get; private set; }
        public static EngineStatus Status { get; private set; }
        public static string MauiApplication { get; private set; }

        static Engine()
        {
            Status = EngineStatus.Stopped;

            // The service provider is always there so that 
            // it can be mocked during unit tests.
            ServiceProvider = new ServiceProvider();
        }

        public static void Boot( string application )
        {
            if ( Status != EngineStatus.Stopped )
            {
                throw new InvalidOperationException( "Engine cannot be booted. State: " + Status );
            }

            MauiApplication = application;

            // Launch the debugger?
            if ( Environment.GetEnvironmentVariable( "MAUIDBG" ).IsTrue() ||
                Environment.GetEnvironmentVariable( "MAUI_DBG" ).IsTrue() )
            {
                Debugger.Launch();
            }

            // get all assemblies already loaded
            var initAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where( a => a.FullName.StartsWith( "Maui." ) ).ToList();

            AppDomain sandbox = AppDomain.CreateDomain( "sandbox" );
            Action<string> LoadAssembly = file =>
                {
                    var newAssembly = sandbox.Load( AssemblyName.GetAssemblyName( file ) );
                    if ( !initAssemblies.Any( a => a.GetName().FullName == newAssembly.GetName().FullName ) )
                    {
                        initAssemblies.Add( Assembly.LoadFrom( file ) );
                    }
                };

            // the other framework services
            var dllDir = OS.CombinePaths( MauiEnvironment.Plugins );
            foreach ( var file in Directory.GetFiles( dllDir, "Maui.*.dll" ) )
            {
                LoadAssembly( file );
            }

            if ( initAssemblies.Count == 0 )
            {
                throw new Exception( "Could not find maui core initializers. There must be s.th. wrong in the environment" );
            }

            // XXX: normal System.Type comparison does not work in ResolverOne
            var initTypes = initAssemblies.SelectMany( a => a.GetTypes() )
                .Where( t => t.BaseType == typeof( AbstractEngineInitializer ) )
                .OrderBy( t => t.Name );

            foreach ( var t in initTypes )
            {
                string desc = string.Join( " ", t.FullName.Split( '_' ).Skip( 2 ).ToArray() );

                try
                {
                    myLogger.Info( "Initializing " + desc );
                    //Console.WriteLine( "[Initializing " + desc + "]" );

                    var initObj = (AbstractEngineInitializer)Activator.CreateInstance( t );
                    initObj.Init();

                    myInitializers.Push( initObj );
                }
                catch ( Exception ex )
                {
                    string msg = "Failed to init step: " + desc;

                    myLogger.Error( ex, msg );

                    throw new Exception( msg, ex );
                }
            }

            // update logger because until now (logging got initialized) we might have worked with the
            // "fallback" logger
            myLogger = LoggerFactory.GetLogger( typeof( Engine ) );
            myLogger.Info( "=== Engine booted ===" );

            Status = EngineStatus.Booted;
        }

        internal static IEnumerable<object> Initializers
        {
            get
            {
                return myInitializers;
            }
        }

        public static void Run()
        {
            if ( Status != EngineStatus.Booted )
            {
                throw new InvalidOperationException( "Engine cannot be run. State: " + Status );
            }

            Status = EngineStatus.Running;
        }

        public static void Shutdown()
        {
            // no check for status - always try to shutdown whatever is running

            // cleanup initializers
            while ( myInitializers.Count > 0 )
            {
                var initObj = myInitializers.Pop();

                try
                {
                    initObj.Fini();
                }
                catch ( Exception ex )
                {
                    string desc = string.Join( " ", initObj.GetType().FullName.Split( '_' ).Skip( 2 ).ToArray() );
                    string msg = "Failed to finalize step: " + desc;

                    myLogger.Error( ex, msg );

                    // try to deinit the rest
                    //throw new Exception( msg, ex );
                }
            }

            // cleanup serviceproviders
            ServiceProvider.Reset();

            Status = EngineStatus.Stopped;
        }

        /// <summary>
        /// Very basic process callback.
        /// </summary>
        public static void LogProgress( string fmt, params object[] args )
        {
            Console.WriteLine( fmt, args );
        }
    }
}
