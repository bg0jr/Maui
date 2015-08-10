using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Windows.Markup;
using Blade.Collections;
using Maui.Data.Sheets.Csv;
using Maui.Logging;
using Maui.Shell;
using Maui.Tasks.Sheets;
using Maui.Shell.Forms;
using Blade.Logging;
using Blade.Shell.Forms;
using Blade.Shell;

namespace Maui.Tasks.Shell
{
    [ContentProperty( "Body" )]
    public class Sheet : ActivityBase
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( Sheet ) );

        [Argument( Short = "-f", Long = "-workbook", Description = "Path to the workbook" )]
        [Required]
        public string WorkbookPath
        {
            get;
            set;
        }

        [Required]
        public ISheetTask Body
        {
            get;
            set;
        }

        [Argument( Short = "-h", Long = "--help", Description = "Prints usage information" )]
        public bool Help
        {
            get;
            set;
        }

        protected override void ExecuteInternal( string[] args )
        {
            try
            {
                var form = new Form( this );
                form.Bind( args );

                if ( Help )
                {
                    form.Usage();
                    return;
                }

                form.Validate();
                
                Run();
            }
            catch ( Exception ex )
            {
                myLogger.Error( ex, "Failed to execute {0}", GetType().Name );
            }
        }

        private void Run()
        {
            if ( !Directory.Exists( WorkbookPath ) )
            {
                throw new FileNotFoundException( "No workbook found at: " + WorkbookPath );
            }

            var workbook = new CsvWorkbook( WorkbookPath );

            Body.Open( workbook );

            var resultSheets = new List<CsvWorksheet>();
            if ( Body.ResultSheets != null )
            {
                resultSheets.AddRange( workbook.Worksheets
                    .Where( worksheet => Body.ResultSheets.Contains( worksheet.Name ) )
                    .OfType<CsvWorksheet>() );

                resultSheets.Foreach( worksheet => worksheet.Modified += OnWorksheetModified );
            }

            Body.Calculate();
            Body.Close();

            workbook.Save();

            Console.WriteLine();
            resultSheets.Foreach( worksheet => worksheet.Modified -= OnWorksheetModified );
            resultSheets.Foreach( worksheet => worksheet.Print() );
        }

        void OnWorksheetModified( object sender, EventArgs e )
        {
            Console.Write( "." );
        }
    }
}
