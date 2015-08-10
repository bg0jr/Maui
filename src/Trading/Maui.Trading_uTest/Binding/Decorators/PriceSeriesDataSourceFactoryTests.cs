using System.Linq;
using Blade.Testing.Mocking;
using Maui.Trading.Binding;
using Maui.Trading.Binding.Decorators;
using Maui.Trading.Data;
using Maui.Trading.UnitTests.Fakes;
using NUnit.Framework;

namespace Maui.Trading.UnitTests.Binding.Decorators
{
    [TestFixture]
    public class PriceSeriesDataSourceFactoryTests : MockingTestBase
    {
        [Test]
        public void CanCreate_WhenCalled_ByPassNonPriceDataSources()
        {
            var factory = CreateFactory();

            bool canCreate = factory.CanCreate( "a", typeof( string ) );

            Assert.That( canCreate, Is.True );
        }

        [Test]
        public void Create_OperatorsAvailable_OperatorsForDataSourcePassedToDataSource()
        {
            var factory = CreateFactory();
            factory.AddOperator( "a", Mockery.NewMock<IPriceSeriesOperator>() );
            factory.AddOperator( "a", Mockery.NewMock<IPriceSeriesOperator>() );
            factory.AddOperator( "b", Mockery.NewMock<IPriceSeriesOperator>() );

            var dataSource = factory.Create( "a", typeof( IPriceSeriesDataSource ), NamedParameters.Null ) as PriceSeriesDataSource;

            Assert.That( dataSource.Operators.Count(), Is.EqualTo( 2 ) );
        }

        [Test]
        public void Create_GlobalOperatorsAvailable_OperatorsPassedToDataSource()
        {
            var factory = CreateFactory();
            factory.AddOperator( Mockery.NewMock<IPriceSeriesOperator>() );
            factory.AddOperator( Mockery.NewMock<IPriceSeriesOperator>() );

            var dataSource = factory.Create( "a", typeof( IPriceSeriesDataSource ), NamedParameters.Null ) as PriceSeriesDataSource;

            Assert.That( dataSource.Operators.Count(), Is.EqualTo( 2 ) );
        }

        private PriceSeriesDataSourceFactory CreateFactory()
        {
            var realFactory = new FakeDataSourceFactory( Mockery );
            return new PriceSeriesDataSourceFactory( realFactory );
        }
    }
}
