using System;
using Blade.Testing.Mocking;
using Blade.Testing.NMock;
using Maui.Trading.Binding;
using Maui.Trading.Binding.Decorators;
using NUnit.Framework;

namespace Maui.Trading.UnitTests.Binding.Decorators
{
    [TestFixture]
    public class CachingDataSourceFactoryTests : MockingTestBase
    {
        private static Type DefaultDataSourceType = typeof( ISingleDataSource<string> );

        [Test]
        public void Create_SameNameAndType_DataSourceIsCached()
        {
            var factory = CreateFactory();

            var dataSource1 = factory.Create( "a", DefaultDataSourceType, NamedParameters.Null );
            var dataSource2 = factory.Create( "a", DefaultDataSourceType, NamedParameters.Null );

            Assert.IsTrue( object.ReferenceEquals( dataSource1, dataSource2 ) );
        }

        [Test]
        public void Create_DifferentName_NewDataSourceCreated()
        {
            var factory = CreateFactory();

            var dataSource1 = factory.Create( "a", DefaultDataSourceType, NamedParameters.Null );
            var dataSource2 = factory.Create( "b", DefaultDataSourceType, NamedParameters.Null );

            Assert.IsFalse( object.ReferenceEquals( dataSource1, dataSource2 ) );
        }

        [Test]
        public void Create_SameNameDifferentType_NewDataSourceCreated()
        {
            var factory = CreateFactory();

            var dataSource1 = factory.Create( "a", DefaultDataSourceType, NamedParameters.Null );
            var dataSource2 = factory.Create( "a", typeof( ISingleDataSource<int> ), NamedParameters.Null );

            Assert.IsFalse( object.ReferenceEquals( dataSource1, dataSource2 ) );
        }

        private CachingDataSourceFactory CreateFactory()
        {
            var realFactory = Mockery.NewMock<IDataSourceFactory>(); ;
            Mockery.Return( realFactory.CanCreate( null, null ), true );

            NMock2.Stub.On( realFactory ).Method( "Create" )
                .Will( new LambdaAction( inv =>
                {
                    inv.Result = Mockery.NewMock( inv.Parameters[ 1 ] as Type );
                } ) );

            return new CachingDataSourceFactory( realFactory );
        }
    }
}
