using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Binding;
using Maui.Trading.Model;
using Maui.Entities;

namespace Maui.Trading.UnitTests.Fakes
{
    // "CreateGeneric()" fails with dynamic mocks :(
    public class FakeSingleDataSource<T> : ISingleDataSource<T>
    {
        public FakeSingleDataSource( string name )
        {
            Name = name;
            Data = new Dictionary<string, T>();
        }

        // data to return per stock, key: isin
        public Dictionary<string, T> Data
        {
            get;
            private set;
        }

        public T ForStock( StockHandle stock )
        {
            if ( Data.ContainsKey(stock.Isin) )
            {
                return Data[ stock.Isin ];
            }
            
            return default( T );
        }

        public string Name
        {
            get;
            private set;
        }
    }
}
