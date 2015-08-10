using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Maui.Trading.Model;
using Maui.Trading.UnitTests.Fakes;

namespace Maui.Trading.UnitTests.Model
{
    [TestFixture]
    public class SeriesRangeTests
    {
        [Test]
        public void Ctor_EmptySet_CountIsZero()
        {
            var range = new SeriesRange<int, int>( TimedValueSeries<int, int>.Null, ClosedInterval.FromOffsetLength( 1, 2 ) );

            Assert.That( range, Is.Empty );
        }

        [Test]
        public void GetEnumerator_ValidSeriesAndInterval_ReturnsValidRange()
        {
            var series = CreateSeries( new TV( 1, 1 ), new TV( 2, 2 ), new TV( 3, 3 ), new TV( 4, 4 ), new TV( 5, 5 ) );

            var range = new SeriesRange<int, int>( series, ClosedInterval.FromOffsetLength( 1, 2 ) );

            var expectedRange = new[] { new TV( 2, 2 ), new TV( 3, 3 ) };
            Assert.That( range, Is.EquivalentTo( expectedRange ) );
        }

        [Test]
        public void Count_ValidSeriesAndInterval_ReturnsValidCount()
        {
            var series = CreateSeries( new TV( 1, 1 ), new TV( 2, 2 ), new TV( 3, 3 ), new TV( 4, 4 ), new TV( 5, 5 ) );

            var range = new SeriesRange<int, int>( series, ClosedInterval.FromOffsetLength( 1, 2 ) );

            Assert.That( range.Count, Is.EqualTo( 2 ) );
        }

        [Test]
        public void GetValue_WithIndex_ReturnsValidValue()
        {
            var series = CreateSeries( new TV( 1, 1 ), new TV( 2, 2 ), new TV( 3, 3 ), new TV( 4, 4 ), new TV( 5, 5 ) );

            var range = new SeriesRange<int, int>( series, ClosedInterval.FromOffsetLength( 1, 2 ) );

            Assert.That( range[ 1 ].Value, Is.EqualTo( 3 ) );
        }

        private ITimedValueSeries<int, int> CreateSeries( params TimedValue<int, int>[] data )
        {
            return new TimedValueSeries<int, int>( SeriesIdentifier.Null, data );
        }
    }
}
