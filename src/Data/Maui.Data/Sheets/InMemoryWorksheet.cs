using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Blade.IO;
using Blade.Data;
using System.Data;
using Blade;

namespace Maui.Data.Sheets
{
    /// <summary>
    /// Worksheet implementation which holds all data completely in memory only.
    /// </summary>
    public class InMemoryWorksheet : AbstractDataTableWorksheet
    {
        private DataTable __myTable;

        /// <summary/>
        public InMemoryWorksheet( IWorkbook workbook, string name )
            : base( workbook, name )
        {
        }

        protected override DataTable Table
        {
            get
            {
                if ( __myTable == null )
                {
                    __myTable = new DataTable();
                }
                return __myTable;
            }
        }
    }
}
