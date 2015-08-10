using System.Data;
using Maui.Dynamics.Data;
using Maui.Dynamics;

namespace Maui.Tasks.Dynamics
{
    public static class PriceEarningRatioFunction
    {
        public static readonly TableSchema TempTable = new TableSchema( "per",
           TableSchema.CreateReference( "stock" ),
           new DataColumn( "date", typeof( string ) ),
           new DataColumn( "value", typeof( double ) )
           );

        public static TableSchema PriceEarningRatio( this IMslScript script, TableSchema price, TableSchema eps )
        {
            return script.PriceEarningRatio( price[ "value" ], eps[ "value" ] );
        }

        public static TableSchema PriceEarningRatio( this IMslScript script, DataColumn price, DataColumn eps )
        {
            return script.PriceEarningRatio( price, eps, null );
        }

        public static TableSchema PriceEarningRatio( this IMslScript script, TableSchema price, TableSchema eps, DataColumn into )
        {
            return script.PriceEarningRatio( price[ "value" ], eps[ "value" ], into );
        }

        public static TableSchema PriceEarningRatio( this IMslScript script, DataColumn price, DataColumn eps, DataColumn into )
        {
            if ( into == null )
            {
                TempTable.Create();
                into = TempTable[ "value" ];
            }

            return script.CrossSeries( price, eps, into, ( _price, _eps ) => _price / _eps );
        }
    }
}
