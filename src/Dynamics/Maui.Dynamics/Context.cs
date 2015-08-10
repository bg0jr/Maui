using System;
using System.IO;
using Maui.Data.Recognition;
using Maui.Dynamics.Data;
using Maui.Logging;
using Blade.Logging;

namespace Maui.Dynamics
{
    /// <summary>
    /// Context for the current script.
    /// </summary>
    public class Context
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( Context ) );

        public Context( IDatumProviderFactory datumProviderFactory )
        {
            DatumProviderFactory = datumProviderFactory;

            MslHome = Path.Combine( MauiEnvironment.Root, "MSL" );
        }

        public IDatumProviderFactory DatumProviderFactory
        {
            get;
            private set;
        }

        public string MslHome = null;

        public ScriptingInterface TomScripting
        {
            get;
            set;
        }

        public ServiceProvider ServiceProvider
        {
            get;
            set;
        }

        public Scope Scope
        {
            get;
            set;
        }

        public ITableManager DailyStockPriceManager
        {
            get
            {
                var mgr = TomScripting.GetManager( Config.Instance.DailyStockPriceTable );
                if ( mgr == null )
                {
                    throw new Exception( "Table for daily stock prices does not exist: " + Config.Instance.DailyStockPriceTable );
                }

                return mgr;
            }
        }
    }
}
