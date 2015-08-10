using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Blade;
using Blade.Collections;
using Maui.Logging;
using Maui.Tasks.Sheets;
using Blade.Logging;

namespace Maui.Tasks
{
    public class SheetTaskLoader
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( SheetTaskLoader ) );

        public static ISheetTask LoadSheetTask( string script, IList<string> args )
        {
            var ext = Path.GetExtension( script ).ToLower();
            if ( ext == ".dll" )
            {
                var assembly = Assembly.LoadFrom( script );

                return FindSheetTask( assembly, args );
            }

            throw new Exception( "Cannot determine script engine from script extension: " + ext );
        }

        private static ISheetTask FindSheetTask( Assembly scriptAssembly, IList<string> args )
        {
            var sheets = scriptAssembly.GetTypes( typeof( ISheetTask ) ).ToList();

            if ( sheets.Count == 0 )
            {
                throw new ApplicationException( "No script found" );
            }

            Type sheetTaskType = null;
            if ( sheets.Count == 1 )
            {
                sheetTaskType = sheets.First();
            }
            else
            {
                string scriptNameArg = "no argument available";

                if ( args.Count > 0 )
                {
                    scriptNameArg = args[ 0 ];
                    myLogger.Info( "Taking first script argument as script class: " + scriptNameArg );

                    sheetTaskType = sheets.SingleOrDefault( t => t.Name == scriptNameArg );
                    if ( sheetTaskType != null )
                    {
                        args.RemoveAt( 0 );
                    }
                }

                if ( sheetTaskType == null )
                {
                    var ex = new ApplicationException( "More than one script/sheet script found and first script argument does not identify a script class" );
                    ex.Data[ "Available main scripts" ] = sheets.Select( t => t.Name ).ToHuman();
                    ex.Data[ "Argument interpreted as script name" ] = scriptNameArg;
                    throw ex;
                }
            }

            var sheetTask = (ISheetTask)Activator.CreateInstance( sheetTaskType );
            return sheetTask;
        }

        public static IList<Type> FindSheetTasks( Assembly scriptAssembly )
        {
            return scriptAssembly.GetTypes( typeof( ISheetTask ) )
                 .ToList();
        }

        public static bool HasSheetTask( Assembly scriptAssembly, IList<string> args )
        {
            var sheetTasks = FindSheetTasks( scriptAssembly );

            if ( sheetTasks.Count == 0 )
            {
                // so class implements a script interface -> failed
                return false;
            }

            if ( sheetTasks.Count == 1 )
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

            var sheetTaskName = args[ 0 ];
            myLogger.Info( "Taking first argument as sheet task class: " + sheetTaskName );

            var scripts = sheetTasks.Where( t => t.Name == sheetTaskName ).ToList();
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

            var ex = new ApplicationException( "More than one sheet tasks found and first argument does not identify a sheet task class" );
            ex.Data[ "Available sheet tasks" ] = sheetTasks.Select( t => t.Name ).ToHuman();
            ex.Data[ "Argument interpreted as sheet task name" ] = sheetTaskName;
            throw ex;
        }
    }
}
