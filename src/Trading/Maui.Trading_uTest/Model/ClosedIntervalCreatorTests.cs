using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Maui.Trading.Model;

namespace Maui.Trading.UnitTests.Model
{
    [TestFixture]
    public class ClosedIntervalCreatorTests
    {
        [Test]
        public void FromMinMax()
        {
            var interval = ClosedInterval.FromMinMax( 1, 4 );

            Assert.That( interval.Min, Is.EqualTo( 1 ) );
            Assert.That( interval.Max, Is.EqualTo( 4 ) );
        }

        [Test]
        public void FromOffsetLength()
        {
            var interval = ClosedInterval.FromOffsetLength( 3, 3 );

            Assert.That( interval.Min, Is.EqualTo( 3 ) );
            Assert.That( interval.Max, Is.EqualTo( 5 ) );
        }

        [Test]
        public void FromOffsetLength_LengthIsNull_ReturnsEmptyInterval()
        {
            var interval = ClosedInterval.FromOffsetLength( 3, 0 );

            Assert.That( interval.IsEmpty, Is.True );
        }
    
        [Test]
        public void FromDynamic()
        {
            var set = new[] { 1, 2, 3, 4, 5, 6 };
            var interval = ClosedInterval.FromDynamic( set, v => v < 3, v => v < 6 );

            Assert.That( interval.Min, Is.EqualTo( 2 ) );
            Assert.That( interval.Max, Is.EqualTo( 4 ) );
        }
    }
}
