using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Shell;
using System.IO;
using Blade.Shell;

namespace Maui.Presentation.Excel.Scripts
{
    public class ConfigureAddIn : ScriptBase
    {
        protected override void Run()
        {
            Console.WriteLine( "Installing Excel AddIn" );
            
            RegisterExcelAddIn();
            ConfigureExcelForDotNet4();
        }

        private void RegisterExcelAddIn()
        {
            var file = Path.GetTempFileName();
            var drive = this.GetCurrentDrive();
            this.CopyAndPatchResource( "install.reg", file, line => line.Replace( "@DRIVE@", drive ) );

            this.Execute( "reg", "import " + file );
            File.Delete( file );
        }

        private void ConfigureExcelForDotNet4()
        {
            var programFiles = Environment.GetEnvironmentVariable( "ProgramFiles(x86)" );
            this.CopyResource( "Excel.exe.config", Path.Combine( programFiles, "Microsoft Office", "Office12", "Excel.exe.config" ) );
        }
    }
}
