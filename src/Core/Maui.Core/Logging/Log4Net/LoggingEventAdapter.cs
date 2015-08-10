using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Core;
using Blade.Logging;

namespace Maui.Logging.Log4Net
{
    internal class LoggingEventAdapter : ILoggingEntry
    {
        public LoggingEventAdapter( LogLevel level, string msg )
        {
            Level = level;
            Message = msg;
        }

        public LogLevel Level
        {
            get;
            private set;
        }

        public string Message
        {
            get;
            private set;
        }
    }
}
