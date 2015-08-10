using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;
using Maui.Trading.Reporting;
using Maui.Entities;

namespace Maui.Trading
{
    public interface IAnalysisResult
    {
        StockHandle Stock { get; }
        Report Report { get; }
    }
}
