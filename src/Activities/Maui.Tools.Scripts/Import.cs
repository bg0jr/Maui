using System;
using Maui.Dynamics.Shell;
using Maui.Entities;
using Maui.Logging;
using Maui.Shell;
using Maui.Tasks.Dynamics;
using Maui.Shell.Forms;
using System.ComponentModel.DataAnnotations;
using Blade.Logging;
using Blade.Shell.Forms;

namespace Maui.Tools.Scripts
{
    /// <summary>
    /// General purpose importer
    /// </summary>
    public class Import : MslScriptBase
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( Import ) );

        [IsinArgument, Required]
        public string Isin
        {
            get;
            set;
        }

        [Argument( Short = "-datum", Description = "Datum to fetch" ), Required]
        public string Datum
        {
            get;
            set;
        }

        protected override void Interpret()
        {
            var stock = StockHandle.GetOrCreate( Isin => this.Isin );

            Console.WriteLine( "Importing " + stock.ToString() );

            this.Import( stock, Datum, true );
        }
    }
}

