using System.Data;
using System.Transactions;
using Maui.Logging;
using Maui.Dynamics.Data;
using Maui.Dynamics;
using Blade.Logging;

namespace Maui.Tasks.Dynamics
{
    /// <summary>
    /// <remarks>
    /// Actually this is a simplification of a "map" function
    /// </remarks>
    /// </summary>
    public static class CopySeriesFunction
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( CopySeriesFunction ) );

        public static readonly TableSchema TempTable = new TableSchema( "copyseries",
            TableSchema.CreateReference( "stock" ),
            new DataColumn( "date", typeof( string ) ),
            new DataColumn( "value", typeof( double ) )
            );

        public static TableSchema CopySeries( this IMslScript script, TableSchema from )
        {
            return CopySeries( script, from[ "value" ] );
        }

        public static TableSchema CopySeries( this IMslScript script, DataColumn from )
        {
            return CopySeries( script, from, null );
        }

        public static TableSchema CopySeries( this IMslScript script, TableSchema from, DataColumn to )
        {
            return CopySeries( script, from[ "value" ], to );
        }

        public static TableSchema CopySeries( this IMslScript script, DataColumn fromCol, DataColumn toCol )
        {
            if ( toCol == null )
            {
                TempTable.Create();
                toCol = TempTable[ "value" ];
            }

            var inputTable = Interpreter.Context.TomScripting.GetManager( fromCol.Table.TableName );
            var outTable = Interpreter.Context.TomScripting.GetManager( toCol.Table.TableName );

            var stock = Interpreter.Context.Scope.Stock;
            var from = Interpreter.Context.Scope.From;
            var to = Interpreter.Context.Scope.To;

            long ownerId = stock.GetId( inputTable.Schema.OwnerIdColumn );
            using ( TransactionScope trans = new TransactionScope() )
            {
                ScopedTable fromData = inputTable.Query( ownerId, new DateClause( from, to ), OriginClause.Default );

                var result = outTable.Query( ownerId );
                foreach ( DataRow row in fromData.Rows )
                {
                    DataRow outRow = result.NewRow();
                    outRow[ result.Schema.OwnerIdColumn ] = ownerId;
                    outRow.SetDate( outTable.Schema, row.GetDate( fromData.Schema ) );
                    outRow[ toCol.ColumnName ] = row[ fromCol.ColumnName ];

                    result.AddOrUpdate( outRow, toCol.ColumnName );
                }

                trans.Complete();
            }

            return Interpreter.Context.TomScripting.GetManager( toCol.Table.TableName ).Schema;
        }
    }
}
