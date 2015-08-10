using System;
using System.Collections.Generic;
using System.IO;
using Blade.Collections;
using Maui.Data.Sheets;
using Maui;
using Maui.Presentation.Excel.Plugins;
using Maui.Tasks;
using Maui.Tasks.Sheets;
using MSExcel = Microsoft.Office.Interop.Excel;

namespace Maui.Presentation.Excel
{
    /// <summary>
    /// Manages activation and deactivation of plugins.
    /// </summary>
    public class PluginManager
    {
        private MSExcel.Application myApplication;
        private PluginContext myPluginContext;
        private IList<AbstractPlugin> myPlugins;

        /// <summary/>
        public PluginManager( MSExcel.Application application )
        {
            myApplication = application;

            myPluginContext = new PluginContext( myApplication );
            myPlugins = new List<AbstractPlugin>();
        }

        /// <summary/>
        public IEnumerable<AbstractPlugin> Plugins
        {
            get { return myPlugins; }
        }

        /// <summary/>
        public bool IsActivated
        {
            get
            {
                // for simplicity we just treat "no plugins" as "not yet activated"
                return myPlugins.Count > 0;
            }
        }

        /// <summary/>
        public void Activate()
        {
            var config = myPluginContext.ActiveWorkbook.Config.GetSection( "ExcelPlugins" );
            if ( config == null )
            {
                config = new ConfigSection( "ExcelPlugins" );
                myPluginContext.ActiveWorkbook.Config.SetSection( config );

                return;
            }

            foreach ( var pair in config.Properties )
            {
                var sheetName = pair.Key;
                var sheetFile = pair.Value;

                var sheet = LoadSheetTask( sheetName, sheetFile );

                CreateAndRegisterPlugin( sheet );
            }
        }

        private ISheetTask LoadSheetTask( string sheetName, string sheetFile )
        {
            if ( !Path.IsPathRooted( sheetFile ) )
            {
                sheetFile = Path.Combine( MauiEnvironment.Root, sheetFile );
            }

            object script = null;
            if ( Path.GetExtension( sheetFile ) == ".dll" )
            {
                // sheet name needed to identify class
                script = SheetTaskLoader.LoadSheetTask( sheetFile, new List<string>() { sheetName } );
            }
            else
            {
                // no args to provide
                script = SheetTaskLoader.LoadSheetTask( sheetFile, new List<string>() );
            }

            if ( script is ISheetTask )
            {
                return (ISheetTask)script;
            }

            var exception = new ApplicationException( "Unknown script type" );
            exception.Data[ "SupportedScriptTypes" ] = typeof( ISheetTask );
            exception.Data[ "DetectedScriptType" ] = script.GetType();
            throw exception;
        }

        private void CreateAndRegisterPlugin( ISheetTask sheet )
        {
            var config = GetOrCreatePluginConfig( sheet.GetType().Name );

            var plugin = new GenericScriptSheetPlugin( sheet,
                config.GetProperty( "Excel.Caption" ),
                int.Parse( config.GetProperty( "Excel.FaceId" ) ) );

            plugin.Open( myPluginContext );

            myPlugins.Add( plugin );
        }

        private ConfigSection GetOrCreatePluginConfig( string pluginName )
        {
            var config = myPluginContext.ActiveWorkbook.Config.GetSection( pluginName );
            if ( config == null )
            {
                config = new ConfigSection( pluginName );
                config.SetProperty( "Excel.Caption", pluginName );
                config.SetProperty( "Excel.FaceId", 16.ToString() );

                myPluginContext.ActiveWorkbook.Config.SetSection( config );
            }

            return config;
        }

        /// <summary/>
        public void Deactivate()
        {
            myPlugins.Foreach( plugin => plugin.Close() );
            myPlugins.Clear();
        }
    }
}
