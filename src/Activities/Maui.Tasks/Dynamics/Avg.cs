using System.Data;
using System.Linq;
using Maui.Dynamics.Data;
using Maui.Dynamics;

namespace Maui.Tasks.Dynamics
{
    public static class AvgFunction
    {
        public static readonly TableSchema TempTable = new TableSchema( "avg",
           TableSchema.CreateReference( "stock" ),
           new DataColumn( "value", typeof( double ) )
           );

        public static TableSchema Avg( this IMslScript script, TableSchema from )
        {
            return Avg( script, from[ "value" ] );
        }

        public static TableSchema Avg( this IMslScript script, DataColumn from )
        {
            return script.Avg( from, null );
        }

        public static TableSchema Avg( this IMslScript script, TableSchema from, DataColumn into )
        {
            return Avg( script, from[ "value" ], into );
        }

        public static TableSchema Avg( this IMslScript script, DataColumn from, DataColumn into )
        {
            if ( into == null )
            {
                TempTable.RewriteOwnerId( from ).Create();
                into = TempTable[ "value" ];
            }

            return script.Aggregate( from, into, values => values.Average() );
        }

        public static double Avg( this IMslScript script, params double[] values )
        {
            return values.Average();
        }
    }
}
