using System;
using Blade;
using Blade.IO;
using Maui;
using Maui.Logging;
using System.IO;
using Blade.Logging;
using System.Collections.Generic;

namespace Maui
{
    internal sealed class Step_010_Logging : AbstractEngineInitializer, IBannerInfoProvider
    {
        public override void Init()
        {
            if ( !MauiEnvironment.InsideTest )
            {
                // XXX: if there are already loggers then we have already configured 
                //      logging (e.g. from integration test)
                LoggerFactory.LoadConfiguration( new Uri( GetType().GetAssemblyConfig() ) );
            }
        }

        public KeyValuePair<string, string> GetBannerInformation()
        {
            return new KeyValuePair<string, string>( "Log", Environment.ExpandEnvironmentVariables( @"%TEMP%\Maui.log" ) );
        }
    }

    internal sealed class Step_020_Configuration : AbstractEngineInitializer
    {
        public override void Init()
        {
            ImportConfigs( OS.CombinePaths( MauiEnvironment.Config, "defaults" ) );
            ImportConfigs( OS.CombinePaths( Environment.GetEnvironmentVariable( "LOCALAPPDATA" ), "maui", "config", "defaults" ) );

            if ( Engine.MauiApplication != null )
            {
                ImportConfigs( OS.CombinePaths( MauiEnvironment.Config, Engine.MauiApplication ) );
                ImportConfigs( OS.CombinePaths( Environment.GetEnvironmentVariable( "LOCALAPPDATA" ), "maui", "config", Engine.MauiApplication ) );
            }
        }

        private void ImportConfigs( string path )
        {
            if ( Directory.Exists( path ) )
            {
                Engine.ServiceProvider.ConfigurationSC().Import( path, true );
            }
        }
    }
}
