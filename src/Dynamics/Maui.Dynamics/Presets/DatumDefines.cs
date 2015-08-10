using System.Data;
using Maui.Dynamics.Types;
using Maui.Dynamics.Data;

namespace Maui.Dynamics.Presets
{
    public class DatumDefines
    {
        public static Datum Eps = new Datum( "eps", true,
            TableSchema.CreateReference( "stock" ),
            TableSchema.CreateReference( "currency" ),
            new DataColumn( "year", typeof( int ) ) { AllowDBNull = false },
            new DataColumn( "value", typeof( double ) ) { AllowDBNull = false }
            );

        public static Datum Dividend = new Datum( "dividend", true,
            TableSchema.CreateReference( "stock" ),
            TableSchema.CreateReference( "currency" ),
            new DataColumn( "year", typeof( int ) ) { AllowDBNull = false },
            new DataColumn( "value", typeof( double ) ) { AllowDBNull = false }
            );

        public static Datum StockPrice = new Datum( "stock_price", true,
            TableSchema.CreateReference( "traded_stock" ),
            new DataColumn( "date", typeof( string ) ) { AllowDBNull = false },
            new DataColumn( "open", typeof( double ) ),
            new DataColumn( "high", typeof( double ) ),
            new DataColumn( "low", typeof( double ) ),
            new DataColumn( "close", typeof( double ) ) { AllowDBNull = false },
            new DataColumn( "volume", typeof( int ) )
            );
    }
}
