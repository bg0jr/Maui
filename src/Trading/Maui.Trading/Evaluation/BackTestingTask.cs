using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Entities;
using Maui.Trading.Model;
using Maui.Trading.Modules;
using Maui.Trading.Reporting;
using System.Threading.Tasks;
using Maui.Trading.Utils;
using Maui.Trading.Indicators;

namespace Maui.Trading.Evaluation
{
    public class BackTestingTask
    {
        private Func<TradingSystem> myTradingSystemBuilder;

        public BackTestingTask( Func<TradingSystem> tradingSystemBuilder, IBroker broker )
        {
            myTradingSystemBuilder = tradingSystemBuilder;
            Broker = broker;
        }

        public IBroker Broker
        {
            get;
            private set;
        }

        public Report Evaluate( IEnumerable<StockHandle> stocks, DateTime dateUnderAnalysis )
        {
            // XXX: system is only created to get the name for the report
            var system = myTradingSystemBuilder();
            var report = new BackTestingReport( system.Name, dateUnderAnalysis );
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
                    var tradingResult = PerformBackTesting( systemResult );

                    lock ( lockObj )
                    {
                        report.AddResult( tradingResult );
                    }
                }
            } );

            return report;
        }

        private TradingResult PerformBackTesting( SystemResult systemResult )
        {
            if ( systemResult.Prices.Empty() || systemResult.Signals.Empty() )
            {
                return null;
            }

            const int InitialCash = 10000;
            var tradingLog = new TradingLog();
            var portfolio = new Portfolio( InitialCash, Broker, tradingLog );

            foreach ( var signal in systemResult.Signals )
            {
                if ( signal.Value.Type == SignalType.Buy )
                {
                    var price = systemResult.Prices[ signal.Time ];
                    portfolio.Buy( signal.Time, price.Value );
                }
                else if ( signal.Value.Type == SignalType.Sell )
                {
                    var price = systemResult.Prices[ signal.Time ];
                    portfolio.Sell( signal.Time, price.Value );
                }
            }

            return new TradingResult( systemResult )
            {
                TradingTimeSpan = systemResult.Prices.Last().Time - systemResult.Prices.First().Time,
                TradingLog = tradingLog,
                InitialCash = InitialCash,
                PortfolioValue = portfolio.GetValue( systemResult.Prices.Last().Value )
            };
        }
    }
}
