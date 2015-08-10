using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;
using Maui.Entities;

namespace Maui.Trading.Binding.Decorators
{
    public class CachingSingleDataSource<T> : AbstractCachingDataSource<ISingleDataSource<T>, T>, ISingleDataSource<T>
    {
        public CachingSingleDataSource( ISingleDataSource<T> realDataSource )
            :base(realDataSource)
        {
        }

        T ISingleDataSource<T>.ForStock( StockHandle stock )
        {
            return ForStock( stock, RealDataSource.ForStock );
        }
    }
}
