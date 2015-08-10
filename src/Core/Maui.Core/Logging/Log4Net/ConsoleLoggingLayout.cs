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
    public class ConsoleLoggingLayout : LayoutSkeleton
    {
        public ConsoleLoggingLayout()
        {
            IgnoresException = false;
        }

        public override void ActivateOptions()
        {
            // nothing to do
        }

        public override void Format(TextWriter writer, LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
            {
                throw new ArgumentNullException("loggingEvent");
            }

            loggingEvent.WriteRenderedMessage(writer);
            writer.WriteLine();
            if ( loggingEvent.ExceptionObject != null )
            {
                loggingEvent.ExceptionObject.DumpTo( writer );
            }
        }
    }
}
