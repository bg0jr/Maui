using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Reporting
{
    public interface ITableView
    {
        IEnumerable<TableRow> Rows { get; }
    }
}
