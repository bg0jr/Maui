using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Xsl;
using Blade.Collections;
using Blade.Text;
using Maui.Entities;
using Maui.Dynamics.Types;
using Maui.Logging;
using Maui.Trading.GT;
using Maui.Trading.GT.BackTesting;
using Maui.Dynamics;
using Blade.Logging;

namespace Maui.Tasks.Dynamics
{
    public static class RunFunction
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( RunFunction ) );

        public static void RunModel( this IMslScript script, Action model )
        {
            Interpreter.Context.Scope.Catalog.ForEach( model );
        }

        public static void RunBacktest( this IMslScript script, AbstractSystem systemName )
        {
            var settings = new BackTestSettings();
            settings.System = systemName;

            Interpreter.Context.Scope.Catalog.ForEach( () =>
                {
                    settings.TradedStock = Interpreter.Context.Scope.Stock.TradedStock;
                    Interpreter.Context.ServiceProvider.TradingSC().DoBackTest( settings );
                } );
        }

        public static void RunReport( this IMslScript script, string embeddedTemplate, string output, Layout layout )
        {
            RunReport( script, embeddedTemplate, output, layout, false );
        }

        public static void RunReport( this IMslScript script, string embeddedTemplate, string output, Layout layout, bool loopCatalog )
        {
            var assembly = script != null ? script.GetType().Assembly : null;
            Action Runner = () => RunReport( embeddedTemplate, assembly, output, layout, loopCatalog );

            if ( loopCatalog )
            {
                // so the report has to be executed for each catalog
                Interpreter.Context.Scope.Catalog.ForEach( Runner );
            }
            else
            {
                Runner();
            }
        }

        public static void RunReport( this IMslScript script, object args )
        {
            script.RunReport(
                GetArg<string>( args, "Template" ),
                GetArg<string>( args, "Output" ),
                GetArg<Layout>( args, "Layout" ),
                GetArg<bool>( args, "LoopCatalog" )
                );
        }

        private static T GetArg<T>( object args, string name )
        {
            var prop = args.GetType().GetProperty( name );
            if ( prop == null )
            {
                return default( T );
            }

            return (T)prop.GetValue( args, null );
        }

        private static void RunReport( string embeddedTemplate, Assembly assembly, string output, Layout layout, bool loopCatalog )
        {
            var tmpFile = Path.GetTempFileName();
            var outFile = GenerateOutputFile( output, loopCatalog );
            var debuggingFile = Path.GetTempFileName();

            try
            {
                Template template = (Path.IsPathRooted( embeddedTemplate ) ? new Template( embeddedTemplate, debuggingFile ) : new Template( embeddedTemplate, assembly, debuggingFile ));
                template.BaseClass = typeof( ReportBase ).ToString();

                // add some standard references and usings
                Interpreter.StandardScriptAssemblies
                    .Select( asm => Path.GetFileName( asm.GetName().CodeBase ) )
                    .Foreach( template.Assemblies.Add );
                ScriptLoader.StandardScriptUsings.Foreach( template.Usings.Add );

                template.Generate( null, tmpFile );

                // now transform the report into the specified layout
                TextWriter writer = null;
                if ( outFile == null )
                {
                    writer = Console.Out;
                }
                else
                {
                    var dir = Path.GetDirectoryName( outFile );
                    if ( !Directory.Exists( dir ) )
                    {
                        Directory.CreateDirectory( dir );
                    }

                    writer = new StreamWriter( outFile );
                }

                var doc = XDocument.Load( tmpFile );
                if ( layout == null )
                {
                    doc.Save( writer );
                }
                else
                {
                    XslCompiledTransform transformer = new XslCompiledTransform();
                    transformer.Load( layout.Stylesheet );
                    transformer.Transform( doc.CreateReader(), null, writer );
                }

                if ( outFile != null )
                {
                    writer.Close();
                }
            }
            catch ( Exception ex )
            {
                myLogger.Error( ex, "Failed to execute report. Un-compiled report: {0}", debuggingFile );
            }
            finally
            {
                File.Delete( tmpFile );
            }
        }

        private static string GenerateOutputFile( string output, bool loopCatalog )
        {
            if ( !loopCatalog || output == null )
            {
                return output;
            }

            // XXX: hack to write to different out-files

            StockHandle stock = Interpreter.Context.Scope.Stock;

            var outFile = Path.Combine( Path.GetDirectoryName( output ), Path.GetFileNameWithoutExtension( output ) );
            outFile += "-";
            outFile += (string.IsNullOrEmpty( stock.TradedStock.Symbol ) ? stock.Stock.Isin : stock.TradedStock.Symbol);
            outFile += Path.GetExtension( output );

            return outFile;
        }
    }
}
