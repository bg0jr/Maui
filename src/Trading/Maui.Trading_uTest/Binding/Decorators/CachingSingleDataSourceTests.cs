using Blade.Testing.Mocking;
using Maui.Trading.Binding;
using Maui.Trading.Binding.Decorators;
using Maui.Trading.UnitTests.Fakes;
using NUnit.Framework;

namespace Maui.Trading.UnitTests.Binding.Decorators
{
    [TestFixture]
    public class CachingSingleDataSourceTests : MockingTestBase
    {
        [Test]
        public void Create_SameStock_DataIsCached()
        {
            var dataSource = CreateDataSource();

            var data1 = dataSource.ForStock( TomEntityBuilder.CreateStockHandle( "a", "a" ) );
            var data2 = dataSource.ForStock( TomEntityBuilder.CreateStockHandle( "a", "a" ) );

            Assert.That( data1, Is.EqualTo( data2 ) );
        }

        [Test]
        public void Create_DifferentStock_NewDataCreated()
        {
            var dataSource = CreateDataSource();

            var data1 = dataSource.ForStock( TomEntityBuilder.CreateStockHandle( "a", "a" ) );
            var data2 = dataSource.ForStock( TomEntityBuilder.CreateStockHandle( "b", "b" ) );

            Assert.That( data1, Is.Not.EqualTo( data2 ) );
        }

        private ISingleDataSource<double> CreateDataSource()
        {
            var realDataSource = new FakeSingleDataSource<double>( "dummy" );
            realDataSource.Data[ "a" ] = 1;
            realDataSource.Data[ "b" ] = 2;

            return new CachingSingleDataSource<double>( realDataSource );
        }
    }
}
