using System.IO;
using Blade;
using Maui.Data.SQL;
using Maui.Data.SQL.SQLite;
using Maui.Entities;
using Maui;
using Maui.Logging;
using Blade.Logging;
using System.Collections.Generic;

namespace Maui.Data
{
    internal sealed class Step_030_TOM_Database : AbstractEngineInitializer, IBannerInfoProvider
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( Engine ) );

        public override void Init()
        {
            Config config = new Config();

            if ( config.TomDatabaseUri.IsNullOrTrimmedEmpty() )
            {
                config.TomDatabaseUri = Path.Combine( MauiEnvironment.Root, "maui_test.db" );
                // TODO: do we want to do this
                //Settings.Default.Save();
            }

            myLogger.Info( "Using Tom-Database: " + config.TomDatabaseUri );

            IDatabaseSC db = new SQLiteDatabaseSC( config.TomDatabaseUri );

            Engine.ServiceProvider.RegisterService( "TOM Database", db );
        }

        public override void Fini()
        {
            myLogger.Info( "Deinitializing Tom database" );

            IDatabaseSC db = (IDatabaseSC)Engine.ServiceProvider.GetService( "TOM Database" );
            Engine.ServiceProvider.UnregisterService( "TOM Database" );

            db.Dispose();
        }

        public KeyValuePair<string, string> GetBannerInformation()
        {
            var db = (IDatabaseSC)Engine.ServiceProvider.GetService( "TOM Database" );
            return new KeyValuePair<string, string>( "Database URI", db.ConnectionUri );
        }
    }

    internal sealed class Step_100_TOM_Interface : AbstractEngineInitializer
    {
        public override void Init()
        {
            var dbConnectionUri = ((IDatabaseSC)Engine.ServiceProvider.GetService( "TOM Database" )).ConnectionUri;
            var factory = new EntityRepositoryFactory( dbConnectionUri );
            Engine.ServiceProvider.RegisterService( typeof( IEntityRepositoryFactory ), factory );
        }
    }
}
