using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CSScriptLibrary;
using Blade;
using Blade.Collections;
using Maui.Logging;
using Maui.Shell;
using Blade.Logging;
using Blade.Shell;

namespace Maui.Dynamics
{
    /// <summary>
    /// Loads scripts of different language and type.
    /// </summary>
    // TODO: actually we should have a factory and different plugins for each
    // script type (e.g. c# assembly, c# script, python, ruby, ...)
    public class ScriptLoader
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( Interpreter ) );

        public static string[] StandardScriptUsings = 
            { 
                // System
                "System",
                "System.Collections.Generic",
                "System.Data",
                "System.Linq",
                // Blade
                "Blade",
                "Blade.Data",
                "Blade.Lists",
                // Maui
                "Maui.Common",
                "Maui.Common.Data",
                "Maui.Entities",
                "Maui.Dynamics.Data",
                "Maui.Scripting",
                "Maui.Scripting.Types",
                "Maui.Scripting.Presets",
                "Maui.Scripting.Functions",
                "Maui.Scripting.Functions.Vendor",
                "Maui.Tasks",
                "Maui.Tools.Scripts",
            };

        public static object LoadScript( string script, IList<string> args )
        {
            var ext = Path.GetExtension( script ).ToLower();
            if ( ext == ".dll" || ext == ".cs" || ext == ".exe" )
            {
                Assembly scriptAssembly = null;
                if ( ext == ".cs" )
                {
                    scriptAssembly = CompileCSScript( script );
                }
                else
                {
                    scriptAssembly = Assembly.LoadFrom( script );
                }

                return FindCSharpScript( scriptAssembly, args );
            }
            else if ( ext == ".py" )
            {
                return new PythonScript( script, args );
            }

            throw new Exception( "Cannot determine script engine from script extension: " + ext );
        }

        private static object FindCSharpScript( Assembly scriptAssembly, IList<string> args )
        {
            var mainScripts = scriptAssembly.GetTypes( typeof( IScript ) )
                .ToList();

            if ( mainScripts.Count == 0 )
            {
                throw new ApplicationException( "No script found" );
            }

            Type scriptType = null;
            if ( mainScripts.Count == 1 )
            {
                scriptType = mainScripts.First();
            }
            else
            {
                string scriptNameArg = "no argument available";

                if ( args.Count > 0 )
                {
                    scriptNameArg = args[ 0 ];
                    myLogger.Info( "Taking first script argument as script class: " + scriptNameArg );

                    scriptType = mainScripts.SingleOrDefault( t => t.Name == scriptNameArg );
                    if ( scriptType != null )
                    {
                        args.RemoveAt( 0 );
                    }
                }

                if ( scriptType == null )
                {
                    var ex = new ApplicationException( "More than one script/sheet script found and first script argument does not identify a script class" );
                    ex.Data[ "Available main scripts" ] = mainScripts.Select( t => t.Name ).ToHuman();
                    ex.Data[ "Argument interpreted as script name" ] = scriptNameArg;
                    throw ex;
                }
            }

            return Activator.CreateInstance( scriptType );
        }

        public static IList<Type> FindScripts( Assembly scriptAssembly )
        {
            return scriptAssembly.GetTypes( typeof( IScript ) )
                 .ToList();
        }

        public static bool HasScript( Assembly scriptAssembly, IList<string> args )
        {
            var mainScripts = FindScripts( scriptAssembly );

            if ( mainScripts.Count == 0 )
            {
                // so class implements a script interface -> failed
                return false;
            }

            if ( mainScripts.Count == 1 )
            {
                // exactly one script implements a script interfae -> success
                return true;
            }

            // more than one script class found - lets check for the first argument
            // whether it specifies the script

            if ( args.Count == 0 )
            {
                // no arguments -> failed
                return false;
            }

            var scriptNameArg = args[ 0 ];
            myLogger.Info( "Taking first script argument as script class: " + scriptNameArg );

            var scripts = mainScripts.Where( t => t.Name == scriptNameArg ).ToList();
            if ( scripts.Count == 0 )
            {
                // still no scripts found -> failed
                return false;
            }
            if ( scripts.Count == 1 )
            {
                // exactly one script found -> success
                return true;
            }

            // more than one script found - dont know what to do

            var ex = new ApplicationException( "More than one script/sheet script found and first script argument does not identify a script class" );
            ex.Data[ "Available main scripts" ] = mainScripts.Select( t => t.Name ).ToHuman();
            ex.Data[ "Argument interpreted as script name" ] = scriptNameArg;
            throw ex;
        }

        private static Assembly CompileCSScript( string scriptFile )
        {
            string assemblyFile = Path.GetTempFileName();

            using ( StreamWriter writer = new StreamWriter( assemblyFile ) )
            {
                // write standard usings
                StandardScriptUsings
                    .Select( ns => "using " + ns + ";" )
                    .Foreach<string>( writer.WriteLine );

                // write the script
                using ( StreamReader reader = new StreamReader( scriptFile ) )
                {
                    writer.WriteLine( reader.ReadToEnd() );
                }
            }

            // compile and load the script
            myLogger.Info( "Compiling C# script" );

            CSScript.AssemblyResolvingEnabled = true;
            CSScript.CacheEnabled = true;
            CSScript.GlobalSettings.HideCompilerWarnings = true;
            CSScript.Rethrow = true;

            return CSScript.Load( assemblyFile );
        }
    }
}
