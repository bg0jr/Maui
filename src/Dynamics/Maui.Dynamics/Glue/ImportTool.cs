using System.Linq;
using Maui.Data.Recognition;
using Maui.Dynamics.Data;
using Maui.Entities;

namespace Maui.Dynamics
{
    public static class ImportTool
    {
        /// <summary>
        /// Imports the given result into TOM.
        /// </summary>
        public static void Import( this MauiX.IImport self, StockHandle stock, IResultPolicy result )
        {
            using ( var tom = Engine.ServiceProvider.CreateEntityRepository() )
            {
                var originName = result.Sites.First().Name;
                var origin = tom.DatumOrigins.FirstOrDefault( o => o.Name == originName );
                if ( origin == null )
                {
                    origin = new DatumOrigin( originName );
                    tom.DatumOrigins.AddObject( origin );
                    tom.SaveChanges();
                }

                var currencyName = result.Sites.First().Content.Currency;
                var currency = tom.Currencies.FirstOrDefault( c => c.Name == currencyName );

                MauiX.Import.Import( stock, result.ResultTable, origin, currency );
            }
        }
    }
}
