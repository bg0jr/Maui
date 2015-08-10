using System.Transactions;
using System.Linq;
using Blade;
using Maui.Entities;
using Maui;
using Maui.Logging;
using Maui.Shell;
using Blade.Logging;
using Blade.Shell;

namespace Maui.Data.Scripts
{
    public class ImportCurrencies : ScriptBase
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( ImportCurrencies ) );

        protected override void Run()
        {
            using ( var tom = Engine.ServiceProvider.CreateEntityRepository() )
            {
                if ( tom.Currencies.Any() )
                {
                    myLogger.Info( "Currencies already imported. Skipping ..." );
                    return;
                }

                using ( var trans = new TransactionScope() )
                {
                    tom.Currencies.AddObject( new Currency( name: "Euro", symbol: "€" ) );
                    tom.Currencies.AddObject( new Currency( name: "Dollar", symbol: "$" ) );
                    tom.Currencies.AddObject( new Currency( name: "Pounds", symbol: "p" ) );
                    tom.Currencies.AddObject( new Currency( name: "Swiss franc", symbol: "CHF" ) );

                    tom.SaveChanges();
                    trans.Complete();
                }
            }
        }
    }
}
