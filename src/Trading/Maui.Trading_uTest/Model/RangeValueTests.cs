using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Maui.Trading.Model;

namespace Maui.Trading.UnitTests.Model
{
    [TestFixture]
    public class RangeValueTests
    {
        [Test]
        public void Ctor_WithoutValue_ValueSetToMin()
        {
            var value = new RangeValue( 0, 100 );

            Assert.That( value.Value, Is.EqualTo( 0 ) );
        }

        [Test]
        public void Ctor_ValueLessThanMin_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>( () => new RangeValue( 0, 100, -1 ) );
        }

        [Test]
        public void Ctor_ValueGreaterThanMax_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>( () => new RangeValue( 0, 100, 101 ) );
        }

        [Test]
        public void RelativeValue_WhenCalled_ReturnsOffsetToMin()
        {
            var value = new RangeValue( 100, 200, 150 );

            Assert.That( value.RelativeValue, Is.EqualTo( 50 ) );
        }

        [Test]
        public void OperatorMultiply_WhenCalled_ReturnsNewInstance()
        {
            var value = new RangeValue( 100, 200, 150 );

            var multipliedValue = value * 0.5;

            Assert.IsFalse( object.ReferenceEquals( value, multipliedValue ) );
        }

        [Test]
        public void OperatorMultiply_WhenCalled_RelativeValueIsMultiplied()
        {
            var value = new RangeValue( 100, 200, 150 );

            var multipliedValue = value * 0.5;

            Assert.That( multipliedValue.RelativeValue, Is.EqualTo( 25 ) );
        }

        [Test]
        public void OperatorAdd_WhenCalled_ReturnsNewInstance()
        {
            var value = new RangeValue( 100, 200, 150 );

            var multipliedValue = value + 1;

            Assert.IsFalse( object.ReferenceEquals( value, multipliedValue ) );
        }

        [Test]
        public void OperatorAdd_WhenCalled_SummandAdded()
        {
            var value = new RangeValue( 100, 200, 150 );

            var multipliedValue = value + 7;

            Assert.That( multipliedValue.RelativeValue, Is.EqualTo( 57 ) );
        }

        [Test]
        public void Includes_ArgumentInsideRange_ReturnsTrue()
        {
            var value = new RangeValue( 100, 200 );

            Assert.IsTrue( value.Includes( 133 ) );
        }

        [Test]
        public void Includes_ArgumentOutsideRange_ReturnsFalse()
        {
            var value = new RangeValue( 100, 200 );

            Assert.IsFalse( value.Includes( 33 ) );
        }

        [Test]
        public void Equals_WithSimilarRangeValue_ReturnsTrue()
        {
            var rangeValue1 = new RangeValue( 0, 10, 4 );
            var rangeValue2 = new RangeValue( 0, 10, 4 );

            Assert.IsTrue( rangeValue1.Equals( rangeValue2 ) );
        }

        [Test]
        public void Equals_WithDifferentRangeValue_ReturnsFalse()
        {
            var rangeValue1 = new RangeValue( 0, 10, 4 );
            var rangeValue2 = new RangeValue( 0, 10, 5 );

            Assert.IsFalse( rangeValue1.Equals( rangeValue2 ) );
        }

        [Test]
        public void EqualsOperator_WithSimilarRangeValue_ReturnsTrue()
        {
            var rangeValue1 = new RangeValue( 0, 10, 4 );
            var rangeValue2 = new RangeValue( 0, 10, 4 );

            Assert.IsTrue( rangeValue1 == rangeValue2 );
        }

        [Test]
        public void EqualsOperator_WithDifferentRangeValue_ReturnsFalse()
        {
            var rangeValue1 = new RangeValue( 0, 10, 4 );
            var rangeValue2 = new RangeValue( 0, 10, 5 );

            Assert.IsFalse( rangeValue1 == rangeValue2 );
        }
    }
}
