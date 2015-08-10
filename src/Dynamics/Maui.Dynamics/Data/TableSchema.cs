using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Blade;
using Maui;

namespace Maui.Dynamics.Data
{
    public class TableSchema
    {
        public TableSchema( string name, bool isPersistent, params DataColumn[] columns )
            : this( name, columns, isPersistent )
        {
        }

        public TableSchema( string name, params DataColumn[] columns )
            : this( name, columns, false )
        {
        }

        public TableSchema( string name, IEnumerable<DataColumn> columns )
            : this( name, columns, false )
        {
        }

        /// <summary>
        /// Creates a DataTable description (schema) with the given parameters.
        /// An id column is added automatically if there is no id column already.
        /// </summary>
        public TableSchema( string name, IEnumerable<DataColumn> columns, bool isPersistent )
        {
            this.Require( x => !string.IsNullOrEmpty( name ) );

            Name = name;
            IsPersistent = isPersistent;

            // always create new DataColumn objects because one DataColumn
            // can only belong to one table
            DataTable table = new DataTable( Name );
            var cols = new List<DataColumn>();
            foreach ( var col in columns )
            {
                var newCol = new DataColumn( col.ColumnName, col.DataType ) { AllowDBNull = col.AllowDBNull };

                cols.Add( newCol );

                // Associate column with a table so that we can move back
                // to the table name using the columns
                table.Columns.Add( newCol );
            }

            if ( !cols.Any( DataColumnExtensions.IsIdColumn ) )
            {
                var newCol = new DataColumn( "id", typeof( long ) );
                cols.Insert( 0, newCol );

                // Associate column with a table so that we can move back
                // to the table name using the columns
                table.Columns.Add( newCol );
            }

            Columns = cols;
        }

        public DataColumn this[ int idx ]
        {
            get
            {
                return Columns.ElementAt( idx );
            }
        }

        public DataColumn this[ string name ]
        {
            get
            {
                return Columns.FirstOrDefault( col => col.ColumnName.Equals( name, StringComparison.OrdinalIgnoreCase ) );
            }
        }

        public string Name { get; private set; }
        public IEnumerable<DataColumn> Columns { get; private set; }
        public bool IsPersistent { get; private set; }

        public ValidationResult Validate()
        {
            ValidationResult result = ValidationResult.None;

            // we need an owner ID
            DataColumn col = Columns.FirstOrDefault( DataColumnExtensions.IsOwnerIdColumn );
            if ( col == null )
            {
                result |= ValidationResult.OwnerIdRequired;
            }

            /*
            // we need a date column
            col = Columns.FirstOrDefault( DataColumnExtensions.IsDateColumn );
            if ( col == null )
            {
                result |= ValidationResult.DateRequired;
            }
            */

            if ( ValueColumns.Count() == 0 )
            {
                result |= ValidationResult.ValueColumnsMissing;
            }

            return result;
        }

        /// <summary>
        /// Creates a temp table based on the schema.
        /// </summary>
        public DataTable NewTempTable()
        {
            DataTable table = new DataTable( Name );
            foreach ( DataColumn col in Columns )
            {
                table.Columns.Add( new DataColumn( col.ColumnName, col.DataType ) );
            }
            return table;
        }

        /// <summary>
        /// Creates a new DataColumn for id columns.
        /// <remarks>
        /// Applies the following conventions to the defined DataColumns:
        /// <list>
        /// <item>column name gets the suffix "_id"</item>
        /// <item>column type is <c>long</c></item>
        /// <item>null values are not allowed</item>
        /// </list>
        /// </remarks>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DataColumn CreateReference( string name )
        {
            if ( !name.EndsWith( "_id" ) )
            {
                name += "_id";
            }

            return new DataColumn( name, typeof( long ) )
            {
                AllowDBNull = false
            };
        }

        #region Semantic column access
        private string myIdColumn = string.Empty;
        public string IdColumn
        {
            get
            {
                if ( myIdColumn == string.Empty )
                {
                    var col = Columns.FirstOrDefault( DataColumnExtensions.IsIdColumn );
                    myIdColumn = ( col != null ? col.ColumnName : null );
                }
                return myIdColumn;
            }
        }

        private string myOwnerIdColumn = string.Empty;
        public string OwnerIdColumn
        {
            get
            {
                if ( myOwnerIdColumn == string.Empty )
                {
                    var col = Columns.FirstOrDefault( DataColumnExtensions.IsOwnerIdColumn );
                    myOwnerIdColumn = ( col != null ? col.ColumnName : null );
                }
                return myOwnerIdColumn;
            }
        }

        private string myDateColumn = string.Empty;
        public string DateColumn
        {
            get
            {
                if ( myDateColumn == string.Empty )
                {
                    var col = Columns.FirstOrDefault( DataColumnExtensions.IsDateColumn );
                    myDateColumn = ( col != null ? col.ColumnName : null );
                }
                return myDateColumn;
            }
        }

        public bool DateIsYear
        {
            get
            {
                return DateColumn.Equals( "year", StringComparison.OrdinalIgnoreCase );
            }
        }

        private string myOriginColumn = string.Empty;
        public string OriginColumn
        {
            get
            {
                if ( myOriginColumn == string.Empty )
                {
                    var col = Columns.FirstOrDefault( DataColumnExtensions.IsOriginColumn );
                    myOriginColumn = ( col != null ? col.ColumnName : null );
                }
                return myOriginColumn;
            }
        }

        private string myCurrencyColumn = string.Empty;
        public string CurrencyColumn
        {
            get
            {
                if ( myCurrencyColumn == string.Empty )
                {
                    var col = Columns.FirstOrDefault( DataColumnExtensions.IsCurrencyColumn );
                    myCurrencyColumn = ( col != null ? col.ColumnName : null );
                }
                return myCurrencyColumn;
            }
        }

        private string myTimestampColumn = string.Empty;
        public string TimestampColumn
        {
            get
            {
                if ( myTimestampColumn == string.Empty )
                {
                    var col = Columns.FirstOrDefault( DataColumnExtensions.IsTimestampColumn );
                    myTimestampColumn = ( col != null ? col.ColumnName : null );
                }
                return myTimestampColumn;
            }
        }

        private IEnumerable<DataColumn> myValueColumns = null;
        /// <summary>
        /// Returns all columns which are no "id", "ownerid" or "date" column.
        /// </summary>
        public IEnumerable<DataColumn> ValueColumns
        {
            get
            {
                if ( myValueColumns == null )
                {
                    myValueColumns = Columns.Where( c => !( c.IsIdColumn() || c.IsOwnerIdColumn() || c.IsDateColumn() ) );
                }
                return myValueColumns;
            }
        }

        private IEnumerable<DataColumn> myDatumColumns = null;
        /// <summary>
        /// Returns all columns which are no ids, dates or other metadata columns.
        /// </summary>
        public IEnumerable<DataColumn> DatumColumns
        {
            get
            {
                if ( myDatumColumns == null )
                {
                    myDatumColumns = ValueColumns.Where( c =>
                        !c.ColumnName.EndsWith( "_id", StringComparison.OrdinalIgnoreCase ) &&
                        !c.ColumnName.Equals( "timestamp", StringComparison.OrdinalIgnoreCase ) &&
                        !c.ColumnName.Equals( "datum_validity", StringComparison.OrdinalIgnoreCase ) );
                }
                return myDatumColumns;
            }
        }

        private IEnumerable<DataColumn> myColumnsWithoutId = null;
        /// <summary>
        /// Returns all columns except the "id" column.
        /// </summary>
        public IEnumerable<DataColumn> ColumnsWithoutId
        {
            get
            {
                if ( myColumnsWithoutId == null )
                {
                    myColumnsWithoutId = Columns.Where( c => !c.IsIdColumn() );
                }
                return myColumnsWithoutId;
            }
        }

        #endregion
    }
}
