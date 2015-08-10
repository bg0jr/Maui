using System.Data;
using Maui.Dynamics.Data;
using Maui.Dynamics;

namespace Maui.Tasks.Dynamics
{
    public static class PercentageFunction
    {
        public static readonly TableSchema TempTable = new TableSchema( "percentage",
           TableSchema.CreateReference( "stock" ),
           new DataColumn( "date", typeof( string ) ),
           new DataColumn( "value", typeof( double ) )
           );

        public static TableSchema Percentage( this IMslScript script, TableSchema dividend, TableSchema divisor )
        {
            return script.Percentage( dividend[ "value" ], divisor[ "value" ] );
        }

        public static TableSchema Percentage( this IMslScript script, DataColumn dividend, DataColumn divisor )
        {
            return script.Percentage( dividend, divisor, null );
        }

        public static TableSchema Percentage( this IMslScript script, TableSchema dividend, TableSchema divisor, DataColumn into )
        {
            return script.Percentage( dividend[ "value" ], divisor[ "value" ], into );
        }

        public static TableSchema Percentage( this IMslScript script, DataColumn dividend, DataColumn divisor, DataColumn into )
        {
            if ( into == null )
            {
                TempTable.Create();
                into = TempTable[ "value" ];
            }

            return script.CrossSeries( dividend, divisor, into, ( _dividend, _divisor ) => _dividend / _divisor * 100 );
        }
    }
}
