using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Reporting
{
    public class ChartSectionVisitor : IReportVisitor
    {
        public ChartSectionVisitor()
        {
            Sections = new List<AbstractSection>();
        }

        public List<AbstractSection> Sections
        {
            get;
            private set;
        }

        public void Visit( Report report )
        {
            // nothing to do
        }

        public void Visit( AbstractSection section )
        {
            if ( !typeof( IChartSection ).IsAssignableFrom( section.GetType() ) )
            {
                return;
            }

            Sections.Add( section );
        }
    }
}
