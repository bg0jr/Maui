using System;
using System.Collections.Generic;
using Blade.Testing.Mocking;
using Maui.Trading.Binding;
using Maui.Trading.Binding.Decorators;
using Maui.Trading.Data;
using Maui.Trading.Model;
using Maui.Trading.UnitTests.Fakes;
using NUnit.Framework;

namespace Maui.Trading.UnitTests.Binding.Decorators
{
    [TestFixture]
    public class PriceSeriesDataSourceTests : MockingTestBase
    {
        [Test]
        public void ForStock_WhenCalled_ProperSeriesIdentifierCreated()
        {
            var dataSource = CreateDataSource( "dummy" );

            var series = dataSource.ForStock( TomEntityBuilder.CreateStockHandle( "a", "a" ) );

            Assert.That( series.Identifier.Owner, Is.InstanceOf<StockObjectIdentifier>() );
            Assert.That( series.Identifier.Type.Name, Is.EqualTo( "dummy" ) );
        }

        [Test]
        public void ForStock_WhenCalled_DataIsSet()
        {
            var data = new List<SimplePrice>() { new SimplePrice( DateTime.Now, 0 ) };
            var dataSource = CreateDataSource( "dummy", data );

            var series = dataSource.ForStock( TomEntityBuilder.CreateStockHandle( "a", "a" ) );

            Assert.That( series, Is.EquivalentTo( data ) );
        }

        [Test]
        public void ForStock_WhenCalled_OperatorsAreApplied()
        {
            var data = new List<SimplePrice>() { new SimplePrice( DateTime.Now, 0 ) };
            var dataSource = CreateDataSource( "dummy", data, new ClearDataOperator() );

            var series = dataSource.ForStock( TomEntityBuilder.CreateStockHandle( "a", "a" ) );

            Assert.That( series, Is.Empty );
        }

        private IPriceSeriesDataSource CreateDataSource( string dataSourceName )
        {
            return CreateDataSource( dataSourceName, new List<SimplePrice>() );
        }

        private IPriceSeriesDataSource CreateDataSource( string dataSourceName, IEnumerable<SimplePrice> data )
        {
            return CreateDataSource( dataSourceName, data, new IPriceSeriesOperator[] { } );
        }

        private IPriceSeriesDataSource CreateDataSource( string dataSourceName, IEnumerable<SimplePrice> data, params IPriceSeriesOperator[] operators )
        {
            var realDataSource = Mockery.NewMock<IEnumerableDataSource<SimplePrice>>();

            Mockery.Return( realDataSource.Name, dataSourceName );
            Mockery.Return( realDataSource.ForStock( null ), data );

            return new PriceSeriesDataSource( realDataSource, operators );
        }

        private class ClearDataOperator : IPriceSeriesOperator
        {
            public IPriceSeries Apply( IPriceSeries series )
            {
                return PriceSeries.Null;
            }
        }
    }
}
