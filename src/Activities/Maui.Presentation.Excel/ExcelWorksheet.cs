using System;
using Maui.Data.Sheets;
using MSExcel = Microsoft.Office.Interop.Excel;

namespace Maui.Presentation.Excel
{
    internal class ExcelWorksheet : IWorksheet
    {
        private MSExcel.Workbook myWorkbook;

        public ExcelWorksheet( MSExcel.Workbook workbook, MSExcel.Worksheet sheet )
        {
            myWorkbook = workbook;
            ExcelSheet = sheet;
        }

        internal MSExcel.Worksheet ExcelSheet { get; private set; }

        public string Name
        {
            get { return ExcelSheet.Name; }
        }

        public object GetCell( string position )
        {
            return ExcelSheet.get_Range( position, Type.Missing ).Value2;
        }

        public T GetCell<T>( string position )
        {
            return (T)GetCell( position );
        }

        public void SetCell( string position, object value )
        {
            ExcelSheet.get_Range( position, Type.Missing ).Value2 = value;
        }

        public void Save()
        {
            myWorkbook.Save();
        }

        public bool IsEmptyCell( object cell )
        {
            return cell == null;
        }

        public int GetRowCount()
        {
            // excel always have 65535 rows - that creates performance problems
            // so we define the end of the sheet as 
            // "the beginning of a block of 10 empty lines"
            int numEmptyRows = 0;
            for ( int row = 1; row < ExcelSheet.Rows.Count; ++row )
            {
                if ( IsEmptyRow( row ) )
                {
                    numEmptyRows++;
                }
                else
                {
                    numEmptyRows = 0;
                }

                if ( numEmptyRows >= 10 )
                {
                    return row - numEmptyRows;
                }
            }

            // fallback if the algorithm above fails
            return ExcelSheet.Rows.Count;
        }

        // a row is interpreted as "empty" if the first 26 colums are empty
        private bool IsEmptyRow( int row )
        {
            for ( char col = 'A'; col <= 'Z'; col++ )
            {
                if ( !IsEmptyCell( GetCell( col.ToString() + row ) ) )
                {
                    return false;
                }
            }

            return true;
        }

        public int GetColumnCount()
        {
            // excel always have 256 columns - until we get performance
            // problems we will return all here
            return ExcelSheet.Columns.Count;
        }
    }
}
