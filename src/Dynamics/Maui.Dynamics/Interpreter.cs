using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Blade;
using Blade.Collections;
using Blade.IO;
using Blade.Logging;
using Blade.Shell;
using Blade.Text;
using Maui.Data.Recognition;
using Maui.Data.Recognition.DatumLocators;
using Maui.Dynamics.Data;
using Maui.Entities;


namespace Maui.Dynamics
{
    /// <summary>
    /// Scripting engine.
    /// Gets scripts and executes them.
    /// Provides access to the outer world (Maui core).
    /// </summary>
    public class Interpreter : ManagedObject, IServiceComponent
    {
        public static IList<Assembly> StandardScriptAssemblies = null;

        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( Interpreter ) );

        private ServiceProvider myServiceProvider = null;

        public void Init( ServiceProvider serviceProvider )
        {
            myServiceProvider = serviceProvider;

            StandardScriptAssemblies = new List<Assembly>()
                {
                    // System
                    typeof( Queryable ).Assembly,       // "System.Core",
                    typeof( DataTable ).Assembly,       // "System.Data",
                    // Blade
                    typeof( TypeExtensions ).Assembly,  // "Blade",
                    // Maui
                    typeof( MauiEnvironment ).Assembly,     // "Maui.Common",
                    typeof( BusinessYear ).Assembly,    // "Maui.Entities",
                    typeof( ITableManager ).Assembly,   // "Maui.Dynamics.Data",
                    typeof( Interpreter ).Assembly,     // "Maui.Scripting",

                    // hack to use helpers in scripts
                    Assembly.LoadFrom(  OS.CombinePaths( MauiEnvironment.Plugins, "Maui.Tasks.dll" ) ),
                    Assembly.LoadFrom(  OS.CombinePaths( MauiEnvironment.Plugins, "Maui.Tools.Scripts.dll" ) )
                };
        }

        public void Start()
        {
            Start( null );
        }

        public void Start( IDictionary<string, string> initialVars )
        {
            Context = new Context( CreateDatumProviderFactory() );

            //globals.db = globals.client.ServiceProviderBE.GetService( "TOM Database" );
            Context.ServiceProvider = myServiceProvider;
            Context.TomScripting = myServiceProvider.TomScripting();

            Context.Scope = new Scope();
            if ( initialVars != null )
            {
                initialVars.Foreach( pair => Context.Scope[ pair.Key ] = pair.Value );
            }
        }

        private IDatumProviderFactory CreateDatumProviderFactory()
        {
            var factory = new DatumProviderFactory(
                Engine.ServiceProvider.Browser(),
                new ScopeLookupPolicy(),
                new ValidRangePolicy( () => Context.Scope.TryFrom, () => Context.Scope.TryTo ) );

            DatumLocatorDefinitions.Defines.Foreach( factory.LocatorRepository.Add );

            var datumLocatorsRoot = Path.Combine( MauiEnvironment.Root, "DatumLocators" );
            factory.LocatorRepository.Load( datumLocatorsRoot );

            return factory;
        }

        public void Run( string script, IList<string> args, Dictionary<string, string> initialVars )
        {
            Start( initialVars );

            var scriptInstance = ScriptLoader.LoadScript( script, args );
            if ( scriptInstance is PythonScript )
            {
                ExecutePythonScript( scriptInstance as PythonScript );
            }
            else
            {
                ExecuteCSharpScript( scriptInstance, args );
            }
        }

        private void ExecuteCSharpScript( object script, IEnumerable<string> scriptArgs )
        {
            if ( script is IScript )
            {
                ((IScript)script).Run( scriptArgs.ToArray() );
            }
            else
            {
                throw new NotSupportedException( "Unsupported script type: " + script.GetType() );
            }
        }

        public static Context Context
        {
            get;
            set;
        }

        public static string ResolveFile( string file )
        {
            if ( Path.IsPathRooted( file ) )
            {
                return file;
            }

            /*
            string f = Path.GetFullPath(
                Path.Combine( Path.GetDirectoryName( Context.Interpreter.Script ), file ) );
            if ( File.Exists( f ) )
            {
                return f;
            }
            */

            return Path.GetFullPath( Path.Combine( Context.MslHome, file ) );
        }

        /// <summary>
        /// Searches for placeholder and replaces them by values of the current context and scope.
        /// </summary>
        public static string Evaluate( string s )
        {
            return Evaluator.Evaluate( s, ScopeLookupPolicy.EvaluateValue );
        }

        public static void Loop( IEnumerable<StockHandle> stocks, Action<StockHandle> body )
        {
            using ( var guard = new NestedScopeGuard() )
            {
                foreach ( var stock in stocks )
                {
                    guard.Scope.Stock = stock;

                    body( stock );
                }
            }
        }

        private void ExecutePythonScript( PythonScript script )
        {
            throw new NotSupportedException( "IronPython is currently not supported" );
        }
    }
}
