using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Reporting
{
    public class DefaultTableView : AbstractTableView
    {
        public DefaultTableView( TableSection table )
            : base( table )
        {
        }

        public override IEnumerable<TableRow> Rows
        {
            get { return Table.Rows; }
        }
    }
}
