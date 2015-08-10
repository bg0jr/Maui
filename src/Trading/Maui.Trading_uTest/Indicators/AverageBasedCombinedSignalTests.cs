using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Maui.Trading.Indicators;
using Maui.Trading.Model;

namespace Maui.Trading.UnitTests.Indicators
{
    /// <summary>
    /// We do only the tests of the base class here. Nevertheless we use a real implementation
    /// to avoid fake reimplementation of signal strength calculation.
    /// </summary>
    [TestFixture]
    public class AverageBasedCombinedSignalTests
    {
        [Test]
        public void Strength_BuySignalsOnly_AverageIsReturned()
        {
            var combinedSignal = new DefensiveCombinedSignal( new BuySignal( 20 ), new BuySignal( 40 ), new BuySignal( 60 ) );

            Assert.That( combinedSignal.Strength.Value, Is.EqualTo( 40 ) );
        }

        [Test]
        public void Strength_SellSignalsOnly_AverageIsReturned()
        {
            var combinedSignal = new DefensiveCombinedSignal( new SellSignal( 20 ), new SellSignal( 40 ), new SellSignal( 60 ) );

            Assert.That( combinedSignal.Strength.Value, Is.EqualTo( 40 ) );
        }

        [Test]
        public void Strength_NeutralSignalsOnly_AverageIsReturned()
        {
            var combinedSignal = new DefensiveCombinedSignal( new NeutralSignal( 20 ), new NeutralSignal( 40 ), new NeutralSignal( 60 ) );

            Assert.That( combinedSignal.Strength.Value, Is.EqualTo( 40 ) );
        }

        [Test]
        public void Strength_NoSignalsOnly_ReturnsZero()
        {
            var combinedSignal = new DefensiveCombinedSignal( Signal.None, Signal.None );

            Assert.That( combinedSignal.Strength.Value, Is.EqualTo( 0 ) );
        }

        [Test]
        public void Quality_WhenCalled_AverageIsReturned()
        {
            var combinedSignal = new DefensiveCombinedSignal( new BuySignal( 20, 20 ), new BuySignal( 40, 40 ), new BuySignal( 60, 60 ) );

            Assert.That( combinedSignal.Quality.Value, Is.EqualTo( 40 ) );
        }

        [Test]
        public void Quality_NoSignalsOnly_ReturnsZero()
        {
            var combinedSignal = new DefensiveCombinedSignal( Signal.None, Signal.None );

            Assert.That( combinedSignal.Quality.Value, Is.EqualTo( 0 ) );
        }
    }
}
