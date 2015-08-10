using System;
using Blade.Testing.NMock;
using Maui.Trading.Binding;
using Maui.Trading.Model;

namespace Maui.Trading.UnitTests.Fakes
{
    public class FakeDataSourceFactory : IDataSourceFactory
    {
        private AutoMockery myMockery;

        public FakeDataSourceFactory( AutoMockery mockery = null )
        {
            myMockery = mockery;
        }

        public bool CanCreate( string name, Type type )
        {
            return true;
        }

        public T Create<T>( string name, NamedParameters ctorArgs )
        {
            return (T)Create( name, typeof( T ), ctorArgs );
        }

        public object Create( string name, Type type, NamedParameters ctorArgs )
        {
            if ( type == typeof( DoubleSetDataSource ) )
            {
                return new DoubleSetDataSource( name );
            }
            if ( type == typeof( IEnumerableDataSource<SimplePrice> ) )
            {
                return myMockery.NewMock<IEnumerableDataSource<SimplePrice>>();
            }
            else if ( type == typeof( ISingleDataSource<string> ) )
            {
                return new FakeSingleDataSource<string>( name );
            }
            else if ( type == typeof( ISingleDataSource<int> ) )
            {
                return new FakeSingleDataSource<int>( name );
            }
            else if ( type == typeof( ISingleDataSource<double> ) )
            {
                return new FakeSingleDataSource<double>( name );
            }

            throw new NotSupportedException( "DataSource type not supported: " + type );
        }
    }
}
