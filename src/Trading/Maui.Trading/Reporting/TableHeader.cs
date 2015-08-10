using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Reporting
{
    public class TableHeader
    {
        private IDictionary<string, TableColumn> myColumns;

        public TableHeader( params string[] columns )
            : this( columns.Select( c => new TableColumn( c ) ).ToArray() )
        {
        }

        public TableHeader( params TableColumn[] columns )
        {
            myColumns = new Dictionary<string, TableColumn>();

            foreach ( var col in columns )
            {
                myColumns[ col.Name ] = col;
            }
        }

        public IEnumerable<TableColumn> Columns
        {
            get
            {
                return myColumns.Values;
            }
        }

        public TableColumn this[ string column ]
        {
            get
            {
                return myColumns[ column ];
            }
        }
    }
}
