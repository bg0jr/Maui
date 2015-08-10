using System;
using System.Collections.Generic;
using System.Linq;
using Maui.Data.Sheets;
using MSExcel = Microsoft.Office.Interop.Excel;

namespace Maui.Presentation.Excel
{
    /// <summary>
    /// Excel implementation of a workbook.
    /// </summary>
    internal class ExcelWorkbook : IWorkbook
    {
        private MSExcel.Application myApplication;

        /// <summary/>
        public ExcelWorkbook( MSExcel.Application application )
        {
            // dont cache values here! sheets may change!

            myApplication = application;
        }

        /// <summary/>
        public IEnumerable<IWorksheet> Worksheets
        {
            get
            {
                foreach ( MSExcel.Worksheet excelSheet in myApplication.ActiveWorkbook.Worksheets )
                {
                    yield return new ExcelWorksheet( myApplication.ActiveWorkbook, excelSheet );
                }
            }
        }

        /// <summary/>
        public void Save()
        {
            myApplication.ActiveWorkbook.Save();
        }

        /// <summary/>
        public WorkbookConfig Config
        {
            get
            {
                // no cache!!
                return new WorkbookConfig( GetOrCreateConfigSheet() );
            }
        }

        private IWorksheet GetOrCreateConfigSheet()
        {
            var configSheet = Worksheets.SingleOrDefault( sheet => sheet.Name == WorkbookConfig.DefaultSheetName );
            if ( configSheet == null )
            {
                // we will NOT create a new excel sheet here - we do not
                // know whether we are inside a maui workbook.

                configSheet = new InMemoryWorksheet( this , WorkbookConfig.DefaultSheetName);
            }

            return configSheet;
        }

        /// <summary>
        /// Creates a real excel worksheet for the config if not already exists.
        /// </summary>
        public void CreateConfigSheet()
        {
            var configSheet = Worksheets.SingleOrDefault( sheet => sheet.Name == WorkbookConfig.DefaultSheetName );
            if ( configSheet != null )
            {
                return;
            }

            var excelSheet = (MSExcel.Worksheet)myApplication.ActiveWorkbook.Worksheets.Add(
                Type.Missing,
                ( (ExcelWorksheet)Worksheets.Last() ).ExcelSheet,
                Type.Missing,
                Type.Missing );

            excelSheet.Name = WorkbookConfig.DefaultSheetName;
        }
    }
}
