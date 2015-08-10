using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;
using Maui.Reflection;
using Blade.Reflection;

namespace Maui.Trading.Binding.Decorators
{
    public class CachingDataSourceFactory : AbstractDataSourceFactory
    {
        private Dictionary<string, object> myDataSourceCache;

        public CachingDataSourceFactory( IDataSourceFactory realFactory )
        {
            if ( realFactory == null )
            {
                throw new ArgumentNullException( "realFactory" );
            }

            RealFactory = realFactory;

            myDataSourceCache = new Dictionary<string, object>();
        }

        public IDataSourceFactory RealFactory
        {
            get;
            private set;
        }

        public override bool CanCreate( string name, Type type )
        {
            return IsDefaultDataSource( type ) && RealFactory.CanCreate( name, type );
        }

        protected override object CreateDataSource( string name, Type type, NamedParameters ctorArgs )
        {
            // we need a cache here to allow caching inside the different DataSources

            var dataSourceId = GetDataSourceId( name, type );

            if ( !myDataSourceCache.ContainsKey( dataSourceId ) )
            {
                var dataSource = CreateCachingDataSource( name, type, ctorArgs );
                myDataSourceCache[ dataSourceId ] = dataSource;
            }

            return myDataSourceCache[ dataSourceId ];
        }

        // we cannot only take name - we have to encode the type otherwise we
        // will get bad cast exceptions
        private string GetDataSourceId( string name, Type type )
        {
            return name + "#" + type.FullName;
        }

        private object CreateCachingDataSource( string name, Type type, NamedParameters ctorArgs )
        {
            var realDataSource = RealFactory.Create( name, type, ctorArgs );

            var innerType = type.GetGenericArguments().First();
            if ( type.GetGenericTypeDefinition() == typeof( ISingleDataSource<> ) )
            {
                return typeof( CachingSingleDataSource<> ).CreateGeneric( innerType, realDataSource );
            }
            if ( type.GetGenericTypeDefinition() == typeof( IEnumerableDataSource<> ) )
            {
                return typeof( CachingEnumerableDataSource<> ).CreateGeneric( innerType, realDataSource );
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
