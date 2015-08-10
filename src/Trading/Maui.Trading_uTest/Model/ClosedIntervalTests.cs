using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Maui.Trading.Model;

namespace Maui.Trading.UnitTests.Model
{
    [TestFixture]
    public class ClosedIntervalTests
    {
        [Test]
        public void Ctor_WhenCalled_MinMaxSet()
        {
            var interval = new ClosedInterval<int>( 1, 4 );

            Assert.That( interval.Min, Is.EqualTo( 1 ) );
            Assert.That( interval.Max, Is.EqualTo( 4 ) );
        }

        [Test]
        public void Includes_MinMaxAndValueEquals_ReturnsTrue()
        {
            var interval = new ClosedInterval<int>( 0, 0 );

            Assert.IsTrue( interval.Includes( 0 ) );
        }

        [Test]
        public void Equals_SameIntervals_ReturnTrue()
        {
            var interval1 = new ClosedInterval<int>( 1, 3 );
            var interval2 = new ClosedInterval<int>( 1, 3 );

            Assert.That( interval1, Is.EqualTo( interval2 ) );
        }

        [Test]
        public void Equals_DifferentIntervals_ReturnFalse()
        {
            var interval1 = new ClosedInterval<int>( 0, 3 );
            var interval2 = new ClosedInterval<int>( 1, 3 );

            Assert.That( interval1, Is.Not.EqualTo( interval2 ) );
        }
    }
}
