using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Maui.Trading.Model;

namespace Maui.Trading.Binding.Tom
{
    public class TomDataSourceFactory : AbstractDataSourceFactory
    {
        public override bool CanCreate( string name, Type type )
        {
            return name == DataSourceNames.Prices && typeof( IEnumerableDataSource<SimplePrice> ).IsAssignableFrom( type );
        }

        protected override object CreateDataSource( string name, Type type, NamedParameters ctorArgs )
        {
            var innerType = type.GetGenericArguments().First();

            var dataSource = new TomStockPricesDataSource();

            return dataSource;
        }
    }
}
