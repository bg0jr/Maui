using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Core;
using log4net.Config;
using Blade;
using log4net.Filter;
using log4net.Appender;
using Maui.Logging.Log4Net;
using System.Diagnostics;
using Blade.Logging;

namespace Maui.Logging
{
    public class LoggerFactoryImpl : ILoggerFactoringImpl
    {
        private LogLevel? myLogLevel;

        static LoggerFactoryImpl()
        {
            LoggerFactory.Implementation = new LoggerFactoryImpl();
        }

        public void LoadConfiguration( Uri uri )
        {
            XmlConfigurator.Configure( uri );

            if ( myLogLevel.HasValue )
            {
                // log level has explicitly been set (e.g. starter -v) so lets override
                // the default from config file here
                LogLevel = myLogLevel.Value;
            }
            else
            {
                // init console logger
                ConsoleLogger.LogLevel = myLogLevel.HasValue ? myLogLevel.Value : Blade.Logging.LogLevel.Warning;
            }

            IsConfigured = true;
        }

        public bool IsConfigured
        {
            get;
            private set;
        }

        public void AddGuiAppender( ILoggingSink sink )
        {
            var appender = new LoggingSinkAppender( sink );
            appender.Layout = new ConsoleLoggingLayout();
            appender.Name = "GuiAppender";
            appender.AddFilter( new LevelRangeFilter() { LevelMin = LogManager.GetRepository().Threshold, LevelMax = Level.Fatal } );
            appender.AddFilter( new DenyAllFilter() );
            appender.ActivateOptions();

            BasicConfigurator.Configure( appender );
        }

        public Blade.Logging.ILogger GetLogger( Type loggingType )
        {
            if ( IsConfigured )
            {
                return new Log4NetLogger( loggingType );
            }
            else
            {
                return new ConsoleLoggerWrapper();
            }
        }

        public LogLevel LogLevel
        {
            get
            {
                var log4netLevel = LogManager.GetRepository().Threshold;
                return LogLevelConverter.FromLog4Net( log4netLevel );
            }
            set
            {
                myLogLevel = value;
                LogManager.GetRepository().Threshold = LogLevelConverter.ToLog4Net( myLogLevel.Value );

                // needs to be configured also because still used as fallback
                ConsoleLogger.LogLevel = myLogLevel.Value;

                if ( myLogLevel <= LogLevel.Info )
                {
                    AdaptAppenders();
                }
            }
        }

        private void AdaptAppenders()
        {
            var repository = LogManager.GetRepository();
            var appenders = repository
                .GetAppenders()
                .OfType<AppenderSkeleton>();

            foreach ( var appender in appenders )
            {
                AdaptFilters( appender, repository.Threshold );
            }
        }

        private void AdaptFilters( AppenderSkeleton appender, Level level )
        {
            var rangeFilter = appender.FilterHead as LevelRangeFilter;
            if ( rangeFilter == null )
            {
                return;
            }

            rangeFilter.LevelMin = level;
        }
    }
}
