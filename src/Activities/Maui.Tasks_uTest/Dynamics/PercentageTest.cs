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
    public class PercentageTaskTest : TestBase, IMslScript
    {
        [Test( Description = "Tests <percentage/> with result table" )]
        public void TempTable()
        {
            SetupInterpreter();

            Interpreter.Context.Scope.From = new DateTime( 2000, 12, 12 );
            Interpreter.Context.Scope.To = new DateTime( 2004, 12, 12 );

            var t1 = DefineTimeSeries( "t1" );
            CreateTimeSeries( t1, 10d, 5d, 2d, 2.5d, 1d );

            var t2 = DefineTimeSeries( "t2" );
            CreateTimeSeries( t2, 5d, 2.5d, 4d, 10d, 4d );

            var t = this.Percentage( t1, t2 );

            var rows = t.Manager().Query( CurrentStockId ).Rows.ToList();

            Assert.AreEqual( 5, rows.Count );
            Assert.AreEqual( 200d, rows[ 0 ][ "value" ] );
            Assert.AreEqual( 200d, rows[ 1 ][ "value" ] );
            Assert.AreEqual( 50d, rows[ 2 ][ "value" ] );
            Assert.AreEqual( 25d, rows[ 3 ][ "value" ] );
            Assert.AreEqual( 25d, rows[ 4 ][ "value" ] );
        }

        [Test( Description = "Tests <percentage/> with result table" )]
        public void ResultTable()
        {
            SetupInterpreter();

            Interpreter.Context.Scope.From = new DateTime( 2000, 12, 12 );
            Interpreter.Context.Scope.To = new DateTime( 2004, 12, 12 );

            var t1 = DefineTimeSeries( "t1" );
            CreateTimeSeries( t1, 10d, 5d, 2d, 2.5d, 1d );

            var t2 = DefineTimeSeries( "t2" );
            CreateTimeSeries( t2, 5d, 2.5d, 4d, 10d, 4d );

            var t = DefineTimeSeries( "tresult", "percentage" );

            this.Percentage( t1, t2, t[ "percentage" ] );

            var rows = t.Manager().Query( CurrentStockId ).Rows.ToList();

            Assert.AreEqual( 5, rows.Count );
            Assert.AreEqual( 200d, rows[ 0 ][ "percentage" ] );
            Assert.AreEqual( 200d, rows[ 1 ][ "percentage" ] );
            Assert.AreEqual( 50d, rows[ 2 ][ "percentage" ] );
            Assert.AreEqual( 25d, rows[ 3 ][ "percentage" ] );
            Assert.AreEqual( 25d, rows[ 4 ][ "percentage" ] );
        }

        private void SetupInterpreter()
        {
            AddDummyStock();
        }
    }
}
