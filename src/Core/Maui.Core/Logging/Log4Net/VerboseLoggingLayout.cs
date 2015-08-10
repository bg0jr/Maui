using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Layout;
using System.IO;
using log4net.Core;
using Maui;

namespace Maui.Logging.Log4Net
{
    public class VerboseLoggingLayout : LayoutSkeleton
    {
        public VerboseLoggingLayout()
        {
            IgnoresException = false;
        }

        public override void ActivateOptions()
        {
            // nothing to do
        }

        public override void Format( TextWriter writer, LoggingEvent loggingEvent )
        {
            if ( loggingEvent == null )
            {
                throw new ArgumentNullException( "loggingEvent" );
            }

            writer.Write( loggingEvent.TimeStamp.ToString( "yyyy-MM-dd HH:mm:ss" ) );
            writer.Write( "|" );
            writer.Write( loggingEvent.Level.DisplayName );
            writer.Write( "|" );
            writer.Write( loggingEvent.ThreadName );
            writer.Write( "|" );
            writer.Write( loggingEvent.LoggerName );
            writer.Write( ": " );
            loggingEvent.WriteRenderedMessage( writer );
            writer.WriteLine();

            if ( loggingEvent.ExceptionObject != null )
            {
                loggingEvent.ExceptionObject.DumpTo( writer );
            }
        }
    }
}
