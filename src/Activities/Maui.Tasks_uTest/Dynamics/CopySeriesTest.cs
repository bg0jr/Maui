using System;
using System.Data;
using System.Linq;
using Maui.Tasks.Dynamics;
using Maui.Dynamics.UnitTest.Mocks;
using NUnit.Framework;
using Maui.Dynamics.UnitTest;
using Maui.Dynamics;

namespace Maui.Tasks.UnitTests.Dynamics
{
    [TestFixture]
    public class CopySeriesTaskTest : TestBase,IMslScript
    {
        [Test( Description = "Tests CopySeriesFunction with temp table" )]
        public void TempTable()
        {
            SetupInterpreter();

            Interpreter.Context.Scope.From = new DateTime( 2000, 12, 12 );
            Interpreter.Context.Scope.To = new DateTime( 2004, 12, 12 );

            var input = DefineTimeSeries( "eps" );
            CreateTimeSeries( input, 12.2d, 13.3d, 14.4d, 15.5d, 16.6d );

            var series = this.CopySeries( input );

            var rows = series.Manager().Query( CurrentStockId ).Rows.ToList();

            Assert.AreEqual( 5, rows.Count );
            Assert.AreEqual( 12.2d, rows[ 0 ][ "value" ] );
            Assert.AreEqual( 13.3d, rows[ 1 ][ "value" ] );
            Assert.AreEqual( 14.4d, rows[ 2 ][ "value" ] );
            Assert.AreEqual( 15.5d, rows[ 3 ][ "value" ] );
            Assert.AreEqual( 16.6d, rows[ 4 ][ "value" ] );
        }

        [Test( Description = "Tests CopySeriesFunction with result table" )]
        public void ResultTable()
        {
            SetupInterpreter();

            Interpreter.Context.Scope.From = new DateTime( 2000, 12, 12 );
            Interpreter.Context.Scope.To = new DateTime( 2004, 12, 12 );

            var input = DefineTimeSeries( "eps" );
            CreateTimeSeries( input, 12.2d, 13.3d, 14.4d, 15.5d, 16.6d );

            var series = DefineTimeSeries( "epsTable", "eps" );
            series.Create();

            this.CopySeries( input, series["eps"] );

            var rows = series.Manager().Query( CurrentStockId ).Rows.ToList();

            Assert.AreEqual( 5, rows.Count );
            Assert.AreEqual( 12.2d, rows[ 0 ][ "eps" ] );
            Assert.AreEqual( 13.3d, rows[ 1 ][ "eps" ] );
            Assert.AreEqual( 14.4d, rows[ 2 ][ "eps" ] );
            Assert.AreEqual( 15.5d, rows[ 3 ][ "eps" ] );
            Assert.AreEqual( 16.6d, rows[ 4 ][ "eps" ] );
        }

        [Test( Description = "Tests CopySeriesFunction without current stock" )]
        [ExpectedException( typeof( Exception ), ExpectedMessage = "No current stock exists", MatchType = MessageMatch.Contains )]
        public void NoCurrentStock()
        {
            Interpreter.Context.Scope.From = new DateTime( 2000, 12, 12 );
            Interpreter.Context.Scope.To = new DateTime( 2004, 12, 12 );

            var input = DefineTimeSeries( "eps" );
            CreateTimeSeries( input, 12.2d, 13.3d, 14.4d, 15.5d, 16.6d );

            this.CopySeries( input );

            Assert.Fail();
        }

        private void SetupInterpreter()
        {
            AddDummyStock();
        }
    }
}
