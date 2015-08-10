using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;
using Maui.Trading.Indicators;
using Maui.Trading.Modules;

namespace Maui.Trading.Reporting
{
    public class AdditionalReportsSection : AbstractSection, IReportGrouping
    {
        public AdditionalReportsSection(  )
            : this( "Additional reports" )
        {
        }

        public AdditionalReportsSection( string name )
            : base( name )
        {
            AdditionalReports = new List<Report>();
        }

        public IList<Report> AdditionalReports
        {
            get;
            private set;
        }

        public IEnumerable<Report> DetailedReports
        {
            get { return AdditionalReports; }
        }
    }
}
