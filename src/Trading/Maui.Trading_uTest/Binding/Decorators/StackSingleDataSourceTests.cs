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
    public class StackSingleDataSourceTests : MockingTestBase
    {
        private StockHandle myDummyStock = TomEntityBuilder.CreateStockHandle( "a", "b", "Euro" );

        [Test]
        public void ForStock_FirstDataSourceHasValue_StopsWithFirstDataSource()
        {
            var ds1 = CreateDataSource( NMock2.Return.Value( new SimplePrice( DateTime.Now, 34 ) ) );
            var ds2 = CreateDataSource( NMock2.Throw.Exception( new Exception() ) );

            var dataSource = CreateStackDataSource( ds1, ds2 );

            var result = dataSource.ForStock( myDummyStock );

            Assert.That( result.Value, Is.EqualTo( 34 ) );
        }

        [Test]
        public void ForStock_FirstDataSourceHasNoValue_ReturnsValueOfSecondDataSource()
        {
            var ds1 = CreateDataSource( NMock2.Return.Value( null ) );
            var ds2 = CreateDataSource( NMock2.Return.Value( new SimplePrice( DateTime.Now, 11 ) ) );

            var dataSource = CreateStackDataSource( ds1, ds2 );

            var result = dataSource.ForStock( myDummyStock );

            Assert.That( result.Value, Is.EqualTo( 11 ) );
        }

        private ISingleDataSource<SimplePrice> CreateDataSource( NMock2.IAction returnAction )
        {
            var ds = Mockery.NewMock<ISingleDataSource<SimplePrice>>();
            NMock2.Stub.On( ds ).Method( "ForStock" ).Will( returnAction );

            return ds;
        }

        private ISingleDataSource<SimplePrice> CreateStackDataSource( params ISingleDataSource<SimplePrice>[] innerDataSources )
        {
            return new StackSingleDataSource<SimplePrice>( "Prices", innerDataSources );
        }
    }
}
