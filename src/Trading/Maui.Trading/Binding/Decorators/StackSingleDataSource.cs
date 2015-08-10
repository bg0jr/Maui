using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Maui.Trading.Model;
using Maui.Entities;

namespace Maui.Trading.Binding.Decorators
{
    public class StackSingleDataSource<T> : ISingleDataSource<T>
    {
        public StackSingleDataSource( string name, IEnumerable<ISingleDataSource<T>> dataSources )
        {
            Name = name;
            DataSources = dataSources;
        }

        public string Name
        {
            get;
            private set;
        }

        public IEnumerable<ISingleDataSource<T>> DataSources
        {
            get;
            private set;
        }

        T ISingleDataSource<T>.ForStock( StockHandle stock )
        {
            return DataSources
                .Select( ds => ds.ForStock( stock ) )
                .FirstOrDefault( r => !object.Equals( r, default( T ) ) );
        }
    }
}
