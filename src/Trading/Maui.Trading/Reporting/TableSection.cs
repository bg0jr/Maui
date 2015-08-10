using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blade;

namespace Maui.Trading.Reporting
{
    public class TableSection : AbstractSection
    {
        private ITableView myView;

        public TableSection( string name, TableHeader header )
            : base( name )
        {
            Header = header;

            myView = new DefaultTableView( this );
            Rows = new List<TableRow>();
        }

        public ITableView View
        {
            get { return myView; }
            set
            {
                if ( value == null )
                {
                    myView = new DefaultTableView( this );
                }
                else
                {
                    myView = value;
                }
            }
        }

        public TableHeader Header
        {
            get;
            private set;
        }

        public IList<TableRow> Rows
        {
            get;
            private set;
        }

        public TableRow NewRow( params object[] values )
        {
            return new TableRow( this, values );
        }
    }
}
