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
    public class SignalTests
    {
        [Test]
        public void Ctor_WithoutQuality_QualitySetTo100Percent()
        {
            var signal = new BuySignal( 45 );

            Assert.That( signal.Quality, Is.EqualTo( Percentage.Hundred ) );
        }

        [Test]
        public void Strength_ForSignalTypeNone_ReturnsZero()
        {
            var signal = Signal.None;

            Assert.That( signal.Strength, Is.EqualTo( Percentage.Zero ) );
        }

        [Test]
        public void Quality_ForSignalTypeNone_ReturnsZero()
        {
            var signal = Signal.None;

            Assert.That( signal.Quality, Is.EqualTo( Percentage.Zero ) );
        }

        [Test]
        public void Weigth_WhenCalled_SignalWithAdjustedStrengthIsReturned()
        {
            var signal = new BuySignal( 4 );

            var weightedSignal = signal.Weight( 0.5 );

            Assert.That( weightedSignal.Strength.Value, Is.EqualTo( 2 ) );
        }

        [Test]
        public void Equals_WithSimilarSignal_ReturnsTrue()
        {
            var signal1 = new BuySignal( 4, 7 );
            var signal2 = new BuySignal( 4, 7 );

            Assert.IsTrue( signal1.Equals( signal2 ) );
        }

        [Test]
        public void Equals_WithDifferentSignal_ReturnsFalse()
        {
            var signal1 = new BuySignal( 4, 7 );
            var signal2 = new BuySignal( 4, 8 );

            Assert.IsFalse( signal1.Equals( signal2 ) );
        }
    }
}
