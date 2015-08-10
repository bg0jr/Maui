using System;
using System.Linq;
using Blade;
using Maui.Entities;
using Maui.Logging;
using Maui.Shell;
using Maui.Shell.Forms;
using Blade.Logging;
using Blade.Shell.Forms;
using Blade.Shell;

namespace Maui.Tools.Scripts
{
    public class DBViewer : ScriptBase
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( DBViewer ) );

        [Argument( Short = "-stocks", Description = "List all stocks" )]
        public bool ListStocks
        {
            get;
            private set;
        }

        [Argument( Short = "-catalogs", Description = "List all catalogs" )]
        public bool ListCatalogs
        {
            get;
            private set;
        }

        [Argument( Short = "-list-catalog", Description = "List content of the given catalog-id" )]
        public long? ListCatalogContent
        {
            get;
            private set;
        }

        protected override void Run()
        {
            if ( ListCatalogs )
            {
                using ( var tom = Engine.ServiceProvider.CreateEntityRepository() )
                {
                    foreach ( var catalog in tom.StockCatalogs )
                    {
                        Console.WriteLine( "{0,4} {1}", catalog.Id, catalog.Name );
                    }
                }
            }
            else if ( ListCatalogContent.HasValue )
            {
                using ( var tom = Engine.ServiceProvider.CreateEntityRepository() )
                {
                    var catalog = tom.StockCatalogs.FirstOrDefault( c => c.Id == ListCatalogContent );
                    if ( catalog == null )
                    {
                        throw new Exception( "No such catalog" );
                    }

                    foreach ( var tsh in catalog.TradedStocks )
                    {
                        var stock = new StockHandle( tsh );
                        Console.WriteLine( "{0,4} {1,-30} {2,5} {3,12} {4,10}", stock.TradedStockId,
                            stock.Name.LimitTo( 30 ), stock.Symbol, stock.Isin, stock.Wpkn );
                    }
                }
            }
            else if ( ListStocks )
            {
                ListAllStocks();
            }
            else
            {
                throw new ArgumentException( "Arguments missing" );
            }
        }

        private static void ListAllStocks()
        {
            using ( var entities = Engine.ServiceProvider.CreateEntityRepository() )
            {
                var stocks = entities.TradedStocks
                    .OrderBy( x => x.Stock.Company.Name.ToLower() )
                    .ToList();

                foreach ( var ts in stocks )
                {
                    try
                    {
                        Console.WriteLine( "{0,4} {1,-30} {2,5} {3,12} {4,10} {5,15}", ts.Id,
                            ts.Stock.Company.Name.LimitTo( 30 ), ts.Symbol, ts.Isin, ts.Wpkn, ts.StockExchange.Name );
                    }
                    catch ( Exception ex )
                    {
                        myLogger.Error( "TOM inconsistancy found: {0}", ex.Message );
                    }
                }

                Console.WriteLine();
                Console.WriteLine( "Count: {0}", stocks.Count );
            }
        }

    }
}
