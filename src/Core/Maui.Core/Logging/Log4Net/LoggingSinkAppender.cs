using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Appender;
using log4net.Core;
using Blade.Logging;

namespace Maui.Logging.Log4Net
{
   internal class LoggingSinkAppender : AppenderSkeleton
    {
        private ILoggingSink mySink;

        public LoggingSinkAppender( ILoggingSink sink )
        {
            mySink = sink;
        }

        protected override void Append( LoggingEvent loggingEvent )
        {
            var level = LogLevelConverter.FromLog4Net(loggingEvent.Level);
            var msg = RenderLoggingEvent( loggingEvent );
            mySink.Write( new LoggingEventAdapter( level, msg ) );
        }
    }
}
