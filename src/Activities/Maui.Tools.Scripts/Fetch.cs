using System;
using Blade.Data;
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
    /// Generic data fetcher.
    /// </summary>
    public class Fetch : MslScriptBase
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( Fetch ) );

        [IsinArgument,Required]
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

            Console.WriteLine( "Fetching " + stock.ToString() );

            var table = this.Fetch( stock, Datum );

            Console.WriteLine();
            table.Dump();
        }
    }
}

