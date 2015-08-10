using System.Collections.Generic;
using System.Data;
using System.Linq;
using Maui.Logging;
using Maui.Dynamics.Data;
using Maui;
using Maui.Dynamics;
using Blade.Logging;

namespace Maui.Tasks.Dynamics
{
    public static class SetFunction
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( SetFunction ) );

        public static IEnumerable<double> ToSet( this IMslScript script, TableSchema from )
        {
            return ToSet( script, from[ "value" ] );
        }

        public static IEnumerable<double> ToSet( this IMslScript script, DataColumn fromCol )
        {
            var inputTable = Interpreter.Context.TomScripting.GetManager( fromCol.Table.TableName );

            var stock = Interpreter.Context.Scope.Stock;
            var from = Interpreter.Context.Scope.From;
            var to = Interpreter.Context.Scope.To;

            long ownerId = stock.GetId( inputTable.Schema.OwnerIdColumn );
            ScopedTable inputData = inputTable.Query( ownerId, new DateClause( from, to ), OriginClause.Default );

            return inputData.Rows.Select( row => row.Field<double>( fromCol.ColumnName ) );
        }

        public static ScopedTable Select( this IMslScript script, DataColumn valueColumn )
        {
            var mgr = Interpreter.Context.TomScripting.GetManager( valueColumn.Table.TableName );

            var stock = Interpreter.Context.Scope.Stock;
            var from = Interpreter.Context.Scope.From;
            var to = Interpreter.Context.Scope.To;

            long ownerId = stock.GetId( mgr.Schema.OwnerIdColumn );

            return mgr.Query( ownerId, new DateClause( from, to ), OriginClause.Default );
        }

        public static TimeSeries CreateSeries( this IMslScript script, DataColumn valueColumn )
        {
            var table = Select( null, valueColumn );
            return SeriesFactory.Create( table.Schema, table.Rows, valueColumn.ColumnName );
        }
    }
}
