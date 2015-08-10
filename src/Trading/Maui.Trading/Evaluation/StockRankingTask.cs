using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Reporting;
using Maui.Trading.Model;
using Maui.Trading.Modules;
using Maui.Entities;
using Maui.Trading.Utils;
using System.Threading.Tasks;

namespace Maui.Trading.Evaluation
{
    // Ranking of the stocks IS the report of that task
    // - ranking: first by signal and if same signal then by expected gain
    // - first level of details: signal of each indicator
    // - second level of details: detailed report of each indicator
    public class StockRankingTask
    {
        private Func<TradingSystem> myTradingSystemBuilder;

        public StockRankingTask( Func<TradingSystem> tradingSystemBuilder )
        {
            myTradingSystemBuilder = tradingSystemBuilder;
        }

        public Report Evaluate( IEnumerable<StockHandle> stocks, DateTime dateUnderAnalysis )
        {
            // XXX: system is only created to get the name for the report
            var system = myTradingSystemBuilder();
            var report = new StockRankingReport( system.Name, dateUnderAnalysis );
            var lockObj = new object();

            Parallel.ForEach( stocks, stock =>
            {
                using ( PerfMon.Profile( "Evaluating stock: {0}", stock.Isin ) )
                {
                    lock ( lockObj )
                    {
                        system = myTradingSystemBuilder();
                    }

                    var systemResult = system.Evaluate( stock, dateUnderAnalysis );

                    lock ( lockObj )
                    {
                        report.AddResult( systemResult );
                    }
                }
            } );

            return report;
        }
    }
}
