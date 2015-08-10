using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Indicators;

namespace Maui.Trading.Reporting
{
    public interface IChartMergeOperator
    {
        bool HandledByMergeOperator( IChartSection chartSection );
        AbstractSection Merge( IAnalysisResult result );
    }
}
