using System;
using Blade.Testing.Mocking;
using Maui.Entities;
using Maui.Trading.Binding;
using Maui.Trading.Binding.Decorators;
using Maui.Trading.Model;
using Maui.Trading.UnitTests.Fakes;
using NUnit.Framework;

namespace Maui.Trading.UnitTests.Binding.Decorators
{
    [TestFixture]
    public class StackIEnumerableDataSourceTests : MockingTestBase
    {
        private StockHandle myDummyStock = TomEntityBuilder.CreateStockHandle( "a", "b", "Euro" );

        [Test]
        public void ForStock_FirstDataSourceHasValue_StopsWithFirstDataSource()
        {
            var data = new[] { new SimplePrice( DateTime.Now, 34 ) };
            var ds1 = CreateDataSource( NMock2.Return.Value( data ) );
            var ds2 = CreateDataSource( NMock2.Throw.Exception( new Exception() ) );

            var dataSource = CreateStackDataSource( ds1, ds2 );

            var result = dataSource.ForStock( myDummyStock );

            Assert.That( result, Is.EquivalentTo( data ) );
        }

        [Test]
        public void ForStock_FirstDataSourceHasNoValue_ReturnsValueOfSecondDataSource()
        {
            var data = new[] { new SimplePrice( DateTime.Now, 11 ) };
            var ds1 = CreateDataSource( NMock2.Return.Value( null ) );
            var ds2 = CreateDataSource( NMock2.Return.Value( data ) );

            var dataSource = CreateStackDataSource( ds1, ds2 );

            var result = dataSource.ForStock( myDummyStock );

            Assert.That( result, Is.EquivalentTo( data ) );
        }

        private IEnumerableDataSource<SimplePrice> CreateDataSource( NMock2.IAction returnAction )
        {
            var ds = Mockery.NewMock<IEnumerableDataSource<SimplePrice>>();
            NMock2.Stub.On( ds ).Method( "ForStock" ).Will( returnAction );

            return ds;
        }

        private IEnumerableDataSource<SimplePrice> CreateStackDataSource( params IEnumerableDataSource<SimplePrice>[] innerDataSources )
        {
            return new StackEnumerableDataSource<SimplePrice>( "Prices", innerDataSources );
        }
    }
}
