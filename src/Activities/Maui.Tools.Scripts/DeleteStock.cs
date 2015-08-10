using System;
using System.Collections.Generic;
using System.Linq;
using Maui.Entities;
using Maui.Entities.Descriptors;
using Maui.Logging;
using Maui.Shell;
using Maui.Tasks;
using Maui.Shell.Forms;
using Blade.Validation;
using System.ComponentModel.DataAnnotations;
using Blade.Logging;
using Blade.Shell.Forms;
using Blade.Shell;

namespace Maui.Tools.Scripts
{
    public class DeleteStock : ScriptBase
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( DBViewer ) );

        [UserControl, Required, ValidateObject]
        public StockArguments StockArgs
        {
            get;
            set;
        }

        protected override void Run()
        {
            var creator = new StockCreator();

            using ( var tom = Engine.ServiceProvider.CreateEntityRepository() )
            {
                var stocksToDelete = StockArgs.Stocks
                    .SelectMany( stockDesc => tom.Stocks.Where( s => s.Isin == stockDesc.Isin ).ToList() )
                    .ToList();

                if ( !stocksToDelete.Any() )
                {
                    Console.WriteLine( "no stocks found to delete" );
                    return;
                }

                Console.WriteLine( "We are going to remove the following stocks: " );
                foreach ( var stock in stocksToDelete )
                {
                    Console.WriteLine( "  Name: {0,-40} Isin: {1}", stock.Company.Name, stock.Isin );
                }
                Console.Write( "Are you sure [y/n]:" );
                var answer = Console.ReadLine();

                if ( !answer.Equals( "y", StringComparison.OrdinalIgnoreCase ) )
                {
                    Console.WriteLine( "aborted" );
                    return;
                }

                foreach ( var stock in stocksToDelete )
                {
                    // TODO: actually we should have a cascading delete
                    foreach ( var ts in stock.TradedStocks.ToList() )
                    {
                        tom.DeleteObject( ts );
                    }
                    tom.DeleteObject( stock );
                }

                tom.SaveChanges();
            }
        }
    }
}
