using System;
using System.Data;
using System.Linq;
using System.Transactions;
using Maui.Logging;
using Maui.Dynamics.Data;
using Maui.Dynamics;
using Blade.Logging;

namespace Maui.Tasks.Dynamics
{
    public static class CrossSeriesFuction
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( CrossSeriesFuction ) );

        public static readonly TableSchema TempTable = new TableSchema( "crossseries",
           TableSchema.CreateReference( "stock" ),
           new DataColumn( "date", typeof( string ) ),
           new DataColumn( "value", typeof( double ) )
           );

        public static TableSchema CrossSeries( this IMslScript script, TableSchema left, TableSchema right, Func<double, double, double> Calculator )
        {
            return CrossSeries( script, left[ "value" ], right[ "value" ], Calculator );
        }

        public static TableSchema CrossSeries( this IMslScript script, DataColumn left, DataColumn right, Func<double, double, double> Calculator )
        {
            return CrossSeries( script, left, right, null, Calculator );
        }

        public static TableSchema CrossSeries( this IMslScript script, TableSchema left, TableSchema right, DataColumn to, Func<double, double, double> Calculator )
        {
            return CrossSeries( script, left[ "value" ], right[ "value" ], to, Calculator );
        }

        public static TableSchema CrossSeries( this IMslScript script, DataColumn leftCol, DataColumn rightCol, DataColumn toCol, Func<double, double, double> Calculator )
        {
            if ( toCol == null )
            {
                TempTable.Create();
                toCol = TempTable[ "value" ];
            }

            var leftTable = Interpreter.Context.TomScripting.GetManager( leftCol.Table.TableName );
            var rightTable = Interpreter.Context.TomScripting.GetManager( rightCol.Table.TableName );
            var outTable = Interpreter.Context.TomScripting.GetManager( toCol.Table.TableName );

            var stock = Interpreter.Context.Scope.Stock;
            var from = Interpreter.Context.Scope.From;
            var to = Interpreter.Context.Scope.To;

            using ( TransactionScope trans = new TransactionScope() )
            {
                var leftData = leftTable.Query( stock.GetId( leftTable.Schema.OwnerIdColumn ), new DateClause( from, to ), OriginClause.Default );
                var rightData = rightTable.Query( stock.GetId( rightTable.Schema.OwnerIdColumn ), new DateClause( from, to ), OriginClause.Default );

                // for easy processing lets join the left and right for ids and date
                // TODO: we should optimize this for the case that left and right
                // are in the same table
                var q = from left in leftData.Rows
                        from right in rightData.Rows
                        where left.GetDate( leftTable.Schema ) == right.GetDate( rightTable.Schema )
                        select new
                        {
                            Date = left.GetDate( leftTable.Schema ),
                            Left = left.Field<double>( leftCol.ColumnName ),
                            Right = right.Field<double>( rightCol.ColumnName )
                        };

                var ownerId = stock.GetId( outTable.Schema.OwnerIdColumn );
                var result = outTable.Query( ownerId );

                foreach ( var row in q )
                {
                    double r = Calculator( row.Left, row.Right );

                    DataRow outRow = result.NewRow();
                    outRow[ result.Schema.OwnerIdColumn ] = ownerId;
                    outRow.SetDate( result.Schema, row.Date );
                    outRow[ toCol.ColumnName ] = r;

                    result.AddOrUpdate( outRow, toCol.ColumnName );
                }

                trans.Complete();
            }
            return Interpreter.Context.TomScripting.GetManager( toCol.Table.TableName ).Schema;
        }
    }
}
