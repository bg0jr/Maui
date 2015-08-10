using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Blade.IO;
using Blade.Data;
using System.Data;
using Blade;

namespace Maui.Data.Sheets.Csv
{
    /// <summary>
    /// CSV based implementation of a worksheet.
    /// </summary>
    public class CsvWorksheet : AbstractDataTableWorksheet
    {
        private string myFile;
        private string mySeparator;
        private DataTable __myTable;
        private CsvWorkbook myWorkbook;

        /// <summary/>
        public CsvWorksheet( CsvWorkbook workbook, string file, char separator )
            : this( workbook, file, separator.ToString() )
        {
        }

        /// <summary/>
        public CsvWorksheet( CsvWorkbook workbook, string file, string separator )
            : base( workbook, Path.GetFileNameWithoutExtension( file ) )
        {
            myWorkbook = workbook;
            myFile = file;
            mySeparator = separator;
        }

        // Dont read the table directly in constructor.
        // This way we would read files everytime we search a worksheet
        // in the workbook. do init lazy.
        protected override DataTable Table
        {
            get
            {
                if ( __myTable == null )
                {
                    __myTable = CsvReader.Read( myFile, mySeparator, false );
                }
                return __myTable;
            }
        }

        /// <summary/>
        public override void Save()
        {
            base.Save();

            using ( var writer = new StreamWriter( myFile ) )
            {
                Table.WriteCsv( writer, mySeparator );
            }
        }
    }
}
