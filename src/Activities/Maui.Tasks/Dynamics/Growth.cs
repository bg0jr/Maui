using System.Data;
using System.Linq;
using System.Transactions;
using Maui.Logging;
using Maui.Dynamics.Data;
using Maui.Dynamics;
using Blade.Logging;

namespace Maui.Tasks.Dynamics
{
    public static class GrowthFunction
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( GrowthFunction ) );

        public static readonly TableSchema TempTable = new TableSchema( "growth",
            TableSchema.CreateReference( "stock" ),
            new DataColumn( "date", typeof( string ) ),
            new DataColumn( "value", typeof( double ) )
            );

        public static TableSchema Growth( this IMslScript script, TableSchema from )
        {
            return Growth( script, from[ "value" ] );
        }

        public static TableSchema Growth( this IMslScript script, DataColumn from )
        {
            return Growth( script, from, null );
        }

        public static TableSchema Growth( this IMslScript script, TableSchema from, DataColumn into )
        {
            return Growth( script, from[ "value" ], into );
        }

        public static TableSchema Growth( this IMslScript script, DataColumn fromCol, DataColumn into )
        {
            if ( into == null )
            {
                TempTable.RewriteOwnerId( fromCol ).Create();
                into = TempTable[ "value" ];
            }

            var inputTable = Interpreter.Context.TomScripting.GetManager( fromCol.Table.TableName );
            var outTable = Interpreter.Context.TomScripting.GetManager( into.Table.TableName );

            var stock = Interpreter.Context.Scope.Stock;
            var from = Interpreter.Context.Scope.From;
            var to = Interpreter.Context.Scope.To;

            long ownerId = stock.GetId( inputTable.Schema.OwnerIdColumn );

            using ( TransactionScope trans = new TransactionScope() )
            {
                ScopedTable inputData = inputTable.Query( ownerId, new DateClause( from, to ), OriginClause.Default );

                var q = from row in inputData.Rows
                        orderby row.GetDateString( inputData.Schema ) ascending
                        select new
                        {
                            Date = row.GetDate( inputData.Schema ),
                            Value = row.Field<double>( fromCol.ColumnName ),
                        };

                var result = outTable.Query( ownerId );

                double prev = double.MaxValue;
                foreach ( var row in q )
                {
                    if ( prev == double.MaxValue )
                    {
                        prev = row.Value;
                    }

                    double growth = row.Value - prev;
                    prev = row.Value;

                    DataRow outRow = result.NewRow();
                    outRow[ result.Schema.OwnerIdColumn ] = ownerId;
                    outRow.SetDate( result.Schema, row.Date );
                    outRow[ into.ColumnName ] = growth;

                    result.AddOrUpdate( outRow, into.ColumnName );
                }

                trans.Complete();
            }

            return Interpreter.Context.TomScripting.GetManager( into.Table.TableName ).Schema;
        }
    }
}
