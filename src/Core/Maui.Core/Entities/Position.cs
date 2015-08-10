using System;
using Maui;

namespace Maui.Entities
{
    public partial class Position
    {
        // maybe we should add some caching here
        public DateTime OpenDate
        {
            get { return TypeConverter.StringToDate( OpenDateInternal ); }
            set { OpenDateInternal = TypeConverter.DateToString( value ); }
        }

        // maybe we should add some caching here
        public DateTime CloseDate
        {
            get { return TypeConverter.StringToDate( CloseDateInternal ); }
            set { CloseDateInternal = TypeConverter.DateToString( value ); }
        }

    }
}
