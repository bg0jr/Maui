using System;
using System.Data;

namespace Maui.Data
{
    /// <summary/>
    public static class DataColumnExtensions
    {
        /// <summary/>
        public static bool IsDateColumn( this DataColumn column )
        {
            return ( column.ColumnName.Equals( "year", StringComparison.OrdinalIgnoreCase ) ||
                     column.ColumnName.Equals( "date", StringComparison.OrdinalIgnoreCase ) );
        }
    }
}
