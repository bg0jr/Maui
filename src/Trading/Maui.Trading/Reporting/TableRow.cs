using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Reporting
{
    public class TableRow
    {
        private TableSection myTable;

        private IDictionary<string, object> myCells;

        public TableRow( TableSection table )
            : this( table, null )
        {
        }

        public TableRow( TableSection table, params object[] values )
        {
            myTable = table;
            myCells = new Dictionary<string, object>();

            SetValues( values );
        }

        private void SetValues( object[] values )
        {
            if ( values == null )
            {
                return;
            }

            var columns = myTable.Header.Columns.ToList();
            var size = Math.Min( columns.Count, values.Length );

            for ( int i = 0; i < size; ++i )
            {
                myCells[ columns[ i ].Name ] = values[ i ];
            }
        }

        public object this[ string column ]
        {
            get
            {
                EnsureColumnExists( column );

                if ( !myCells.ContainsKey( column ) )
                {
                    return null;
                }

                return myCells[ column ];
            }
            set
            {
                EnsureColumnExists( column );

                myCells[ column ] = value;
            }
        }

        public object this[ TableColumn column ]
        {
            get
            {
                EnsureColumnExists( column );

                return this[ column.Name ];
            }
            set
            {
                EnsureColumnExists( column );

                myCells[ column.Name ] = value;
            }
        }

        private void EnsureColumnExists( string column )
        {
            if ( !myTable.Header.Columns.Any( col => col.Name == column ) )
            {
                throw new ArgumentException( "no such column: " + column );
            }
        }

        private void EnsureColumnExists( TableColumn column )
        {
            if ( !myTable.Header.Columns.Contains( column ) )
            {
                throw new ArgumentException( "no such column: " + column.Name );
            }
        }
    }
}
