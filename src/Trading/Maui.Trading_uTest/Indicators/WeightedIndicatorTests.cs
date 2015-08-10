using System;
using Blade.Testing.Mocking;
using Maui.Trading.Indicators;
using NUnit.Framework;

namespace Maui.Trading.UnitTests.Indicators
{
    [TestFixture]
    public class WeightedIndicatorTests : MockingTestBase
    {
        [Test]
        public void Ctor_InvalidWeight_Throws( [Values( -1, 0, 2 )] double weight )
        {
            var innerIndicator = Mockery.NewMock<IIndicator>();

            Assert.Throws<ArgumentOutOfRangeException>( () => new WeightedIndicator( innerIndicator, weight ) );
        }

        [Test]
        public void Ctor_ValidWeight_WeigthPropertyIsSet( [Values( 0.1, 0.5, 1 )] double weight )
        {
            var innerIndicator = Mockery.NewMock<IIndicator>();

            var indicator = new WeightedIndicator( innerIndicator, weight );

            Assert.That( indicator.Weight, Is.EqualTo( weight ) );
        }
    }
}
