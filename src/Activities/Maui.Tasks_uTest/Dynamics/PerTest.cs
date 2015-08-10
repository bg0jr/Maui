using System;
using System.Linq;
using System.Xml.Linq;
using Maui.Dynamics.Types;
using NUnit.Framework;
using Maui.Tasks.Dynamics;
using Maui.Dynamics.UnitTest;
using Maui.Dynamics;

namespace Maui.Tasks.UnitTests.Dynamics
{
    /// <summary>
    /// Its only a shortcut to CrossSeries so only a short test here.
    /// </summary>
    [TestFixture]
    public class PerTaskTest : TestBase, IMslScript
    {
        [Test( Description = "Tests <Per/> with result table" )]
        public void TempTable()
        {
            SetupInterpreter();

            Interpreter.Context.Scope.From = new DateTime( 2000, 12, 12 );
            Interpreter.Context.Scope.To = new DateTime( 2004, 12, 12 );

            var t1 = DefineTimeSeries( "t1" );
            CreateTimeSeries( t1, 10d, 5d, 2d, 2.5d, 1d );

            var t2 = DefineTimeSeries( "t2" );
            CreateTimeSeries( t2, 5d, 2.5d, 4d, 10d, 4d );

            var t = this.PriceEarningRatio( t1, t2 );

            var rows = t.Manager().Query( CurrentStockId ).Rows.ToList();

            Assert.AreEqual( 5, rows.Count );
            Assert.AreEqual( 2d, rows[ 0 ][ "value" ] );
            Assert.AreEqual( 2d, rows[ 1 ][ "value" ] );
            Assert.AreEqual( 0.50d, rows[ 2 ][ "value" ] );
            Assert.AreEqual( 0.25d, rows[ 3 ][ "value" ] );
            Assert.AreEqual( 0.25d, rows[ 4 ][ "value" ] );
        }

        [Test( Description = "Tests <Per/> with result table" )]
        public void ResultTable()
        {
            SetupInterpreter();

            Interpreter.Context.Scope.From = new DateTime( 2000, 12, 12 );
            Interpreter.Context.Scope.To = new DateTime( 2004, 12, 12 );

            var t1 = DefineTimeSeries( "t1" );
            CreateTimeSeries( t1, 10d, 5d, 2d, 2.5d, 1d );

            var t2 = DefineTimeSeries( "t2" );
            CreateTimeSeries( t2, 5d, 2.5d, 4d, 10d, 4d );

            var t = DefineTimeSeries( "tresult", "Per" );

            this.PriceEarningRatio( t1, t2, t[ "Per" ] );

            var rows = t.Manager().Query( CurrentStockId ).Rows.ToList();

            Assert.AreEqual( 5, rows.Count );
            Assert.AreEqual( 2d, rows[ 0 ][ "Per" ] );
            Assert.AreEqual( 2d, rows[ 1 ][ "Per" ] );
            Assert.AreEqual( 0.50d, rows[ 2 ][ "Per" ] );
            Assert.AreEqual( 0.25d, rows[ 3 ][ "Per" ] );
            Assert.AreEqual( 0.25d, rows[ 4 ][ "Per" ] );
        }

        private void SetupInterpreter()
        {
            AddDummyStock();
        }
    }
}
