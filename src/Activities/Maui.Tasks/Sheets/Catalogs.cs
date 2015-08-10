using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Blade.Collections;
using Maui.Entities;
using Maui;
using Maui.Data.Sheets;

namespace Maui.Tasks.Sheets
{
    /// <summary>
    /// Sheet to manage catalogs in Maui.
    /// </summary>
    public class Catalogs : ISheetTask
    {
        private class Config : ConfigSection
        {
            public Config()
                : base( typeof( Catalogs ).Name )
            {
                SetProperty( "SheetPrefix", "@" );
                SetProperty( "HeaderRow", "1" );
            }

            public string SheetPrefix
            {
                get { return GetProperty( "SheetPrefix" ); }
            }

            public int HeaderRow
            {
                get { return int.Parse( GetProperty( "HeaderRow" ) ); }
            }
        }

        private class CatalogSheet
        {
            public IWorksheet Worksheet { get; private set; }
            private CatalogIterator myIterator;
            private Config myConfig;

            public CatalogSheet( IWorksheet worksheet, Config config )
            {
                Worksheet = worksheet;
                myConfig = config;

                myIterator = new CatalogIterator( worksheet, myConfig.HeaderRow );
            }

            public bool IsEmpty()
            {
                return Worksheet.GetRowCount() - myConfig.HeaderRow == 0;
            }

            public void Load()
            {
                using ( var trans = new TransactionScope() )
                {
                    var catalog = GetCatalogForWorksheet();

                    var stocks = catalog.TradedStocks.Select( tsh => new StockHandle( tsh ) );
                    myIterator.FillStocks( stocks );

                    trans.Complete();
                }
            }

            public void Store()
            {
                using ( var trans = new TransactionScope() )
                {
                    var catalog = GetCatalogForWorksheet();

                    catalog.TradedStocks.Clear();

                    GetStocksFromWorksheet()
                        .Foreach( stock => catalog.TradedStocks.Add( stock ) );

                    trans.Complete();
                }
            }

            public StockCatalog GetCatalogForWorksheet()
            {
                var catalogName = GetCatalogNameFromWorksheet();
                return FindCatalog( catalogName );
            }

            private static StockCatalog FindCatalog( string catalog )
            {
                using ( var tom = Engine.ServiceProvider.CreateEntityRepository() )
                {
                    return tom.StockCatalogs.SingleOrDefault( c => c.Name == catalog );
                }
            }
            
            public string GetCatalogNameFromWorksheet()
            {
                return Worksheet.Name.Substring( myConfig.SheetPrefix.Length );
            }

            public IEnumerable<TradedStock> GetStocksFromWorksheet()
            {
                foreach ( var isin in myIterator )
                {
                    var stock = StockHandle.GetOrCreate( Isin => isin );
                    yield return stock.TradedStock;
                }
            }
        }

        private Config myConfig;
        private IWorkbook myWorkbook;
        private List<CatalogSheet> myWorksheets;

        /// <summary/>
        public void Open( IWorkbook workbook )
        {
            myWorkbook = workbook;

            ReadConfig();
        }

        private void ReadConfig()
        {
            myConfig = new Config();

            var section = myWorkbook.Config.GetSection( "Catalogs" );
            if ( section != null )
            {
                myConfig.SetProperties( section.Properties );
            }

            // always write back (maybe the section is shared)
            myWorkbook.Config.SetSection( myConfig );
        }

        /// <summary/>
        public void Close()
        {
            // nothing to do
        }

        /// <summary/>
        public IEnumerable<string> ResultSheets
        {
            get
            {
                return myWorkbook.Worksheets
                    .Select( sheet => sheet.Name )
                    .Where( sheetName => sheetName.StartsWith( myConfig.SheetPrefix ) );
            }
        }

        /// <summary>
        /// Fetches the current prices for the isins provided by the iterator
        /// and stores them in the sheet using the iterator.
        /// Sets the current date into the upper left corner.
        /// </summary>
        public void Calculate()
        {
            ConnectToWorksheets();

            UpdateCatalogs();
        }

        private void ConnectToWorksheets()
        {
            // always reconnect! excel sheet may have changed!

            myWorksheets = myWorkbook.Worksheets
                .Where( sheet => sheet.Name.StartsWith( myConfig.SheetPrefix ) )
                .Select( sheet => new CatalogSheet( sheet, myConfig ) )
                .ToList();
        }

        private void UpdateCatalogs()
        {
            foreach ( var sheet in myWorksheets )
            {
                if ( sheet.IsEmpty() )
                {
                    sheet.Load();
                }
                else
                {
                    sheet.Store();
                }
            }
        }
    }
}
