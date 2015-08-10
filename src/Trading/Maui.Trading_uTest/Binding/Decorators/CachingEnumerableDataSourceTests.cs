using Blade.Testing.Mocking;
using Maui.Trading.Binding;
using Maui.Trading.Binding.Decorators;
using Maui.Trading.UnitTests.Fakes;
using NUnit.Framework;

namespace Maui.Trading.UnitTests.Binding.Decorators
{
    [TestFixture]
    public class CachingEnumerableDataSourceTests : MockingTestBase
    {
        [Test]
        public void Create_SameStock_DataIsCached()
        {
            var dataSource = CreateDataSource();

            var data1 = dataSource.ForStock( TomEntityBuilder.CreateStockHandle( "a", "a" ) );
            var data2 = dataSource.ForStock( TomEntityBuilder.CreateStockHandle( "a", "a" ) );

            Assert.IsTrue( object.ReferenceEquals( data1, data2 ) );
        }

        [Test]
        public void Create_DifferentStock_NewDataCreated()
        {
            var dataSource = CreateDataSource();

            var data1 = dataSource.ForStock( TomEntityBuilder.CreateStockHandle( "a", "a" ) );
            var data2 = dataSource.ForStock( TomEntityBuilder.CreateStockHandle( "b", "b" ) );

            Assert.IsFalse( object.ReferenceEquals( data1, data2 ) );
        }

        private IEnumerableDataSource<double> CreateDataSource()
        {
            var realDataSource = new DoubleSetDataSource( "dummy" );
            return new CachingEnumerableDataSource<double>( realDataSource );
        }
    }
}
