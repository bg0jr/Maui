using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Blade.Collections;

namespace Maui.Data.Sheets.Csv
{
    /// <summary>
    /// Implements a workbook based on CSV files.
    /// A workbook is always a complete directory. All containing
    /// *.csv files will be treated as worksheets.
    /// <remarks>
    /// Currently only CSV separator ';' supported
    /// </remarks>
    /// </summary>
    public class CsvWorkbook : IWorkbook
    {
        private const string mySeparator = ";";

        private string myDirectory;
        private IList<IWorksheet> myWorksheets;
        private WorkbookConfig myConfig;

        /// <summary/>
        public CsvWorkbook( string directory )
        {
            if ( !Directory.Exists( directory ) )
            {
                throw new FileNotFoundException( "Workbook directory not found", directory );
            }

            myDirectory = directory;

            LoadWorksheets();
        }

        private void LoadWorksheets()
        {
            myWorksheets = Directory.GetFiles( myDirectory, "*.csv" )
               .Select( file => new CsvWorksheet( this, file, mySeparator ) )
               .OfType<IWorksheet>()
               .ToList();
        }

        /// <summary/>
        public IEnumerable<IWorksheet> Worksheets
        {
            get
            {
                return myWorksheets;
            }
        }

        /// <summary/>
        public void Save()
        {
            myWorksheets.Foreach( worksheet => worksheet.Save() );
        }

        /// <summary/>
        public WorkbookConfig Config
        {
            get
            {
                if ( myConfig == null )
                {
                    myConfig = new WorkbookConfig( GetOrCreateConfigSheet() );
                }

                return myConfig;
            }
        }

        private IWorksheet GetOrCreateConfigSheet()
        {
            var configSheet = Worksheets.SingleOrDefault( sheet => sheet.Name == WorkbookConfig.DefaultSheetName );
            if ( configSheet == null )
            {
                var file = Path.Combine( myDirectory, WorkbookConfig.DefaultSheetName + ".csv" );
                File.Create( file );

                configSheet = new CsvWorksheet( this, file, mySeparator );
            }

            return configSheet;
        }
    }
}
