using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Reporting
{
    public abstract class AbstractTableView : ITableView
    {
        protected AbstractTableView( TableSection table )
        {
            Table = table;
        }

        protected TableSection Table
        {
            get;
            private set;
        }

        public abstract IEnumerable<TableRow> Rows { get; }
    }
}
