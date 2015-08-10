using System;
using Blade.Data;
using Maui.Entities;
using Maui.Dynamics.Data;
using Maui.Dynamics;

namespace Maui.Tasks.Dynamics
{
    /// <summary>
    /// Prints the given things to the script console.
    /// <remarks>
    /// The script should write to the console directly. This way it is 
    /// esier to use another protocol later.
    /// </remarks>
    /// </summary>
    public static class EchoFunction
    {
        public static void Echo( this IMslScript script, string message, params object[] args )
        {
            Console.WriteLine( "[Echo] " + Interpreter.Evaluate( message ), args );
        }

        public static void Echo( this IMslScript script, TableSchema schema )
        {
            long ownerId = Interpreter.Context.Scope.Stock.GetId( schema.OwnerIdColumn );

            Interpreter.Context.TomScripting.GetManager( schema )
                .Query( ownerId ).Rows.Dump();
        }

        public static void Echo( this IMslScript script, StockCatalog catalog )
        {
            Console.WriteLine( "Catalog: " + catalog.Name );
            foreach ( var tsh in catalog.TradedStocks )
            {
                Console.WriteLine( string.Format( "   Isin = {0,15}, Symbol = {1,15}, WPKN = {2,15}",
                    tsh.Stock.Isin, tsh.Symbol, tsh.Wpkn ) );
            }
        }
    }
}
