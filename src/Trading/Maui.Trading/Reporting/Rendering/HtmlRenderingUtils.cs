using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Maui.Trading.Indicators;
using Maui.Trading.Model;
using Maui.Entities;

namespace Maui.Trading.Reporting.Rendering
{
    public class HtmlRenderingUtils
    {
        public static void RenderDictionary( IDictionary<string, object> entries, TextWriter document )
        {
            document.WriteLine( "<table>" );

            foreach ( var entry in entries )
            {
                document.WriteLine( "<tr><td>{0}</td><td>:</td><td>{1}</td></tr>", entry.Key, GetDisplayText( entry.Value ) );
            }

            document.WriteLine( "</table>" );
        }

        public static string GetDisplayText( object obj )
        {
            if ( obj == null )
            {
                return "-";
            }
            else if ( obj is StockHandle )
            {
                var stock = (StockHandle)obj;
                return string.Format( "{0}, {1}", stock.Name, stock.Isin );
            }
            else if ( obj is DateTime )
            {
                return ( (DateTime)obj ).ToString( "yyyy-MM-dd" );
            }
            else if ( typeof( TimedValue<DateTime, Signal> ).IsAssignableFrom( obj.GetType() ) )
            {
                var timedSignal = (TimedValue<DateTime, Signal>)obj;
                var s = string.Format( "{0} {1}", timedSignal.Time.ToString( "yyyy-MM-dd" ), GetDisplayText( timedSignal.Value ) );
                return s;
            }
            else if ( obj is Signal )
            {
                return GetDisplayText( (Signal)obj );
            }
            else if ( obj is IEnumerable<TimedValue<DateTime, Signal>> )
            {
                var signals = ( (IEnumerable<TimedValue<DateTime, Signal>>)obj )
                    .Select( timedSignal => GetDisplayText( timedSignal ) )
                    .ToArray();

                return string.Join( ",", signals );
            }
            else if ( obj is double? )
            {
                return GetDisplayText( (double?)obj );
            }
            else if ( obj is double )
            {
                return GetDisplayText( (double)obj );
            }
            else
            {
                return obj.ToString();
            }
        }

        public static string GetDisplayText( Signal signal )
        {
            if ( signal.Type == SignalType.None )
            {
                return "none";
            }

            var formatedType = FormatSignalType( signal );

            if ( signal.Quality.Value == signal.Quality.Max )
            {
                return string.Format( "{0} ({1}%)", formatedType, signal.Strength.Value );
            }
            else
            {
                return string.Format( "{0} (S={1}%, Q={2}%)",
                    formatedType, signal.Strength.Value, signal.Quality.Value );
            }
        }

        private static string FormatSignalType( Signal signal )
        {
            if ( signal.Type == SignalType.Buy )
            {
                return "<span style='color:green;'>buy</span>";
            }
            else if ( signal.Type == SignalType.Sell )
            {
                return "<span style='color:red;'>sell</span>";
            }
            else if ( signal.Type == SignalType.Neutral )
            {
                return "neutral";
            }
            else if ( signal.Type == SignalType.None )
            {
                return "none";
            }
            else
            {
                throw new NotSupportedException( "Unknown signal type: " + signal.GetType() );
            }
        }

        public static string GetDisplayText( double? value )
        {
            return value.HasValue ? value.Value.ToString( "#0.00" ) : "-";
        }

        public static string GetDisplayText( TimedValue<DateTime,Signal> signal )
        {
            return string.Format( "{0}: {1}", signal.Time.ToString( "yyyy-MM-dd" ), GetDisplayText( signal.Value ) );
        }
    }
}
