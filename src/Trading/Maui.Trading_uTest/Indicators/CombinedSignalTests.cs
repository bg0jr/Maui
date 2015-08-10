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
    public class CombinedSignalTests
    {
        [Test]
        public void Ctor_WhenCalled_SignalsAreSet()
        {
            var signals = new Signal[]
            {
                new BuySignal(),
                new SellSignal()
            };

            var combinedSignal = new DefensiveCombinedSignal( signals );

            Assert.That( combinedSignal.Signals, Is.EquivalentTo( signals ) );
        }

        [Test]
        public void Ctor_WithCombinedSignals_SignalsAreSet()
        {
            var signals = new Signal[]
            {
                new BuySignal(),
                new SellSignal()
            };

            var signalsToAdd = signals.Select( s => new DefensiveCombinedSignal( s ) );
            var combinedSignal = new DefensiveCombinedSignal( signalsToAdd );

            Assert.That( combinedSignal.Signals, Is.EquivalentTo( signals ) );
        }

        [Test]
        public void Weight_WhenCalled_AllInternalSignalsAreWeighted()
        {
            var signals = new Signal[]
            {
                new BuySignal( 20 ),
                new SellSignal( 50 )
            };
            var combinedSignal = new DefensiveCombinedSignal( signals );

            var weightedSignal = (CombinedSignal)combinedSignal.Weight( 0.5d );

            var weightedSignals = signals.Select( s => s.Weight( 0.5d ) ).ToList();
            Assert.That( weightedSignal.Signals, Is.EquivalentTo( weightedSignals ) );
        }

        [Test]
        public void Equals_WithSimilarSignal_ReturnsTrue()
        {
            var signal1 = new DefensiveCombinedSignal( new BuySignal( 4, 7 ) );
            var signal2 = new DefensiveCombinedSignal( new BuySignal( 4, 7 ) );

            Assert.IsTrue( signal1.Equals( signal2 ) );
        }

        [Test]
        public void Equals_WithDifferentSignal_ReturnsFalse()
        {
            var signal1 = new DefensiveCombinedSignal( new BuySignal( 4, 7 ) );
            var signal2 = new DefensiveCombinedSignal( new BuySignal( 4, 8 ) );

            Assert.IsFalse( signal1.Equals( signal2 ) );
        }
    }
}
