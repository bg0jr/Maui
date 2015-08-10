using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Blade.IO;
using Blade.Data;
using System.Data;
using Blade;

namespace Maui.Data.Sheets
{
    /// <summary>
    /// Base class for DataTable based implementations of a worksheet.
    /// </summary>
    public abstract class AbstractDataTableWorksheet : IWorksheet
    {
        /// <summary/>
        public AbstractDataTableWorksheet( IWorkbook workbook, string name )
        {
            Workbook = workbook;
            Name = name;
        }

        /// <summary>
        /// Fired whenever the sheet has been modified.
        /// </summary>
        public event EventHandler Modified;

        private void FireModified()
        {
            if ( Modified != null )
            {
                Modified( this, EventArgs.Empty );
            }
        }
        
        protected IWorkbook Workbook { get; private set; }
        protected abstract DataTable Table { get; }

        /// <summary/>
        public string Name { get; private set; }

        /// <summary/>
        public object GetCell( string position )
        {
            var coords = GetRowColumn( position );

            Table.GrowToCell( coords.Item1, coords.Item2 );

            return Table.Rows[ coords.Item1 ][ coords.Item2 ];
        }

        /// <summary>
        /// Turns excel like position into zero-based row-column index.
        /// </summary>
        private Tuple<int, int> GetRowColumn( string position )
        {
            string row = string.Empty;
            int column = 0;

            foreach ( var c in position )
            {
                if ( char.IsDigit( c ) )
                {
                    row += c;
                }
                else
                {
                    column += ( c - 'A' );
                }
            }

            return new Tuple<int, int>( int.Parse( row ) - 1, column );
        }

        /// <summary/>
        public T GetCell<T>( string position )
        {
            return (T)GetCell( position );
        }

        /// <summary/>
        public void SetCell( string position, object value )
        {
            var coords = GetRowColumn( position );

            Table.GrowToCell( coords.Item1, coords.Item2 );

            Table.Rows[ coords.Item1 ][ coords.Item2 ] = value;

            FireModified();
        }

        /// <summary/>
        public virtual void Save()
        {
            Table.FitToContent();

            // its a temp thing - no save
        }

        /// <summary/>
        public bool IsEmptyCell( object cell )
        {
            return Table.IsEmptyCell( cell );
        }

        /// <summary>
        /// Dumps the content to the console.
        /// </summary>
        public virtual void Print()
        {
            Table.FitToContent();

            PrintTableContent();
        }

        private void PrintTableContent()
        {
            var rows = Table.Rows.ToSet();

            var first = rows.FirstOrDefault();
            if ( first == null )
            {
                Console.WriteLine( "table is empty" );
                return;
            }

            var columnWidths = CalculateColumnWidths();

            foreach ( DataRow row in rows )
            {
                for ( int i = 0; i < row.ItemArray.Length; ++i )
                {
                    int width = columnWidths[ i ] + 1;
                    Console.Write( string.Format( "{0," + width + "}", row.ItemArray[ i ] ) );
                }
                Console.WriteLine();
            }
        }

        private int[] CalculateColumnWidths()
        {
            var widths = new int[ Table.Columns.Count ];

            foreach ( DataRow row in Table.Rows )
            {
                for ( int col = 0; col < Table.Columns.Count; ++col )
                {
                    widths[ col ] = Math.Max( widths[ col ], GetCellWidth( row[ col ] ) );
                }
            }

            return widths;
        }

        private int GetCellWidth( object cell )
        {
            if ( cell == null || !( cell is string ) )
            {
                return 20;
            }

            return Math.Min( 20, ( (string)cell ).Length );
        }

        /// <summary/>
        public int GetRowCount()
        {
            return Table.Rows.Count;
        }

        /// <summary/>
        public int GetColumnCount()
        {
            return Table.Columns.Count;
        }
    }
}
