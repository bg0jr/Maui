using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Maui.Trading.Indicators;

namespace Maui.Trading.UnitTests.Indicators
{
    [TestFixture]
    public class DefaultSignalsTests
    {
        [Test( Description = "reason: 50 = buy, 100 = strong buy" )]
        public void BuySignal_WhenCreated_StrengthIs50()
        {
            var signal = new BuySignal();

            Assert.That( signal.Strength.Value, Is.EqualTo( 50 ) );
        }

        [Test( Description = "reason: 50 = neutral, 100 = strong neutral" )]
        public void NeutralSignal_WhenCreated_StrengthIs50()
        {
            var signal = new NeutralSignal();

            Assert.That( signal.Strength.Value, Is.EqualTo( 50 ) );
        }
        
        [Test( Description = "reason: 50 =sell, 100 = strong sell" )]
        public void SellSignal_WhenCreated_StrengthIs50()
        {
            var signal = new SellSignal();

            Assert.That( signal.Strength.Value, Is.EqualTo( 50 ) );
        }
    }
}
