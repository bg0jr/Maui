using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Reporting
{
    // TODO: this is actually only a markup interface to find "charts containing section".
    // we should later on add a really usefull interface or baseclass if we know which kinds
    // of chartsections we will have
    public interface IChartSection
    {
        string Name { get; }
    }
}
