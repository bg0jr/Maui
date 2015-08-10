using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Maui.Trading.Model;
using Maui.Entities;

namespace Maui.Trading.Binding.Decorators
{
    public class StackEnumerableDataSource<T> : IEnumerableDataSource<T>
    {
        public StackEnumerableDataSource( string name, IEnumerable<IEnumerableDataSource<T>> dataSources )
        {
            Name = name;
            DataSources = dataSources;
        }

        public string Name
        {
            get;
            private set;
        }

        public IEnumerable<IEnumerableDataSource<T>> DataSources
        {
            get;
            private set;
        }

        IEnumerable<T> IEnumerableDataSource<T>.ForStock( StockHandle stock )
        {
            return DataSources
                .Select( ds => ds.ForStock( stock ) )
                .FirstOrDefault( r => r != null && r.Any() );
        }
    }
}
