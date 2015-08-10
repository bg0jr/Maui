using System;
using System.Data;
using System.Linq;
using Maui;
using Maui.Tasks.Dynamics;
using Maui.Entities;
using Maui.Dynamics.Data;
using NUnit.Framework;
using Maui.Dynamics;
using Maui.Dynamics.UnitTest;

namespace Maui.Tasks.UnitTests.Dynamics
{
    [TestFixture]
    public class HighlowTaskTest : TestBase, IMslScript
    {
        [Test( Description = "Tests HighLowFunction with temp table" )]
        public void TempTable()
        {
            SetupInterpreter();

            Interpreter.Context.Scope.From = new DateTime( 2001, 1, 1 );
            Interpreter.Context.Scope.To = new DateTime( 2003, 12, 31 );

            var input = Interpreter.Context.TomScripting.GetManager( "stock_price" ).Schema;

            var t = this.HighLow( input[ "close" ], TimeGrouping.Year );

            var rows = t.Manager().Query( CurrentStockId ).Rows.ToList();

            Assert.AreEqual( 3, rows.Count );
            Assert.AreEqual( 13.1d, rows[ 0 ][ "low" ] );
            Assert.AreEqual( 14.1d, rows[ 1 ][ "low" ] );
            Assert.AreEqual( 15.1d, rows[ 2 ][ "low" ] );
            Assert.AreEqual( 13.3d, rows[ 0 ][ "high" ] );
            Assert.AreEqual( 14.3d, rows[ 1 ][ "high" ] );
            Assert.AreEqual( 15.3d, rows[ 2 ][ "high" ] );
        }

        [Test( Description = "Tests HighLowFunction with result table" )]
        public void ResultTable()
        {
            SetupInterpreter();

            Interpreter.Context.Scope.From = new DateTime( 2000, 1, 1 );
            Interpreter.Context.Scope.To = new DateTime( 2004, 12, 31 );

            var input = Interpreter.Context.TomScripting.GetManager( "stock_price" ).Schema;
            var t = DefineTimeSeries( "highlowTable", "high", "low" );

            this.HighLow( input[ "close" ], TimeGrouping.Year, t[ "high" ], t[ "low" ] );

            var rows = t.Manager().Query( CurrentStockId ).Rows.ToList();

            Assert.AreEqual( 5, rows.Count );
            Assert.AreEqual( 12.1d, rows[ 0 ][ "low" ] );
            Assert.AreEqual( 13.1d, rows[ 1 ][ "low" ] );
            Assert.AreEqual( 14.1d, rows[ 2 ][ "low" ] );
            Assert.AreEqual( 15.1d, rows[ 3 ][ "low" ] );
            Assert.AreEqual( 16.1d, rows[ 4 ][ "low" ] );
            Assert.AreEqual( 12.3d, rows[ 0 ][ "high" ] );
            Assert.AreEqual( 13.3d, rows[ 1 ][ "high" ] );
            Assert.AreEqual( 14.3d, rows[ 2 ][ "high" ] );
            Assert.AreEqual( 15.3d, rows[ 3 ][ "high" ] );
            Assert.AreEqual( 16.3d, rows[ 4 ][ "high" ] );
        }

        [Test( Description = "Tests HighLowFunction without current stock" )]
        [ExpectedException( typeof( Exception ), ExpectedMessage = "No current stock exists", MatchType = MessageMatch.Contains )]
        public void NoCurrentStock()
        {
            Interpreter.Context.Scope.From = new DateTime( 2001, 1, 1 );
            Interpreter.Context.Scope.To = new DateTime( 2003, 12, 31 );

            var input = new TableSchema( "stock_price",
                           new DataColumn( "traded_stock_id", typeof( long ) ),
                           new DataColumn( "date", typeof( string ) ),
                           new DataColumn( "close", typeof( double ) ) )
                           .Create();

            this.HighLow( input[ "close" ], TimeGrouping.Year );

            Assert.Fail();
        }

        private void SetupInterpreter()
        {
            AddDummyStock();

            TableSchema schema = new TableSchema( "stock_price",
                               new DataColumn( "traded_stock_id", typeof( long ) ),
                               new DataColumn( "date", typeof( string ) ),
                               new DataColumn( "close", typeof( double ) ) )
                               .Create();

            // we have a datum so we need the table now

            ScopedTable table = schema.Manager().Query( CurrentStockId );

            AddStockPrice( table, "2000-12-12 00:00", 12.2d );
            AddStockPrice( table, "2000-12-11 00:00", 12.1d );
            AddStockPrice( table, "2000-12-13 00:00", 12.3d );

            AddStockPrice( table, "2001-12-12 00:00", 13.2d );
            AddStockPrice( table, "2001-12-11 00:00", 13.1d );
            AddStockPrice( table, "2001-12-13 00:00", 13.3d );

            AddStockPrice( table, "2002-12-12 00:00", 14.2d );
            AddStockPrice( table, "2002-12-11 00:00", 14.1d );
            AddStockPrice( table, "2002-12-13 00:00", 14.3d );

            AddStockPrice( table, "2003-12-12 00:00", 15.2d );
            AddStockPrice( table, "2003-12-11 00:00", 15.1d );
            AddStockPrice( table, "2003-12-13 00:00", 15.3d );

            AddStockPrice( table, "2004-12-12 00:00", 16.2d );
            AddStockPrice( table, "2004-12-11 00:00", 16.1d );
            AddStockPrice( table, "2004-12-13 00:00", 16.3d );
        }

        private void AddStockPrice( ScopedTable table, string date, double value )
        {
            DataRow row = table.NewRow();
            row[ "traded_stock_id" ] = CurrentTradedStockId;
            row.SetDate( table.Schema, DateTime.Parse( date ) );
            row[ "close" ] = value;
            table.Add( row );
        }
    }
}
