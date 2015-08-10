using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Reporting
{
    public class DetailedReportAdapter : DetailedReport
    {
        private Report myReport;

        public DetailedReportAdapter( string reference, Report report )
            : base( reference, report.Name, report.Title )
        {
            myReport = report;
        }

        public override IList<AbstractSection> Sections
        {
            get
            {
                return myReport.Sections;
            }
        }
    }
}
