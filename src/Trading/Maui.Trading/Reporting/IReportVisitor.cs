using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Reporting
{
    public interface IReportVisitor
    {
        void Visit( Report report );
        void Visit( AbstractSection section );
    }
}
