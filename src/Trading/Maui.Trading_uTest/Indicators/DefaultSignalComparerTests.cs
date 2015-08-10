using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Maui.Trading.Indicators;

namespace Maui.Trading.UnitTests.Indicators
{
    [TestFixture]
    public class DefaultSignalComparerTests
    {
        [Test]
        public void Compare_BuyAndSell_ReturnsOne()
        {
            var comparer = new DefaultSignalComparer();

            int result = comparer.Compare( new BuySignal(), new SellSignal() );

            Assert.That( result, Is.EqualTo( 1 ) );
        }

        [Test]
        public void Compare_BuyAndNeutral_ReturnsOne()
        {
            var comparer = new DefaultSignalComparer();

            int result = comparer.Compare( new BuySignal(), new NeutralSignal() );

            Assert.That( result, Is.EqualTo( 1 ) );
        }

        [Test]
        public void Compare_NeutralAndSell_ReturnsOne()
        {
            var comparer = new DefaultSignalComparer();

            int result = comparer.Compare( new NeutralSignal(), new SellSignal() );

            Assert.That( result, Is.EqualTo( 1 ) );
        }

        [Test]
        public void Compare_DifferentStrength_GreaterStrengthIsGreater()
        {
            var comparer = new DefaultSignalComparer();

            int result = comparer.Compare( new BuySignal( 80 ), new BuySignal( 50 ) );

            Assert.That( result, Is.EqualTo( 1 ) );
        }

        [Test]
        public void Compare_DifferentQuality_GreaterQualityIsGreater()
        {
            var comparer = new DefaultSignalComparer();

            int result = comparer.Compare( new BuySignal( 80, 40 ), new BuySignal( 80, 10 ) );

            Assert.That( result, Is.EqualTo( 1 ) );
        }
    }
}
