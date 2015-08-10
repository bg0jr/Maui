using System;
using System.Data;
using System.Linq;
using System.Transactions;
using Maui;
using Maui.Logging;
using Maui.Dynamics.Data;
using Maui.Dynamics;
using Blade.Logging;

namespace Maui.Tasks.Dynamics
{
    public static class HighLowFunction
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( HighLowFunction ) );

        public static readonly TableSchema TempTable = new TableSchema( "highlow",
            TableSchema.CreateReference( "traded_stock" ),
            new DataColumn( "date", typeof( string ) ),
            new DataColumn( "high", typeof( double ) ),
            new DataColumn( "low", typeof( double ) )
            );

        public static TableSchema HighLow( this IMslScript script, DataColumn value, TimeGrouping grouping )
        {
            return HighLow( script, value, grouping, null, null );
        }

        public static TableSchema HighLow( this IMslScript script, DataColumn value, TimeGrouping grouping, DataColumn intoHigh, DataColumn intoLow )
        {
            if ( intoHigh == null )
            {
                TempTable.Create();
                intoHigh = TempTable[ "high" ];
            }
            if ( intoLow == null )
            {
                TempTable.Create();
                intoLow = TempTable[ "low" ];
            }

            if ( intoHigh.Table.TableName != intoLow.Table.TableName )
            {
                throw new Exception( "High and low columns need to belong to the same table" );
            }

            var inputTable = Interpreter.Context.TomScripting.GetManager( value.Table.TableName );
            var outTable = Interpreter.Context.TomScripting.GetManager( intoHigh.Table.TableName );

            var stock = Interpreter.Context.Scope.Stock;
            var from = Interpreter.Context.Scope.From;
            var to = Interpreter.Context.Scope.To;

            using ( TransactionScope trans = new TransactionScope() )
            {
                long ownerId = stock.GetId( outTable.Schema.OwnerIdColumn );
                var result = outTable.Query( ownerId );

                ScopedTable inputData = inputTable.Query( ownerId, new DateClause( from, to ), OriginClause.Default );

                DateTime? currentPeriod = null;
                DateTime? newPeriod = null;
                double low = double.MaxValue;
                double high = double.MinValue;

                // helper Lambda which adds a found result to the result table
                Action AddResult = delegate()
                {
                    // handle no min, max could be determined
                    if ( low == double.MaxValue )
                    {
                        myLogger.Warning( "stock_price_low could not be determined for: TradedStockId = {0}", stock.TradedStock.Id );
                        low = -1;
                    }
                    if ( high == double.MinValue )
                    {
                        myLogger.Warning( "stock_price_high could not be determined for: TradedStockId = {0}", stock.TradedStock.Id );
                        high = -1;
                    }

                    DataRow outRow = result.NewRow();
                    outRow[ result.Schema.OwnerIdColumn ] = ownerId;
                    outRow.SetDate( result.Schema, currentPeriod.Value );
                    outRow[ intoHigh.ColumnName ] = high;
                    outRow[ intoLow.ColumnName ] = low;

                    result.AddOrUpdate( outRow, intoHigh.ColumnName, intoLow.ColumnName );
                };

                foreach ( DataRow row in inputData.Rows.OrderBy( row => row[ inputTable.Schema.DateColumn ] ) )
                {
                    DateTime date = row.GetDate( inputTable.Schema );

                    // convert the new period
                    if ( grouping == TimeGrouping.Year )
                    {
                        newPeriod = Blade.DateTimeExtensions.FirstOfYear( date.Year );
                    }
                    else if ( grouping == TimeGrouping.Month )
                    {
                        newPeriod = new DateTime( date.Year, date.Month, 1 );
                    }
                    else
                    {
                        throw new InvalidOperationException( "Unsupported time period definition: " + grouping );
                    }

                    // new period start detected other than the first one?
                    if ( currentPeriod != newPeriod && currentPeriod != null )
                    {
                        AddResult();

                        // reset temp variables
                        currentPeriod = newPeriod;
                        high = double.MinValue;
                        low = double.MaxValue;
                    }
                    else if ( currentPeriod == null )
                    {
                        // handle first period
                        currentPeriod = newPeriod;
                    }

                    double close = (double)row[ value.ColumnName ];

                    if ( close == -1 )
                    {
                        myLogger.Warning( "Ignoring invalid close price for: TradedStockId = {0}", stock.TradedStock.Id );
                        continue;
                    }

                    low = Math.Min( close, low );
                    high = Math.Max( close, high );
                }

                // we left the time scope but is there s.th. left to store?
                if ( low != double.MaxValue && high != double.MinValue )
                {
                    AddResult();
                }

                trans.Complete();
            }

            return Interpreter.Context.TomScripting.GetManager( intoHigh.Table.TableName ).Schema;
        }
    }
}
