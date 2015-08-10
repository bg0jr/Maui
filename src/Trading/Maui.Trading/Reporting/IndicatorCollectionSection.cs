using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;
using Maui.Trading.Indicators;

namespace Maui.Trading.Reporting
{
    /// <summary>
    /// Lists all indicator results for one stock (if multiple indicators
    /// where calculated).
    /// Used by CombinedIndicator.
    /// </summary>
    public class IndicatorCollectionSection : AbstractSection, IReportGrouping
    {
        public IndicatorCollectionSection()
            : base( "Indicators" )
        {
            IndicatorResults = new List<IndicatorResult>();
        }

        public List<IndicatorResult> IndicatorResults
        {
            get;
            private set;
        }

        public IEnumerable<Report> DetailedReports
        {
            get
            {
                return IndicatorResults.Select( result => result.Report );
            }
        }
    }
}
