using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;
using Maui.Trading.Indicators;

namespace Maui.Trading.Reporting
{
    public class SystemDetailsSection : AbstractSection, IReportGrouping
    {
        public SystemDetailsSection()
            : base( "Details" )
        {
        }

        public IndicatorResult Indicator
        {
            get;
            set;
        }

        public IEnumerable<Report> DetailedReports
        {
            get { yield return Indicator.Report; }
        }
    }
}
