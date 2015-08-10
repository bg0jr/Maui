using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Maui
{
    /// <summary/>
    public class TypeConverter
    {
        private TypeConverter()
        {
        }

        /// <summary/>
        static public string DateToString( DateTime date )
        {
            StringBuilder sb = new StringBuilder();

            sb.Append( date.Year.ToString( CultureInfo.InvariantCulture ) );
            sb.Append( (date.Month < 10 ? "-0" : "-") );
            sb.Append( date.Month );
            sb.Append( (date.Day < 10 ? "-0" : "-") );
            sb.Append( date.Day );

            return sb.ToString();
        }

        /// <summary/>
        static public DateTime StringToDate( string date )
        {
            return DateTime.ParseExact( date, "yyyy-MM-dd", CultureInfo.InvariantCulture );
        }

        /// <summary/>
        static public string DateTimeToString( DateTime dateTime )
        {
            return dateTime.Year + "-" + dateTime.Month + "-" + dateTime.Day + " " +
                dateTime.Hour + ":" + dateTime.Minute + ":" + dateTime.Second;
        }

        /// <summary/>
        static public DateTime StringToDateTime( string dateTime )
        {
            return DateTime.ParseExact( dateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture );
        }

        /// <summary/>
        public static object ConvertTo( string value, Type type )
        {
            if ( type == typeof( Uri ) )
            {
                return new Uri( value );
            }
            else
            {
                return Blade.TypeConverter.ConvertTo( value, type );
            }
        }
    }
}
