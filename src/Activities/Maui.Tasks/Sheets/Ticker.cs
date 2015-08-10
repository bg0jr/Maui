using System;
using System.Collections.Generic;
using System.Linq;
using Maui.Data.Sheets;
using Maui.Data.Recognition.DatumLocators;
using Maui.Tasks;
using Maui.Entities.Descriptors;

namespace Maui.Tasks.Sheets
{
    /// <summary>
    /// Worksheet for the ticker plugin.
    /// The worksheets expects a header at row 3 and a title of "Maui-Ticker".
    /// The current date will be set at A1 and B1 after each download.
    /// </summary>
    public class Ticker : ISheetTask
    {
        private class Config : ConfigSection
        {
            public Config()
                : base( typeof( Ticker ).Name )
            {
                SetProperty( "SheetName", "Maui-Ticker" );
                SetProperty( "HeaderRow", "3" );
            }

            public string WorksheetName
            {
                get { return GetProperty( "SheetName" ); }
            }

            public int HeaderRow
            {
                get { return int.Parse( GetProperty( "HeaderRow" ) ); }
            }
        }

        private Config myConfig;
        private IWorkbook myWorkbook;
        private IWorksheet myWorksheet;

        /// <summary/>
        public void Open( IWorkbook workbook )
        {
            myWorkbook = workbook;

            ReadConfig();
        }

        private void ReadConfig()
        {
            myConfig = new Config();

            var section = myWorkbook.Config.GetSection( "Ticker" );
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
                yield return myConfig.WorksheetName;
            }
        }

        /// <summary>
        /// Fetches the current prices for the isins provided by the iterator
        /// and stores them in the sheet using the iterator.
        /// Sets the current date into the upper left corner.
        /// </summary>
        public void Calculate()
        {
            ConnectToWorksheet();

            FetchCurrentPrices();

            SetCurrentDate();
        }

        private void ConnectToWorksheet()
        {
            // always reconnect! excel sheet may have changed!

            myWorksheet = myWorkbook.Worksheets.SingleOrDefault( sheet => sheet.Name == myConfig.WorksheetName );
            if ( myWorksheet == null )
            {
                throw new ApplicationException( "Could not find ticker worksheet '" + myConfig.WorksheetName + "'" );
            }
        }

        private void FetchCurrentPrices()
        {
            var iterator = new TickerIterator( myWorksheet, myConfig.HeaderRow );
            iterator.Loop(
                isin => FetchCurrentPrice( isin ),
                price => iterator.SetCurrentPrice( price ) );
        }

        private double FetchCurrentPrice( string isin )
        {
            var stockCreator = new StockCreator();
            var stock = stockCreator.Create( new StockDescriptor( isin, "F" ) );

            var result = DatumLocatorDefinitions.CurrentPrice.FetchSingle<double>( stock );
            return result.Value;
        }

        private void SetCurrentDate()
        {
            myWorksheet.SetCell( "A1", "Data fetched at:" );
            myWorksheet.SetCell( "B1", DateTime.Today );
        }
    }
}
