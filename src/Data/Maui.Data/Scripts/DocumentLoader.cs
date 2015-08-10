using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blade.Shell;
using Maui.Data.Recognition.Spec;
using Blade.Shell.Forms;

namespace Maui.Data.Scripts
{
    public class DocumentLoader : ScriptBase
    {
        [ConfigFileArgument( Short = "-f", Description = "Document to load" )]
        public Document Document
        {
            get;
            set;
        }

        protected override void Run()
        {
            if ( Document == null )
            {
                Console.WriteLine( "NO document loaded" );
            }
            else
            {
                Console.WriteLine( "DONE" );
            }
        }
    }
}
