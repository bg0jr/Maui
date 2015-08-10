using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Maui.Trading.Data;
using Maui.Trading.Model;

namespace Maui.Trading.UnitTests.Data
{
    [TestFixture]
    public class InterpolateMissingDatesOperatorTests
    {
        [Test]
        public void Apply_EmptySeries_EmptySeriesIsReturned()
        {
            var op = new InterpolateMissingDatesOperator();

            var series = op.Apply( PriceSeries.Null );

            Assert.That( series.Any(), Is.False );
        }

        [Test]
        public void Apply_NoDataForWeekend_NoDataInterpolatedForWeekend()
        {
            var op = new InterpolateMissingDatesOperator();
            var friday = new SimplePrice( new DateTime( 2011, 1, 7 ), 0 );
            var monday = new SimplePrice( new DateTime( 2011, 1, 10 ), 0 );
            var input = new PriceSeries( SeriesIdentifier.Null, new[] { friday, monday } );

            var series = op.Apply( input );

            Assert.That( series, Is.EquivalentTo( input ) );
        }

        [Test]
        public void Apply_DayMissing_DataFromDayBeforeIsTaken()
        {
            var op = new InterpolateMissingDatesOperator();
            var monday = new SimplePrice( new DateTime( 2011, 1, 10 ), 123 );
            var weddnesday = new SimplePrice( new DateTime( 2011, 1, 12 ), 456 );
            var input = new PriceSeries( SeriesIdentifier.Null, new[] { monday, weddnesday } );

            var series = op.Apply( input );

            Assert.That( series[ 1 ].Value, Is.EqualTo( 123 ) );
        }

        [Test]
        public void Apply_NoDatesMissing_SeriesRemainsUnchanged()
        {
            var op = new InterpolateMissingDatesOperator();
            var monday = new SimplePrice( new DateTime( 2011, 1, 10 ), 12 );
            var thuesday = new SimplePrice( new DateTime( 2011, 1, 11 ), 23 );
            var weddnesday = new SimplePrice( new DateTime( 2011, 1, 12 ), 34 );
            var input = new PriceSeries( SeriesIdentifier.Null, new[] { monday, thuesday, weddnesday } );

            var series = op.Apply( input );

            Assert.That( series, Is.EquivalentTo( input ) );
        }
    }
}
