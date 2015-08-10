using System;
using System.Linq;
using NUnit.Framework;
using Maui.Tasks.Dynamics;
using Maui.Dynamics.UnitTest;
using Maui.Dynamics;

namespace Maui.Tasks.UnitTests.Dynamics
{
    [TestFixture]
    public class GrowthTaskTest : TestBase, IMslScript
    {
        [Test( Description = "Tests GrowthFunction with temp table" )]
        public void TempTable()
        {
            SetupInterpreter();

            Interpreter.Context.Scope.From = new DateTime( 2000, 12, 12 );
            Interpreter.Context.Scope.To = new DateTime( 2002, 12, 12 );

            var t1 = DefineTimeSeries( "t1" );
            CreateTimeSeries( t1, 2.2d, 3.3d, 2.4d, 5.5d, 1.6d );

            var t = this.Growth( t1 );

            var rows = t.Manager().Query( CurrentStockId ).Rows.ToList();

            Assert.AreEqual( 3, rows.Count );
            Assert.AreEqual( 0d, rows[ 0 ][ "value" ] );
            Assert.AreEqual( 1.1d, (double)rows[ 1 ][ "value" ], 0.00001d );
            Assert.AreEqual( -0.9d, (double)rows[ 2 ][ "value" ], 0.00001d );
        }

        [Test( Description = "Tests GrowthFunction with result table" )]
        public void ResultTable()
        {
            SetupInterpreter();

            Interpreter.Context.Scope.From = new DateTime( 2000, 12, 12 );
            Interpreter.Context.Scope.To = new DateTime( 2004, 12, 12 );

            var t1 = DefineTimeSeries( "t1" );
            CreateTimeSeries( t1,2.2d, 3.3d, 2.4d, 5.5d, 1.6d );

            var t = DefineTimeSeries( "tresult", "growth" );

            this.Growth( t1,t["growth"] );

            var rows = t.Manager().Query( CurrentStockId ).Rows.ToList();

            Assert.AreEqual( 5, rows.Count );
            Assert.AreEqual( 0d, rows[ 0 ][ "growth" ] );
            Assert.AreEqual( 1.1d, (double)rows[ 1 ][ "growth" ], 0.00001d );
            Assert.AreEqual( -0.9d, (double)rows[ 2 ][ "growth" ], 0.00001d );
            Assert.AreEqual( 3.1d, (double)rows[ 3 ][ "growth" ], 0.00001d );
            Assert.AreEqual( -3.9d, (double)rows[ 4 ][ "growth" ], 0.00001d );
        }


        [Test( Description = "Tests GrowthFunction without current stock" )]
        [ExpectedException( typeof( Exception ), ExpectedMessage = "No current stock exists", MatchType = MessageMatch.Contains )]
        public void NoCurrentStock()
        {
            Interpreter.Context.Scope.From = new DateTime( 2000, 12, 12 );
            Interpreter.Context.Scope.To = new DateTime( 2002, 12, 12 );

            var t1 = DefineTimeSeries( "t1" );
            CreateTimeSeries( t1, 2.2d, 3.3d, 2.4d, 5.5d, 1.6d );

            this.Growth( t1 );

            Assert.Fail();
        }

        private void SetupInterpreter()
        {
            AddDummyStock();
        }
    }
}
