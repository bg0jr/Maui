using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;

namespace Maui.Trading.Data
{
    public interface IPriceSeriesOperator
    {
        IPriceSeries Apply( IPriceSeries series );
    }
}
