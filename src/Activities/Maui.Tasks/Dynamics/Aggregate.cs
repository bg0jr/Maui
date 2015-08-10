using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using Maui.Logging;
using Maui.Dynamics.Data;
using Maui.Dynamics;
using Blade.Logging;

namespace Maui.Tasks.Dynamics
{
    public static class AggregateFunction
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( AggregateFunction ) );

        public static readonly TableSchema TempTable = new TableSchema( "aggregate",
           TableSchema.CreateReference( "stock" ),
           new DataColumn( "value", typeof( double ) )
           );

        public static TableSchema Aggregate( this IMslScript script, TableSchema from, Func<IEnumerable<double>, double> Calculator )
        {
            return Aggregate( script, from[ "value" ], Calculator );
        }

        public static TableSchema Aggregate( this IMslScript script, DataColumn from, Func<IEnumerable<double>, double> Calculator )
        {
            return Aggregate( script, from, null, Calculator );
        }

        public static TableSchema Aggregate( this IMslScript script, TableSchema from, DataColumn to, Func<IEnumerable<double>, double> Calculator )
        {
            return Aggregate( script, from[ "value" ], to, Calculator );
        }

        public static TableSchema Aggregate( this IMslScript script, DataColumn fromCol, DataColumn toCol, Func<IEnumerable<double>, double> Calculator )
        {
            if ( toCol == null )
            {
                TempTable.RewriteOwnerId( fromCol ).Create();
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
                ScopedTable inputData = inputTable.Query( ownerId, new DateClause( from, to ), OriginClause.Default );

                // calculate
                var aggr = Calculator( inputData.Rows.Select( row => row.Field<double>( fromCol.ColumnName ) ) );

                var outData = outTable.Query( ownerId );

                DataRow outRow = outData.NewRow();
                outRow[ outData.Schema.OwnerIdColumn ] = ownerId;
                outRow[ toCol.ColumnName ] = aggr;

                outData.AddOrUpdate( outRow, toCol.ColumnName );

                trans.Complete();
            }

            return Interpreter.Context.TomScripting.GetManager( toCol.Table.TableName ).Schema;
        }
    }
}
