using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Maui.Trading.Indicators;
using Maui.Trading.Model;

namespace Maui.Trading.UnitTests.Indicators
{
    [TestFixture]
    public class DefensiveCombinedSignalTests
    {
        [Test]
        public void Strength_Buy100Sell100_ReturnsNeutral50()
        {
            var combinedSignal = new DefensiveCombinedSignal( new BuySignal( 100 ), new SellSignal( 100 ) );

            var expectedSignal = new NeutralSignal( 50 );
            Assert.That( combinedSignal, Is.EqualTo( expectedSignal ) );
        }

        [Test]
        public void Strength_Buy100Sell50_ReturnsNeutral75()
        {
            var combinedSignal = new DefensiveCombinedSignal( new BuySignal( 100 ), new SellSignal( 50 ) );

            var expectedSignal = new NeutralSignal( 75 );
            Assert.That( combinedSignal, Is.EqualTo( expectedSignal ) );
        }

        [Test]
        public void Strength_Buy100Sell0_ReturnsNeutral100()
        {
            var combinedSignal = new DefensiveCombinedSignal( new BuySignal( 100 ), new SellSignal( 0 ) );

            var expectedSignal = new NeutralSignal( 100 );
            Assert.That( combinedSignal, Is.EqualTo( expectedSignal ) );
        }

        [Test]
        public void Strength_Buy100Neutral100_ReturnsBuy50()
        {
            var combinedSignal = new DefensiveCombinedSignal( new BuySignal( 100 ), new NeutralSignal( 100 ) );

            var expectedSignal = new BuySignal( 50 );
            Assert.That( combinedSignal, Is.EqualTo( expectedSignal ) );
        }

        [Test]
        public void Strength_Buy100Neutral50_ReturnsBuy25()
        {
            var combinedSignal = new DefensiveCombinedSignal( new BuySignal( 100 ), new NeutralSignal( 50 ) );

            var expectedSignal = new BuySignal( 25 );
            Assert.That( combinedSignal, Is.EqualTo( expectedSignal ) );
        }

        [Test]
        public void Strength_Buy100Neutral0_ReturnsNeutral100()
        {
            var combinedSignal = new DefensiveCombinedSignal( new BuySignal( 100 ), new NeutralSignal( 0 ) );

            var expectedSignal = new NeutralSignal( 100 );
            Assert.That( combinedSignal, Is.EqualTo( expectedSignal ) );
        }

        [Test]
        public void Strength_Buy50Sell100_ReturnsNeutral25()
        {
            var combinedSignal = new DefensiveCombinedSignal( new BuySignal( 50 ), new SellSignal( 100 ) );

            var expectedSignal = new NeutralSignal( 25 );
            Assert.That( combinedSignal, Is.EqualTo( expectedSignal ) );
        }

        [Test]
        public void Strength_Buy0Sell100_ReturnsSell0()
        {
            var combinedSignal = new DefensiveCombinedSignal( new BuySignal( 0 ), new SellSignal( 100 ) );

            var expectedSignal = new SellSignal( 0 );
            Assert.That( combinedSignal, Is.EqualTo( expectedSignal ) );
        }

        [Test]
        public void Strength_Sell100Neutral100_ReturnsSell0()
        {
            var combinedSignal = new DefensiveCombinedSignal( new SellSignal( 100 ), new NeutralSignal( 100 ) );

            var expectedSignal = new SellSignal( 0 );
            Assert.That( combinedSignal, Is.EqualTo( expectedSignal ) );
        }

        [Test]
        public void Strength_Sell100Neutral50_ReturnsSell50()
        {
            var combinedSignal = new DefensiveCombinedSignal( new SellSignal( 100 ), new NeutralSignal( 50 ) );

            var expectedSignal = new SellSignal( 25 );
            Assert.That( combinedSignal, Is.EqualTo( expectedSignal ) );
        }

        [Test]
        public void Strength_Sell100Neutral0_ReturnsSell100()
        {
            var combinedSignal = new DefensiveCombinedSignal( new SellSignal( 100 ), new NeutralSignal( 0 ) );

            var expectedSignal = new SellSignal( 50 );
            Assert.That( combinedSignal, Is.EqualTo( expectedSignal ) );
        }

        [Test]
        public void Strength_Buy50Neutral100_ReturnsBuy25()
        {
            var combinedSignal = new DefensiveCombinedSignal( new BuySignal( 50 ), new NeutralSignal( 100 ) );

            var expectedSignal = new BuySignal( 25 );
            Assert.That( combinedSignal, Is.EqualTo( expectedSignal ) );
        }

        [Test]
        public void Strength_Buy0Neutral100_ReturnsNeutral100()
        {
            var combinedSignal = new DefensiveCombinedSignal( new BuySignal( 0 ), new NeutralSignal( 100 ) );

            var expectedSignal = new NeutralSignal( 100 );
            Assert.That( combinedSignal, Is.EqualTo( expectedSignal ) );
        }

        [Test]
        public void Strength_Sell50Neutral100_ReturnsNeutral25()
        {
            var combinedSignal = new DefensiveCombinedSignal( new SellSignal( 50 ), new NeutralSignal( 100 ) );

            var expectedSignal = new NeutralSignal( 25 );
            Assert.That( combinedSignal, Is.EqualTo( expectedSignal ) );
        }

        [Test]
        public void Strength_Sell0Neutral100_ReturnsNeutral50()
        {
            var combinedSignal = new DefensiveCombinedSignal( new SellSignal( 0 ), new NeutralSignal( 100 ) );

            var expectedSignal = new NeutralSignal( 50 );
            Assert.That( combinedSignal, Is.EqualTo( expectedSignal ) );
        }
    }
}
