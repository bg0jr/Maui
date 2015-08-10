using System;
using Maui.Data.Recognition;
using Maui.Data.Recognition.Spec;

namespace Maui.Dynamics
{
    public static class FormatColumnExtensions
    {
        public static bool IsDateColumn( this FormatColumn column )
        {
            return ( column.Name.Equals( "year", StringComparison.OrdinalIgnoreCase ) ||
                     column.Name.Equals( "date", StringComparison.OrdinalIgnoreCase ) );
        }
    }
}
