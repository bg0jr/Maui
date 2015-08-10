using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Maui.Trading.Binding
{
    public abstract class AbstractDataSourceFactory : IDataSourceFactory
    {
        public abstract bool CanCreate( string name, Type type );

        /// <summary>
        /// Can be used by factories in "CanCreate" if the factory supports
        /// all default data source types.
        /// </summary>
        protected bool IsDefaultDataSource( Type type )
        {
            var SupportedDataSourceTypes = new[]
                {
                    typeof( ISingleDataSource<> ) ,
                    typeof( IEnumerableDataSource<> )
                };

            var typeDef = GetComparableType( type );
            return SupportedDataSourceTypes.Contains( typeDef );
        }

        protected Type GetComparableType( Type type )
        {
            return type.IsGenericType ? type.GetGenericTypeDefinition() : type;
        }

        public T Create<T>( string name, NamedParameters ctorArgs )
        {
            return (T)Create( name, typeof( T ),ctorArgs );
        }

        public object Create( string name, Type type, NamedParameters ctorArgs )
        {
            if ( !CanCreate( name, type ) )
            {
                throw new ArgumentException( "Cannot create DataSource from: " + type );
            }

            return CreateDataSource( name, type, ctorArgs );
        }

        protected abstract object CreateDataSource( string name, Type type, NamedParameters ctorArgs );
    }
}
