using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Maui.Trading.Model;
using Maui.Entities;
using Maui.Trading.Binding.Tom;
using Blade;
using Blade.Binding;

namespace Maui.Trading.Binding.Xml
{
    public class XmlDataSource<T> : ISingleDataSource<T>, IEnumerableDataSource<T>
    {
        private XmlDataStore myDataStore;

        public XmlDataSource( string name, XmlDataStore dataStore )
        {
            Name = name;
            myDataStore = dataStore;
        }

        public string Name
        {
            get;
            private set;
        }

        IEnumerable<T> IEnumerableDataSource<T>.ForStock( StockHandle stock )
        {
            var xmlDataSources = myDataStore.GetData( Name, stock );
            if ( xmlDataSources == null )
            {
                return new List<T>();
            }

            var dataSource = xmlDataSources
                .OfType<CollectionDataSource>()
                .Where( xmlDs => HasValidCurrency( xmlDs, stock ) )
                .SingleOrDefault();

            if ( dataSource == null )
            {
                return new List<T>();
            }

            return dataSource.Values.Cast<T>().ToList();
        }

        T ISingleDataSource<T>.ForStock( StockHandle stock )
        {
            var xmlDataSources = myDataStore.GetData( Name, stock );
            if ( !xmlDataSources.Any() )
            {
                return default( T );
            }

            var dataSource = xmlDataSources
                .OfType<SingleDataSource>()
                .Where( xmlDs => HasValidCurrency( xmlDs, stock ) )
                .SingleOrDefault();

            if ( dataSource == null )
            {
                return default( T );
            }

            var strValue = dataSource.Value as string;
            if ( strValue == null )
            {
                return (T)dataSource.Value;
            }
            else
            {
                return (T)Blade.TypeConverter.ConvertTo( strValue, typeof( T ) );
            }
        }

        private bool HasValidCurrency( AbstractDataSource source, StockHandle stock )
        {
            if ( source.Currency == null )
            {
                return true;
            }

            return stock.StockExchange.Currency.Name.EqualsI( source.Currency );
        }
    }
}
