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
    public class SeriesViewportTests
    {
        [Test]
        public void Ctor_EmptySeries_ViewportIsEmpty()
        {
            var series = CreateSeries();

            var viewport = new SimpleSeriesViewport( series, ClosedInterval.FromOffsetLength( 0, 0 ) );

            Assert.That( viewport.Series, Is.Empty );
        }

        [Test]
        public void Ctor_WhenCalled_ViewportRangeIsApplied()
        {
            var series = CreateSeries( new TV( 1, 1 ), new TV( 2, 2 ), new TV( 3, 3 ), new TV( 4, 4 ) );

            var viewport = new SimpleSeriesViewport( series, ClosedInterval.FromOffsetLength( 2, 2 ) );

            var expectedSeries = new[] { new TV( 2, 2 ), new TV( 3, 3 ) };
            Assert.That( viewport.Series, Is.EquivalentTo( expectedSeries ) );
        }

        [Test]
        public void Ctor_WhenCalled_MinMaxCalculatedProperly()
        {
            var series = CreateSeries( new TV( 1, 1 ), new TV( 2, 2 ), new TV( 3, 3 ), new TV( 4, 4 ) );

            var viewport = new SimpleSeriesViewport( series, ClosedInterval.FromOffsetLength( 2, 2 ) );

            Assert.That( viewport.MinTime, Is.EqualTo( 2 ) );
            Assert.That( viewport.MinValue.Value, Is.EqualTo( 2 ) );
            Assert.That( viewport.MaxTime, Is.EqualTo( 3 ) );
            Assert.That( viewport.MaxValue.Value, Is.EqualTo( 3 ) );
        }

        [Test]
        public void SetViewport_WhenCalled_ViewportRangeIsApplied()
        {
            var series = CreateSeries( new TV( 1, 1 ), new TV( 2, 2 ), new TV( 3, 3 ), new TV( 4, 4 ) );

            var viewport = new SimpleSeriesViewport( series, ClosedInterval.FromMinMax( int.MinValue, int.MaxValue ) );
            viewport.ViewPort = ClosedInterval.FromOffsetLength( 2, 2 );

            var expectedSeries = new[] { new TV( 2, 2 ), new TV( 3, 3 ) };
            Assert.That( viewport.Series, Is.EquivalentTo( expectedSeries ) );
        }

        private ITimedValueSeries<int, int> CreateSeries( params TimedValue<int, int>[] data )
        {
            return new TimedValueSeries<int, int>( SeriesIdentifier.Null, data );
        }

        private class SimpleSeriesViewport : SeriesViewport<int, int, ITimedValueSeries<int, int>>
        {
            public SimpleSeriesViewport( ITimedValueSeries<int, int> series, ClosedInterval<int> range )
                : base( series, range )
            {
            }

            protected override ITimedValueSeries<int, int> CreateSeriesRange( ITimedValueSeries<int, int> series, ClosedInterval<int> interval )
            {
                return new SeriesRange<int, int>( series, interval );
            }
        }
    }
}
