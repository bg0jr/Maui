using System;
using System.Data;
using System.Xml.Linq;
using Maui.Entities;
using Maui.Dynamics.Data;
using NUnit.Framework;

namespace Maui.Tasks.UnitTests.Dynamics
{
    //[TestFixture]
    //public class MauiFunctionsTest : TestBase
    //{
    //    [SetUp]
    //    public override void SetUp()
    //    {
    //        base.SetUp();

    //        myInterpreter.InterpreterStarting += delegate( object sender, EventArgs e )
    //        {
    //            AddDummyStock();
    //        };

    //        DatumOrigin origin = new DatumOrigin( "dummy" );
    //        myMockery.Return( myTomInterface.DatumOriginCollection.FindByName( null ), origin );
    //    }

    //    [Test( Description = "Tests maui::close with close price from DB" )]
    //    public void CloseFromDB()
    //    {
    //        myInterpreter.InterpreterStarting += delegate( object sender, EventArgs e )
    //        {
    //            TableSchema schema = new TableSchema( "stock_price",
    //                new DataColumn( "traded_stock_id", typeof( long ) ),
    //                new DataColumn( "date", typeof( string ) ),
    //                new DataColumn( "close", typeof( double ) ) );

    //            // we have a datum so we need the table now
    //            var mgr = myContext.Interpreter.TomScripting.GetManager( schema );
    //            mgr.CreateTable();

    //            ScopedTable table = mgr.Query( 1 );
    //            DataRow row = table.NewRow();
    //            row[ "traded_stock_id" ] = 1;
    //            row.SetDate( schema, DateTime.Now.GetMostRecentTradingDay() );
    //            row[ "close" ] = 25.25d;

    //            table.Add( row );
    //        };

    //        // do the test
    //        XElement script = new XElement( "script",
    //                        CreateVariable( "a", "double", "${maui::close()}" )
    //                        );

    //        RunAndValidate( script, "a", 25.25d );
    //    }

    //    [Test( Description = "Tests maui::close with close price from user" )]
    //    public void CloseFromUser()
    //    {
    //        myInterpreter.InterpreterStarting += delegate( object sender, EventArgs e )
    //        {
    //            TableSchema schema = new TableSchema( "stock_price",
    //                new DataColumn( "traded_stock_id", typeof( long ) ),
    //                new DataColumn( "date", typeof( string ) ),
    //                new DataColumn( "close", typeof( double ) ) );

    //            // we have a datum so we need the table now
    //            var mgr = myContext.Interpreter.TomScripting.GetManager( schema );
    //            mgr.CreateTable();
    //        };

    //        // do the test
    //        XElement script = new XElement( "script",
    //                        CreateVariable( "a", "double", "${maui::close()}" )
    //                        );

    //        RunAndValidate( script, "a", (double?)null );
    //    }
    //}
}
