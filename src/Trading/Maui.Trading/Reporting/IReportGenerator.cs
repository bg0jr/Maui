using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Indicators;

namespace Maui.Trading.Reporting
{
    public interface IReportGenerator
    {
        Report Generate( IAnalysisResult result );
    }
}
