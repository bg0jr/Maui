using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;
using Maui.Entities;

namespace Maui.Trading.Binding.Decorators
{
    public class CachingEnumerableDataSource<T> : AbstractCachingDataSource<IEnumerableDataSource<T>, IEnumerable<T>>, IEnumerableDataSource<T>
    {
        public CachingEnumerableDataSource( IEnumerableDataSource<T> realDataSource )
            : base( realDataSource )
        {
        }

        IEnumerable<T> IEnumerableDataSource<T>.ForStock( StockHandle stock )
        {
            return ForStock( stock, RealDataSource.ForStock );
        }
    }
}
