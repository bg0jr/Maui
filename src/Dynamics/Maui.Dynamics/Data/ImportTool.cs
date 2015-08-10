using System;
using System.Data;
using System.Linq;
using System.Transactions;
using Blade.Data;
using Maui.Entities;

namespace Maui.Dynamics.Data
{
    public static class ImportTool
    {
        /// <summary>
        /// Imports the given table into TOM.
        /// <remarks>
        /// The values of the table belong to the given origin and the given currency.
        /// All rows are owned by the given stock. The table name identifies the datum
        /// (and so the target table).
        /// </remarks>
        /// </summary>
        public static void Import( this MauiX.IImport self, StockHandle stock, DataTable table, DatumOrigin origin, Maui.Entities.Currency currency )
        {
            // XXX: how to handle origin and currency if we allow "merged results"?
            //  then we actually already need to enrich the result table from the policy
            //  with origin and currency ...

            var tomScripting = Engine.ServiceProvider.TomScripting();

            var datum = table.TableName;

            using ( TransactionScope trans = new TransactionScope() )
            {
                // we have a datum so we need the table now
                var mgr = tomScripting.GetManager( datum );
                if ( mgr == null )
                {
                    throw new Exception( "No table found for datum: " + datum );
                }

                // get scoped data for owner id
                long ownerId = stock.GetId( mgr.Schema.OwnerIdColumn );
                ScopedTable output = mgr.Query( ownerId );

                var resultColumns = table.Columns.ToSet();
                var dateCol = resultColumns.FirstOrDefault( c => c.IsDateColumn() );

                // setting date and values depend on the format
                Action<DataRow, DataRow> SetValues = null;
                // default date setter: assume there is no date 
                Action<DataRow, DataRow> SetDate = ( dest, src ) => { };
                if ( dateCol != null )
                {
                    SetDate = ( dest, src ) => dest.SetDate( mgr.Schema, src.GetDate( dateCol.ColumnName ) );
                }

                var datumCols = output.Schema.DatumColumns
                    .Where( col => resultColumns.Any( c => c.ColumnName.Equals( col.ColumnName, StringComparison.OrdinalIgnoreCase ) ) );
                SetValues = ( dest, src ) =>
                {
                    foreach ( var col in datumCols )
                    {
                        dest[ col.ColumnName ] = src[ col.ColumnName ];
                    }
                };

                // ok - now import the data

                DateIdCache<long> cache = new DateIdCache<long>();
                if ( output.Schema.DateColumn != null )
                {
                    cache.Fill( output );
                }

                foreach ( DataRow row in table.Rows )
                {
                    var newRow = output.NewRow();

                    // set owner id
                    newRow[ output.Schema.OwnerIdColumn ] = ownerId;

                    // a "datum" always need a date
                    SetDate( newRow, row );

                    // set values
                    SetValues( newRow, row );

                    // set currency (the value is of no use when there is no currency)
                    // -> currency might implicitly available through TradedStock->StockExchange
                    if ( output.Schema.CurrencyColumn != null )
                    {
                        newRow[ output.Schema.CurrencyColumn ] = currency.Id;
                    }

                    // set origin (optional)
                    if ( output.Schema.OriginColumn != null )
                    {
                        newRow[ output.Schema.OriginColumn ] = origin.Id;
                    }

                    if ( output.Schema.DateColumn != null && cache.Contains( newRow.GetDate( output.Schema ) ) )
                    {
                        long id = cache[ newRow.GetDate( output.Schema ) ];
                        output.Update( id, newRow );
                    }
                    else
                    {
                        output.Add( newRow );
                    }
                }

                trans.Complete();
            }
        }
    }
}
