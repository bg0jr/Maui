using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Blade.Shell.Forms;

namespace Maui.Shell.Forms
{
    public class ExtensionPointArguments
    {
        [Argument( Short = "-x", Long = "-extension", Description = "name of the assembly containing the extension point(s)" ), Required]
        public string Assembly
        {
            get;
            set;
        }

        /// <summary>
        /// Type containing the impl.
        /// </summary>
        [Argument( Short = "-t", Long = "-type", Description = "contrete type selectino for the extension point" )]
        public string Implementation
        {
            get;
            set;
        }
    }
}
