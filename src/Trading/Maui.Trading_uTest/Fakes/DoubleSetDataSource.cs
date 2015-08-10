using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Binding;
using Maui.Trading.Model;
using Maui.Entities;

namespace Maui.Trading.UnitTests.Fakes
{
    public class DoubleSetDataSource : IEnumerableDataSource<double>
    {
        public DoubleSetDataSource( string name )
        {
            Name = name;
        }

        public IEnumerable<double> ForStock( StockHandle stock )
        {
            if ( stock.Isin == "a" )
            {
                return new List<double>() { 1, 2, 3 };
            }
            else if ( stock.Isin == "b" )
            {
                return new List<double>() { 1, 2, 3 };
            }

            return new List<double>();
        }

        public string Name
        {
            get;
            private set;
        }
    }
}
