using System;
using System.Data;

namespace Maui.Dynamics.Data
{
    public static class DataColumnExtensions
    {
        public static bool IsIdColumn( this DataColumn column )
        {
            return column.ColumnName.Equals( "id", StringComparison.OrdinalIgnoreCase );
        }

        public static bool IsOwnerIdColumn( this DataColumn column )
        {
            return ( column.ColumnName.Equals( "traded_stock_id", StringComparison.OrdinalIgnoreCase ) ||
                     column.ColumnName.Equals( "stock_id", StringComparison.OrdinalIgnoreCase ) ||
                     column.ColumnName.Equals( "company_id", StringComparison.OrdinalIgnoreCase ) );
        }

        public static bool IsDateColumn( this DataColumn column )
        {
            return ( column.ColumnName.Equals( "year", StringComparison.OrdinalIgnoreCase ) ||
                     column.ColumnName.Equals( "date", StringComparison.OrdinalIgnoreCase ) );
        }

        public static bool IsOriginColumn( this DataColumn column )
        {
            return column.ColumnName.Equals( "datum_origin_id", StringComparison.OrdinalIgnoreCase );
        }
        
        public static bool IsCurrencyColumn( this DataColumn column )
        {
            return column.ColumnName.Equals( "currency_id", StringComparison.OrdinalIgnoreCase );
        }

        public static bool IsTimestampColumn( this DataColumn column )
        {
            return column.ColumnName.Equals( "timestamp", StringComparison.OrdinalIgnoreCase );
        }
    }
}
