using System.Data;
using System;
using Blade;
using Maui;

namespace Maui.Dynamics.Data
{
    public static class DataRowExtensions
    {
        /// <summary>
        /// Compares the content of two rows which belong to the same table.
        /// </summary>
        public static bool ContentEquals( this DataRow lhs, DataRow rhs )
        {
            if ( lhs.Table != rhs.Table )
            {
                return false;
            }

            for ( int i = 0; i < lhs.ItemArray.Length; ++i )
            {
                if ( !lhs[ i ].Equals( rhs[ i ] ) )
                {
                    return false;
                }
            }
            return true;
        }

        public static string GetDateString( this DataRow row, TableSchema schema )
        {
            return row[ schema.DateColumn ].ToString();
        }

        public static DateTime GetDate( this DataRow row, TableSchema schema )
        {
            return GetDate( row, schema.DateColumn );
        }

        public static void SetDate( this DataRow row, TableSchema schema, DateTime value )
        {
            row.SetDate( schema.DateColumn, value );
        }

        #region Basic date handling

        /// <summary>
        /// If "year" always returns first day of this year.
        /// </summary>
        public static DateTime GetDate( this DataRow row, string column )
        {
            if ( row[ column ] is DateTime )
            {
                return (DateTime)row[column];
            }

            if ( column.Equals( "year", StringComparison.OrdinalIgnoreCase ) )
            {
                return Blade.DateTimeExtensions.FirstOfYear( Convert.ToInt32( row[ column ] ) );
            }
            else
            {
                return Maui.TypeConverter.StringToDate( row[ column ].ToString() );
            }
        }

        public static void SetDate( this DataRow row, string column, DateTime value )
        {
            if ( column.Equals( "year", StringComparison.OrdinalIgnoreCase ) )
            {
                row[ column ] = value.Year;
            }
            else
            {
                row[ column ] = Maui.TypeConverter.DateToString( value );
            }
        }

        public static DateTime GetDateTime( this DataRow row, string column )
        {
            return Maui.TypeConverter.StringToDateTime( row[ column ].ToString() );
        }

        public static void SetDateTime( this DataRow row, string column, DateTime value )
        {
            row[ column ] = Maui.TypeConverter.DateTimeToString( value );
        }
        #endregion
    }
}
