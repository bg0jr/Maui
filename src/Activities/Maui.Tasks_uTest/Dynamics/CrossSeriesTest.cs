using System;
using System.Linq;
using NUnit.Framework;
using Maui.Tasks.Dynamics;
using Maui.Dynamics.UnitTest;
using Maui.Dynamics;

namespace Maui.Tasks.UnitTests.Dynamics
{
    [TestFixture]
    public class CrossSeriesTaskTest : TestBase, IMslScript
    {
        [Test( Description = "Tests CrossSeriesFunction with temp table" )]
        public void TempTable()
        {
            SetupInterpreter();

            Interpreter.Context.Scope.From = new DateTime( 2001, 12, 12 );
            Interpreter.Context.Scope.To = new DateTime( 2003, 12, 12 );

            var t1 = DefineTimeSeries( "t1" );
            CreateTimeSeries( t1, 10d, 5d, 2d, 2.5d, 1d );

            var t2 = DefineTimeSeries( "t2" );
            CreateTimeSeries( t2, 5d, 2.5d, 4d, 10d, 4d );

            var t = this.CrossSeries( t1, t2, ( left, right ) => left / right * 100 );

            var rows = t.Manager().Query( CurrentStockId ).Rows.ToList();

            Assert.AreEqual( 3, rows.Count );
            Assert.AreEqual( 200d, rows[ 0 ][ "value" ] );
            Assert.AreEqual( 50d, rows[ 1 ][ "value" ] );
            Assert.AreEqual( 25d, rows[ 2 ][ "value" ] );
        }

        [Test( Description = "Tests CrossSeriesFunction with result table" )]
        public void ResultTable()
        {
            SetupInterpreter();

            Interpreter.Context.Scope.From = new DateTime( 2000, 12, 12 );
            Interpreter.Context.Scope.To = new DateTime( 2004, 12, 12 );

            var t1 = DefineTimeSeries( "t1" );
            CreateTimeSeries( t1, 10d, 5d, 2d, 2.5d, 1d );

            var t2 = DefineTimeSeries( "t2" );
            CreateTimeSeries( t2, 5d, 2.5d, 4d, 10d, 4d );

            var t = DefineTimeSeries( "tresult", "v1", "v2" );

            this.CrossSeries( t1, t2, t[ "v1" ], ( left, right ) => left / right );
            // do it twice to check that v1 is not overwritten
            this.CrossSeries( t1, t2, t[ "v2" ], ( left, right ) => left / right );

            var rows = t.Manager().Query( CurrentStockId ).Rows.ToList();

            Assert.AreEqual( 5, rows.Count );

            Assert.AreEqual( 2d, rows[ 0 ][ "v1" ] );
            Assert.AreEqual( 2d, rows[ 1 ][ "v1" ] );
            Assert.AreEqual( 0.5d, rows[ 2 ][ "v1" ] );
            Assert.AreEqual( 0.25d, rows[ 3 ][ "v1" ] );
            Assert.AreEqual( 0.25d, rows[ 4 ][ "v1" ] );

            Assert.AreEqual( 2d, rows[ 0 ][ "v2" ] );
            Assert.AreEqual( 2d, rows[ 1 ][ "v2" ] );
            Assert.AreEqual( 0.5d, rows[ 2 ][ "v2" ] );
            Assert.AreEqual( 0.25d, rows[ 3 ][ "v2" ] );
            Assert.AreEqual( 0.25d, rows[ 4 ][ "v2" ] );
        }

        [Test( Description = "Tests CrossSeriesFunction without current stock" )]
        [ExpectedException( typeof( Exception ), ExpectedMessage = "No current stock exists", MatchType = MessageMatch.Contains )]
        public void NoCurrentStock()
        {
            Interpreter.Context.Scope.From = new DateTime( 2001, 12, 12 );
            Interpreter.Context.Scope.To = new DateTime( 2003, 12, 12 );

            var t1 = DefineTimeSeries( "t1" );
            CreateTimeSeries( t1, 10d, 5d, 2d, 2.5d, 1d );

            var t2 = DefineTimeSeries( "t2" );
            CreateTimeSeries( t2, 5d, 2.5d, 4d, 10d, 4d );

            var t = this.CrossSeries( t1, t2, ( left, right ) => left / right * 100 );

            Assert.Fail();
        }

        private void SetupInterpreter()
        {
            AddDummyStock();
        }
    }
}
