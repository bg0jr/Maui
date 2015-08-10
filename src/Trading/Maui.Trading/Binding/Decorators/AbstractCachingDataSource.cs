using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;
using Maui.Entities;

namespace Maui.Trading.Binding.Decorators
{
    public abstract class AbstractCachingDataSource<TDataSource, TData>
        where TDataSource : IDataSource
    {
        // key: isin
        private Dictionary<string, TData> myCache;

        public AbstractCachingDataSource( TDataSource realDataSource )
        {
            if ( realDataSource == null )
            {
                throw new ArgumentNullException( "realDataSource" );
            }

            RealDataSource = realDataSource;

            myCache = new Dictionary<string, TData>();
        }

        public string Name
        {
            get { return RealDataSource.Name; }
        }

        public TDataSource RealDataSource
        {
            get;
            private set;
        }

        protected TData ForStock( StockHandle stock, Func<StockHandle, TData> fetchData )
        {
            if ( !myCache.ContainsKey( stock.Isin ) )
            {
                myCache[ stock.Isin ] = fetchData( stock );
            }

            return myCache[ stock.Isin ];
        }
    }
}
