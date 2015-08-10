using System;
using System.IO;
using Maui.Shell;
using Blade.Shell;

namespace Maui.Data.Scripts
{
    public class ConfigureDbUri : ScriptBase
    {
        protected override void Run()
        {
            Console.WriteLine( "Configure Maui database URI" );
            
            var localAppData = Environment.GetEnvironmentVariable( "LOCALAPPDATA" );
            var userConfigRoot = Path.Combine( localAppData, "Maui", "config", "defaults" );
            var userDataConfig = Path.Combine( userConfigRoot, "Maui.Data.xml" );

            var userDatabaseUri = string.Format( "{0}:/Maui.Datawarehouse/Database/maui.db", this.GetCurrentDrive() );
            this.CopyAndPatchResource( "Maui.Data.xml", userDataConfig, line => line.Replace( "@DATABASE.URI@", userDatabaseUri ) );
        }
    }
}
