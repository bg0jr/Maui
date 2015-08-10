using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Maui.Shell;
using Maui.Tasks;
using Maui.Trading.Evaluation;
using Maui.Trading.Modules;
using Maui.Trading.Reporting;
using Maui.Trading.Reporting.Rendering;
using Maui.Trading.Utils;
using Maui.Shell.Forms;
using Blade.Validation;
using Blade.Shell.Forms;
using Blade.Shell;

namespace Maui.Tools.Scripts
{
    public class StockRanking : ScriptBase
    {
        public StockRanking()
        {
            ReportOutputDirectory = Path.Combine( Path.GetTempPath(), "Maui.Trading.Report" );
            DataStores = new List<string>();
        }

        [UserControl, Required, ValidateObject]
        public StockArguments StockArgs
        {
            get;
            set;
        }

        [Argument( Short = "-out", Description = "report output directory" )]
        public string ReportOutputDirectory
        {
            get;
            set;
        }

        [Argument( Short = "-store", Description = "Path to datastore" ), Required]
        public List<string> DataStores
        {
            get;
            private set;
        }

        [UserControl, Required, ValidateObject]
        public ExtensionPointArguments TradingSystem
        {
            get;
            set;
        }

        protected override void Run()
        {
            PerfMon.Enabled = true;

            var tradingSystemBuilder = CreateTradingSystemBuilder();

            var report = RunStockRankingEvaluation( tradingSystemBuilder );

            RenderReport( report );
        }

        private Func<TradingSystem> CreateTradingSystemBuilder()
        {
            var extensionPoint = new ExtensionPoint( typeof( TradingSystem ) );
            var tradingSystemType = extensionPoint.GetSingleImplementation( TradingSystem.Assembly, TradingSystem.Implementation );

            return () => BuildTradingSystem( tradingSystemType );
        }

        private TradingSystem BuildTradingSystem( Type tradingSystemType )
        {
            var system = (TradingSystem)Activator.CreateInstance( tradingSystemType );

            var container = new DefaultBindingContainer();
            foreach ( var dataStore in DataStores )
            {
                container.DataStore.Add( dataStore );
            }
            container.Bind( system );

            return system;
        }

        private Report RunStockRankingEvaluation( Func<TradingSystem> tradingSystemBuilder )
        {
            using ( PerfMon.Profile( "Performing evaluation" ) )
            {
                var stockListBuilder = new StockListBuilder();
                stockListBuilder.Add( StockArgs.Catalog );

                var task = new StockRankingTask( tradingSystemBuilder );
                var report = task.Evaluate( stockListBuilder.Stocks, DateTime.Today.GetMostRecentTradingDay() );

                return report;
            }
        }

        private void RenderReport( Report report )
        {
            if ( !Directory.Exists( ReportOutputDirectory ) )
            {
                Directory.CreateDirectory( ReportOutputDirectory );
            }

            using ( PerfMon.Profile( "Rendering report" ) )
            {
                var renderer = new HtmlRenderer();
                renderer.Render( report, ReportOutputDirectory );
            }

            Console.WriteLine( "Report written to: {0}", ReportOutputDirectory );
        }
    }
}
