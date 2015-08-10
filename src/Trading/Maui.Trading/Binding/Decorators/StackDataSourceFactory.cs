using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Collections;
using Maui.Reflection;
using Blade.Reflection;

namespace Maui.Trading.Binding.Decorators
{
    public class StackDataSourceFactory : AbstractDataSourceFactory
    {
        public StackDataSourceFactory( params IDataSourceFactory[] factories )
        {
            Factories = factories;
        }

        public IEnumerable<IDataSourceFactory> Factories
        {
            get;
            private set;
        }

        public override bool CanCreate( string name, Type type )
        {
            return IsDefaultDataSource( type ) &&
                Factories.Any( f => f.CanCreate( name, type ) );
        }

        protected override object CreateDataSource( string name, Type type, NamedParameters ctorArgs )
        {
            var dataSources = (IList)typeof( List<> ).CreateGeneric( type );
            foreach ( var factory in Factories )
            {
                if ( factory.CanCreate( name, type ) )
                {
                    dataSources.Add( factory.Create( name, type, ctorArgs ) );
                }
            }

            var innerType = type.GetGenericArguments().First();
            if ( type.GetGenericTypeDefinition() == typeof( ISingleDataSource<> ) )
            {
                return typeof( StackSingleDataSource<> ).CreateGeneric( innerType, name, dataSources );
            }
            if ( type.GetGenericTypeDefinition() == typeof( IEnumerableDataSource<> ) )
            {
                return typeof( StackEnumerableDataSource<> ).CreateGeneric( innerType, name, dataSources );
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
