using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Core;

namespace Maui.Logging.Log4Net
{
    internal class Log4NetLogger : Blade.Logging.ILogger
    {
        private ILog myLogger;
        private Type myLoggingType;

        public Log4NetLogger( Type loggingType )
        {
            if ( loggingType == null )
            {
                throw new ArgumentNullException( "loggingType" );
            }

            myLoggingType = loggingType;
            myLogger = LogManager.GetLogger( loggingType );
        }

        public void Debug( string format, params object[] args )
        {
            myLogger.DebugFormat( format, args );
        }

        /// <summary>
        /// Notice is higher than info
        /// </summary>
        public void Info( string format, params object[] args )
        {
            myLogger.InfoFormat( format, args );
        }

        /// <summary>
        /// Notice is higher than info
        /// </summary>
        public void Notice( string format, params object[] args )
        {
            myLogger.Logger.Log( myLoggingType, Level.Notice, string.Format( format, args ), null );
        }

        public void Warning( string format, params object[] args )
        {
            myLogger.WarnFormat( format, args );
        }

        public void Warning( Exception exception, string format, params object[] args )
        {
            myLogger.Warn( string.Format( format, args ), exception );
        }

        public void Error( string format, params object[] args )
        {
            myLogger.ErrorFormat( format, args );
        }

        public void Error( Exception exception, string format, params object[] args )
        {
            myLogger.Error( string.Format( format, args ), exception );
        }
    }
}
