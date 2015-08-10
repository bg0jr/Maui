using System;
using System.Linq;
using System.Data;
using System.Xml.Linq;
using Maui.Dynamics.Types;
using NUnit.Framework;
using Maui.Tasks.Dynamics;
using Maui.Dynamics.UnitTest;
using Maui.Dynamics;

namespace Maui.Tasks.UnitTests.Dynamics
{
    [TestFixture]
    public class AggregateTest : TestBase, IMslScript
    {
        [Test( Description = "Tests AggregateFunction with temp table" )]
        public void TempTable()
        {
            SetupInterpreter();

            Interpreter.Context.Scope.From = new DateTime( 2001, 12, 12 );
            Interpreter.Context.Scope.To = new DateTime( 2003, 12, 12 );

            var series = DefineTimeSeries( "dummy" );
            CreateTimeSeries( series, 12.2d, 13.3d, 14.4d, 15.5d, 16.6d );

            var t = this.Aggregate( series, values => values.Sum() );

            var rows = t.Manager().Query( CurrentStockId ).Rows.ToList();

            Assert.AreEqual( 1, rows.Count );
            Assert.AreEqual( 43.2d, rows[ 0 ][ "value" ] );
        }

        [Test( Description = "Tests AggregateFunction with result table" )]
        public void ResultTable()
        {
            SetupInterpreter();

            var t = DefineTimeSeries( "AggregateTable", "Aggregate" );
            t.Create();

            Interpreter.Context.Scope.From = new DateTime( 2001, 12, 12 );
            Interpreter.Context.Scope.To = new DateTime( 2003, 12, 12 );

            var series = DefineTimeSeries( "dummy" );
            CreateTimeSeries( series, 12.2d, 13.3d, 14.4d, 15.5d, 16.6d );

            this.Aggregate( series, t[ "Aggregate" ], values => values.Sum() );

            var rows = t.Manager().Query( CurrentStockId ).Rows.ToList();

            Assert.AreEqual( 1, rows.Count );
            Assert.AreEqual( 43.2d, rows[ 0 ][ "Aggregate" ] );
        }

        [Test( Description = "Tests AggregateFunction without current stock" )]
        [ExpectedException( typeof( Exception ), ExpectedMessage = "No current stock exists", MatchType = MessageMatch.Contains )]
        public void NoCurrentStock()
        {
            var t = DefineTimeSeries( "AggregateTable", "Aggregate" );
            t.Create();

            Interpreter.Context.Scope.From = new DateTime( 2001, 12, 12 );
            Interpreter.Context.Scope.To = new DateTime( 2003, 12, 12 );

            var series = DefineTimeSeries( "dummy" );
            CreateTimeSeries( series, 12.2d, 13.3d, 14.4d, 15.5d, 16.6d );

            this.Aggregate( series, values => values.Sum() );

            Assert.Fail();
        }

        private void SetupInterpreter()
        {
            AddDummyStock();
        }
    }
}
