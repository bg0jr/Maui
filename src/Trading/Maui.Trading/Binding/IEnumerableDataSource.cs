using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;
using Maui.Entities;

namespace Maui.Trading.Binding
{
    public interface IEnumerableDataSource<T> : IDataSource
    {
        IEnumerable<T> ForStock( StockHandle stock );
    }
}
