using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using Blade;
using Blade.Data;
using Blade.Collections;

namespace Maui.Dynamics.Data
{
    /// <summary>
    /// Provides a scoped view to the data.
    /// The scope is described by the "row filter".
    /// </summary>
    public class ScopedTable
    {
        private Func<DataRow, bool> myRowFilter = null;
        private Func<IEnumerable<DataRow>, IEnumerable<DataRow>> myTableFilter = null;

        public ScopedTable( TableSchema schema, DataTable table, OriginClause originClause )
            : this( schema, table, originClause, null )
        {
        }

        public ScopedTable( TableSchema schema, DataTable table, OriginClause originClause, Func<DataRow, bool> rowFilter )
        {
            this.Require( x => table != null );
            this.Require( x => schema != null );

            Schema = schema;
            Table = table;

            // set the row filter
            // -> no operation on deleted rows
            myRowFilter = r => r.RowState != DataRowState.Deleted && ( rowFilter == null || rowFilter( r ) );

            myTableFilter = CreateOriginClause( originClause );
        }

        public TableSchema Schema { get; private set; }
        private DataTable Table { get; set; }

        public IEnumerable<DataRow> Rows
        {
            get
            {
                return myTableFilter( Table.Rows.ToSet().Where( myRowFilter ) );
            }
        }

        /// <summary>
        /// Creates a new row.
        /// </summary>
        public DataRow NewRow()
        {
            return Table.NewRow();
        }

        /// <summary>
        /// Adds the given row without any checks.
        /// </summary>
        public void Add( DataRow row )
        {
            ValidateAndComplete( row );
            Table.Rows.Add( row );
        }

        /// <summary>
        /// Updates the row with the given id with the given data.
        /// </summary>
        public void Update( long id, DataRow row )
        {
            var existingRow = Rows.FirstOrDefault( r => r[ Schema.IdColumn ].Equals( id ) );
            if ( existingRow == null )
            {
                throw new Exception( "Invalid id: " + id );
            }

            CopyValues( row, existingRow, null );
        }

        /// <summary>
        /// No merge - just copy.
        /// </summary>
        public bool AddOrUpdate( DataRow row )
        {
            return AddOrUpdate( row, null );
        }

        /// <summary>
        /// Checks against the scope (e.g. ownerId, or ownerId and date range).
        /// If one or more rows with the given scope parameters already exist they 
        /// get updated otherwise the row is added.
        /// If the table has a "timestamp" column this gets updated too.
        /// 
        /// If "copyColumns" is set only these columns are copied in case of an update, 
        /// otherwise all columns are copied.
        /// </summary>
        public bool AddOrUpdate( DataRow row, params string[] copyColumns )
        {
            Func<DataRow, bool> ExistsRow = null;
            if ( Schema.DateColumn == null )
            {
                ExistsRow = r => r[ Schema.OwnerIdColumn ].Equals( row[ Schema.OwnerIdColumn ] );
            }
            else
            {
                if ( Schema.OriginColumn == null )
                {
                    ExistsRow = r => r[ Schema.OwnerIdColumn ].Equals( row[ Schema.OwnerIdColumn ] ) &&
                                     r[ Schema.DateColumn ].Equals( row[ Schema.DateColumn ] );
                }
                else
                {
                    ExistsRow = r => r[ Schema.OwnerIdColumn ].Equals( row[ Schema.OwnerIdColumn ] ) &&
                                     r[ Schema.DateColumn ].Equals( row[ Schema.DateColumn ] ) &&
                                     r[ Schema.OriginColumn ].Equals( row[ Schema.OriginColumn ] );
                }
            }

            // XXX: we recreate this list everytime we add a stock_price :(
            IList<DataRow> existingRows = null;
            try
            {
                existingRows = Rows.Where( ExistsRow ).ToList();
            }
            catch
            {
                // we could not get rows for a specific origin
                // -> ignore the exception, it just means that there are no existing rows
            }

            if ( existingRows != null && existingRows.Count > 0 )
            {
                using ( TransactionScope trans = new TransactionScope() )
                {
                    foreach ( var existingRow in existingRows )
                    {
                        CopyValues( row, existingRow, copyColumns );
                    }

                    trans.Complete();
                }
            }
            else
            {
                ValidateAndComplete( row );
                Table.Rows.Add( row );
            }

            return true;
        }

        private void CopyValues( DataRow src, DataRow dest, params string[] copyColumns )
        {
            dest.BeginEdit();

            if ( copyColumns != null )
            {
                copyColumns.Foreach( col => dest[ col ] = src[ col ] );
            }
            else
            {
                Schema.ValueColumns.Foreach( col => dest[ col.ColumnName ] = src[ col.ColumnName ] );
            }

            ValidateAndComplete( dest );

            dest.EndEdit();
        }

        private void ValidateAndComplete( DataRow row )
        {
            if ( Schema.TimestampColumn != null )
            {
                row.SetDate( Schema.TimestampColumn, DateTime.Now );
            }

            foreach ( var col in Schema.ColumnsWithoutId.Where( c => !c.AllowDBNull ) )
            {
                if ( row[ col.ColumnName ] == DBNull.Value || row[ col.ColumnName ] == null )
                {
                    throw new Exception( string.Format( "Column must not be null: {0}.{1}", Schema.Name, col.ColumnName ) );
                }
            }
        }

        private Func<IEnumerable<DataRow>, IEnumerable<DataRow>> CreateOriginClause( OriginClause originClause )
        {
            if ( originClause == null )
            {
                return rows => rows;
            }

            if ( Schema.OriginColumn == null )
            {
                return rows => rows;
            }

            return rows => FilterByOrigin( originClause, rows );
        }

        private IEnumerable<DataRow> FilterByOrigin( OriginClause originClause, IEnumerable<DataRow> rows )
        {
            // 1. the sort may create groups
            // 2. foreach group (1-n rows) choose one row

            string orderCol = ( Schema.DateColumn != null ? Schema.DateColumn : Schema.OwnerIdColumn );

            var rowGroup = new List<DataRow>();
            object origin = null;
            foreach ( var row in rows.OrderBy( row => row[ orderCol ] ) )
            {
                if ( rowGroup.Count == 0 )
                {
                    rowGroup.Add( row );
                    continue;
                }

                // new group?
                if ( rowGroup[ 0 ][ orderCol ].Equals( row[ orderCol ] ) )
                {
                    rowGroup.Add( row );
                    continue;
                }

                // analyse the group
                var outRow = GetRowByGroup( rowGroup, origin, originClause );
                if ( outRow != null )
                {
                    origin = outRow[ Schema.OriginColumn ];
                    yield return outRow;
                }

                rowGroup.Clear();
                rowGroup.Add( row );
            }

            // hanlde remaining group
            {
                var outRow = GetRowByGroup( rowGroup, origin, originClause );
                if ( outRow != null )
                {
                    yield return outRow;
                }
            }
        }

        private DataRow GetRowByGroup( IEnumerable<DataRow> rowGroup, object origin, OriginClause originClause )
        {
            // analyse the group
            if ( originClause.IsMergeAllowed )
            {
                // merging is allowed so just take the best of ranking
                return GetRowByRanking( rowGroup, originClause.Ranking );
            }

            if ( origin == null )
            {
                // no origin yet selected -> free choise
                return GetRowByRanking( rowGroup, originClause.Ranking );
            }
            else
            {
                // take the same origin as before
                var outRow = rowGroup.FirstOrDefault( r => r[ Schema.OriginColumn ].Equals( origin ) );
                if ( outRow == null )
                {
                    // TODO: throwing an exception here??
                    //       but we could not simple ignore this group of rows ...
                    // TODO: get the string
                    string orderCol = ( Schema.DateColumn != null ? Schema.DateColumn : Schema.OwnerIdColumn );
                    throw new Exception( string.Format( "Could not get a value for {0} and {1}", rowGroup.First()[ orderCol ], origin ) );
                }

                return outRow;
            }
        }

        private DataRow GetRowByRanking( IEnumerable<DataRow> group, IEnumerable<long> ranking )
        {
            var first = group.FirstOrDefault();
            if ( first == null )
            {
                return null;
            }

            foreach ( var origin in ranking )
            {
                var row = group.FirstOrDefault( r => r[ Schema.OriginColumn ].Equals( (long)origin ) );
                if ( row != null )
                {
                    return row;
                }
            }

            return first;
        }
    }
}
