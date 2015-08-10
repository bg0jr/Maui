using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Maui.Reflection;
using Blade.Reflection;
using Blade.Binding;

namespace Maui.Trading.Binding.Xml
{
    public class XmlDataSourceFactory : AbstractDataSourceFactory
    {
        private XmlDataStore myDataStore;

        public XmlDataSourceFactory( XmlDataStore dataStore )
        {
            myDataStore = dataStore;
        }

        public override bool CanCreate( string name, Type type )
        {
            return IsDefaultDataSource( type ) ||
                GetComparableType( type ) == typeof( ICurrencyDataSource );
        }

        protected override object CreateDataSource( string name, Type type, NamedParameters ctorArgs )
        {
            if ( type == typeof( ICurrencyDataSource ) )
            {
                return new XmlCurrencyDataSource( myDataStore );
            }
            else
            {
                var innerType = type.GetGenericArguments().First();

                var dataSource = typeof( XmlDataSource<> ).CreateGeneric( innerType, name, myDataStore );

                return dataSource;
            }
        }
    }
}
