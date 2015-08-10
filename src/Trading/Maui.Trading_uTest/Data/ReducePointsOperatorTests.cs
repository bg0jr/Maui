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
    public class ReducePointsOperatorTests
    {
        [Test]
        public void Apply_EmptySeries_EmptySeriesIsReturned()
        {
            var op = new ReducePointsOperator( 2 );

            var series = op.Apply( PriceSeries.Null );

            Assert.That( series.Any(), Is.False );
        }

        [Test]
        public void Apply_LessOrEqualPointsThanMax_SeriesRemainsUnchanged()
        {
            var op = new ReducePointsOperator( 2 );
            var monday = new SimplePrice( new DateTime( 2011, 1, 10 ), 12 );
            var thuesday = new SimplePrice( new DateTime( 2011, 1, 11 ), 23 );
            var input = new PriceSeries( SeriesIdentifier.Null, new[] { monday, thuesday } );

            var series = op.Apply( input );

            Assert.That( series, Is.EquivalentTo( input ) );
        }

        [Test]
        public void Apply_MorePointsThanMax_DataGroupedByAverage()
        {
            var op = new ReducePointsOperator( 2 );
            var monday = new SimplePrice( new DateTime( 2011, 1, 10 ), 10 );
            var thuesday = new SimplePrice( new DateTime( 2011, 1, 11 ), 20 );
            var weddnesday = new SimplePrice( new DateTime( 2011, 1, 12 ), 30 );
            var input = new PriceSeries( SeriesIdentifier.Null, new[] { monday, thuesday, weddnesday } );

            var series = op.Apply( input );

            Assert.That( series.Count, Is.EqualTo( 2 ) );
            Assert.That( series[ 0 ].Value, Is.EqualTo( 15 ) );
            Assert.That( series[ 1 ].Value, Is.EqualTo( 30 ) );
        }
    }
}
