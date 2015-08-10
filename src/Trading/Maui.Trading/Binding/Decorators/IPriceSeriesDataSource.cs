using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;
using Maui.Entities;

namespace Maui.Trading.Binding.Decorators
{
    public interface IPriceSeriesDataSource : IDataSource
    {
        IPriceSeries ForStock( StockHandle stock );
    }
}
