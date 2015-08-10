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
    public class AbstractCombinedSignalCreatorTests
    {
        [Test]
        public void Create_EmptySignalsSet_EmptySignalsSetReturned()
        {
            var creator = new CombinedSignalCreator();

            var series = creator.Create( new ISignalSeries[] { } );

            Assert.That( series, Is.Empty );
        }

        [Test]
        public void Create_EachSetHasSignalForEachDate_AverageReturned()
        {
            var creator = new CombinedSignalCreator();
            var series1 = CreateSignalSeries( new TestSignal( 7, 80 ), new TestSignal( 8, 40 ) );
            var series2 = CreateSignalSeries( new TestSignal( 7, 40 ), new TestSignal( 8, 80 ) );

            var series = creator.Create( new[] { series1, series2 } );

            var expectedSignals = CreateSignalSeries( new TestSignal( 7, 60 ), new TestSignal( 8, 60 ) );
            Assert.That( series, Is.EquivalentTo( expectedSignals ) );
        }

        [Test]
        public void Create_OneSeriesHasMoreDataThenOther_NoneSignalsAreInterpolatedAndAverageReturned()
        {
            var creator = new CombinedSignalCreator();
            var series1 = CreateSignalSeries( new TestSignal( 7, 80 ), new TestSignal( 8, 40 ) );
            var series2 = CreateSignalSeries( new TestSignal( 7, 40 ) );

            var series = creator.Create( new[] { series1, series2 } );

            var expectedSignals = CreateSignalSeries( new TestSignal( 7, 60 ), new TestSignal( 8, 20 ) );
            Assert.That( series, Is.EquivalentTo( expectedSignals ) );
        }

        private ISignalSeries CreateSignalSeries( params TimedSignal[] signals )
        {
            var prices = new PriceSeries( new SeriesIdentifier( NullObjectIdentifier.Null, new ObjectDescriptor( DatumNames.Prices ) ), PriceSeries.Null );
            return new SignalSeries( prices, SeriesIdentifier.Null, signals );
        }

        private class CombinedSignalCreator : AbstractCombinedSignalCreator
        {
            public override Signal Create( IEnumerable<Signal> signals )
            {
                return new NeutralSignal( (int)signals.Average( s => s.Strength.Value ) );
            }
        }

        private class TestSignal : TimedSignal
        {
            public TestSignal( int day, int strength )
                : base( new DateTime( 2011, 1, day ), new NeutralSignal( strength ) )
            {
            }
        }
    }
}
