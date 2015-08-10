using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Blade.Collections;

namespace Maui.Dynamics.Data
{
    public class DefaultFilterBuilder
    {
        public static DataRow[] EmptyRowSet = { };

        public DefaultFilterBuilder( TableSchema schema )
        {
            Schema = schema;
            NotApplyableFallback = PassThroughFilter;
        }

        public TableSchema Schema { get; private set; }

        /// <summary>
        /// Defines what a filter should return if the concrete filter could
        /// not be applied e.g. because the table schema does not contain 
        /// required columns.
        /// Default: "PassThroughFilter"
        /// </summary>
        public RowFilter NotApplyableFallback { get; set; }

        /// <summary>
        /// Simply by-passes all data passed in.
        /// </summary>
        public RowFilter PassThroughFilter
        {
            get
            {
                return rows => rows;
            }
        }

        /// <summary>
        /// Always return an empty row set.
        /// </summary>
        public RowFilter EmptyFilter
        {
            get
            {
                return rows => EmptyRowSet;
            }
        }

        public RowFilter CreateOriginFilter( long origin )
        {
            if ( Schema.OriginColumn == null )
            {
                return NotApplyableFallback;
            }

            return rows => rows.Where( row => origin == (long)row[ Schema.OriginColumn ] );
        }

        /// <summary>
        /// Creates a filter which returns all data of the first origin for which data exists.
        /// </summary>
        public RowFilter CreateOriginFilter( IEnumerable<long> originPrios )
        {
            if ( Schema.OriginColumn == null )
            {
                return NotApplyableFallback;
            }

            return rows =>
            {
                var origin = originPrios.FirstOrDefault(
                    o => rows.Any( row => o == (long)row[ Schema.OriginColumn ] ),
                    -1 );

                if ( origin == -1 )
                {
                    return EmptyRowSet;
                }

                return rows.Where( row => origin == (long)row[ Schema.OriginColumn ] );
            };
        }

        public RowFilter CreateDateRangeFilter( DateTime from, DateTime to )
        {
            if ( Schema.DateColumn == null )
            {
                return NotApplyableFallback;
            }

            Blade.DateTimeExtensions.NormalizeRange( ref from, ref to );

            if ( Schema.DateIsYear )
            {
                return rows => rows.Where( row => row.GetDate( Schema ).Year >= from.Year && row.GetDate( Schema ).Year <= to.Year );
            }
            else
            {
                return rows => rows.Where( row => row.GetDate( Schema ) >= from && row.GetDate( Schema ) <= to );
            }
        }

        public RowFilter CreateYearOriginFilter( DateTime from, DateTime to, IEnumerable<long> originPrios, bool allowMerge )
        {
            if ( Schema.OriginColumn == null )
            {
                return NotApplyableFallback;
            }

            RowFilter dateRangeFilter = CreateDateRangeFilter( from, to );
            if ( dateRangeFilter == NotApplyableFallback )
            {
                return NotApplyableFallback;
            }

            Blade.DateTimeExtensions.NormalizeRange( ref from, ref to );

            RowFilter filter = rows =>
            {
                if ( rows.Count() == 0 )
                {
                    return EmptyRowSet;
                }

                // this will be the final result
                // -> one value per year
                DataRow[] result = new DataRow[ to.Year - from.Year + 1 ];

                // now lets check each origin in order of the given priorities
                foreach ( var origin in originPrios )
                {
                    if ( !allowMerge )
                    {
                        // dont keep the previous results
                        Array.Clear( result, 0, result.Length );
                    }

                    // fill the result table with the found temp result
                    foreach ( var r in CreateOriginFilter( origin )( rows ) )
                    {
                        int idx = r.GetDate( Schema.DateColumn ).Year - from.Year;

                        // do not overwrite -> set new values only
                        if ( idx < result.Length && result[ idx ] == null )
                        {
                            result[ idx ] = r;
                        }
                    }

                    // if the temp result filled the final result completely we 
                    // found all we were searching for
                    if ( result.All( r => r != null ) )
                    {
                        return result;
                    }
                }

                return EmptyRowSet;
            };

            return filter.Compose( dateRangeFilter );
        }
    }
}
