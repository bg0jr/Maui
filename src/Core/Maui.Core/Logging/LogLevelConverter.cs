using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Core;
using Blade;
using Blade.Logging;

namespace Maui.Logging
{
    /// <summary>
    /// Log4Net levels
    /// 
    /// 0x1d4c0 "EMERGENCY"
    /// 0x1adb0 "FATAL"
    /// 0x186a0 "ALERT"
    /// 0x15f90 "CRITICAL"
    /// 0x13880 "SEVERE"
    /// 0x11170 "ERROR"
    /// 
    /// 0x0ea60 "WARN"
    /// 0x0c350 "NOTICE"
    /// 0x09c40 "INFO"
    /// 
    /// 0x07530 "DEBUG"
    /// 0x07530 "FINE"
    /// 
    /// 0x04e20 "FINER"
    /// 0x04e20 "TRACE"
    /// 
    /// 0x02710 "FINEST"
    /// 0x02710 "VERBOSE"
    /// </summary>
    internal class LogLevelConverter
    {
        internal static LogLevel FromLog4Net( Level level )
        {
            if ( level == Level.Error )
            {
                return LogLevel.Error;
            }
            else if ( level == Level.Warn )
            {
                return LogLevel.Warning;
            }
            else if ( level == Level.Notice )
            {
                return LogLevel.Notice;
            }
            else if ( level == Level.Info )
            {
                return LogLevel.Info;
            }
            else if ( level == Level.Debug )
            {
                return LogLevel.Debug;
            }
            else
            {
                throw new NotSupportedException( "LogLevel not supported: " + level );
            }
        }

        internal static Level ToLog4Net( LogLevel level )
        {
            if ( level == LogLevel.Error )
            {
                return Level.Error;
            }
            else if ( level == LogLevel.Warning )
            {
                return Level.Warn;
            }
            else if ( level == LogLevel.Notice )
            {
                return Level.Notice;
            }
            else if ( level == LogLevel.Info )
            {
                return Level.Info;
            }
            else if ( level == LogLevel.Debug )
            {
                return Level.Debug;
            }
            else
            {
                throw new NotSupportedException( "LogLevel not supported: " + level );
            }
        }
    }
}
