using System.Linq;
using System.Transactions;
using Blade;
using Maui.Entities;
using Maui;
using Maui.Logging;
using Maui.Shell;
using Blade.Logging;
using Blade.Shell;

namespace Maui.Data.Scripts
{
    public class ImportDatumOrigins : ScriptBase
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( ImportDatumOrigins ) );

        protected override void Run()
        {
            using ( var tom = Engine.ServiceProvider.CreateEntityRepository() )
            {
                if ( tom.DatumOrigins.Any() )
                {
                    myLogger.Info( "DatumOrigins already imported. Skipping ..." );
                    return;
                }

                using ( var trans = new TransactionScope() )
                {
                    tom.DatumOrigins.AddObject( new DatumOrigin( "Calculated" ) );
                    tom.DatumOrigins.AddObject( new DatumOrigin( "Business report" ) );

                    tom.SaveChanges();
                }
            }
        }
    }
}
