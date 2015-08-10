using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Maui.Trading.Model;

namespace Maui.Trading.UnitTests.Model
{
    [TestFixture]
    public class PercentageTests
    {
        [Test]
        public void Ctor_WhenCalled_MinIsZero()
        {
            var value = new Percentage( 0 );

            Assert.That( value.Min, Is.EqualTo( 0 ) );
        }

        [Test]
        public void Ctor_WhenCalled_MaxIsOneHundret()
        {
            var value = new Percentage( 0 );

            Assert.That( value.Max, Is.EqualTo( 100 ) );
        }

        [Test]
        public void Compare_ValueOfLhsIsSmaller_ReturnsMinusOne()
        {
            var lhs = new Percentage( 1 );
            var rhs = new Percentage( 2 );

            int compare = lhs.CompareTo( rhs );

            Assert.That( compare, Is.EqualTo( -1 ) );
        }

        [Test]
        public void Compare_ValueOfLhsIsGreater_ReturnsOne()
        {
            var lhs = new Percentage( 2 );
            var rhs = new Percentage( 1 );

            int compare = lhs.CompareTo(  rhs );

            Assert.That( compare, Is.EqualTo( 1 ) );
        }

        [Test]
        public void Compare_ValueOfLhsAndRhsAreEqual_ReturnsZero()
        {
            var lhs = new Percentage( 1 );
            var rhs = new Percentage( 1 );

            int compare = lhs.CompareTo( rhs );

            Assert.That( compare, Is.EqualTo( 0 ) );
        }

        [Test]
        public void EqualsOperator_WithSimilarRangeValue_ReturnsTrue()
        {
            var rangeValue1 = new Percentage( 4 );
            var rangeValue2 = new Percentage( 4 );

            Assert.IsTrue( rangeValue1 == rangeValue2 );
        }

        [Test]
        public void EqualsOperator_WithDifferentRangeValue_ReturnsFalse()
        {
            var rangeValue1 = new Percentage( 4 );
            var rangeValue2 = new Percentage( 5 );

            Assert.IsFalse( rangeValue1 == rangeValue2 );
        }
    }
}
