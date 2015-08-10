using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;
using Maui.Entities;

namespace Maui.Trading.Binding
{
    public interface ISingleDataSource<T> : IDataSource
    {
        T ForStock( StockHandle stock );
    }
}
