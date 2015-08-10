using System.Data;
using System.IO;
using System.Linq;
using Maui.Data.SQL;
using Maui;
using Maui.Entities;
using Maui.Dynamics.Data;
using NUnit.Framework;
using Maui.UnitTests.Entities;

namespace Maui.Dynamics.UnitTest
{
    public class TestBase : Maui.UnitTests.TestBase
    {
        protected Interpreter myInterpreter = null;
        protected ServiceProvider myServiceProvider = null;
        protected IDatabaseSC myDataAccess = null;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            myDataAccess = myMockery.NewMock<IDatabaseSC>();

            myServiceProvider = Engine.ServiceProvider;
            myServiceProvider.RegisterService( "TOM Database", myDataAccess );
            myServiceProvider.RegisterService( typeof( IEntityRepositoryFactory ), new FakeEntityRepositoryFactory() );

            myServiceProvider.ConfigurationSC().Import( Path.Combine( MauiHome, "config" ), true );

            ScriptingInterface tomScripting = new ScriptingInterface();
            tomScripting.Init( myServiceProvider );
            myServiceProvider.RegisterService( typeof( ScriptingInterface ), tomScripting );

            myInterpreter = new Interpreter();
            myInterpreter.Init( myServiceProvider );
            //myInterpreter.DumpErrorToConsole = true;
            myInterpreter.Start();
        }

        [TearDown]
        public override void TearDown()
        {
            Interpreter.Context.TomScripting.Dispose();
            myServiceProvider.Reset();

            myInterpreter = null;
            myServiceProvider = null;
            myDataAccess = null;

            base.TearDown();
        }

        protected void AddDummyStock()
        {
            AddDummyStock( "xyz" );
        }

        protected void AddDummyStock(string isin)
        {
            var company = new Company( "C" );
            var stock = new Stock( company, isin );
            var exchange = new StockExchange( "Xetra", "De", new Currency( "Euro", "Euro" ) );
            var tradedStock = new TradedStock( stock, exchange );

            Interpreter.Context.Scope.Stock = new StockHandle( tradedStock );
        }

        protected TableSchema DefineTimeSeries( string name )
        {
            return DefineTimeSeries( name, "value" );
        }

        protected TableSchema DefineTimeSeries( string name, params string[] valueCols )
        {
            return new TableSchema( name,
                new DataColumn[] 
                {
                   TableSchema.CreateReference( "stock" ),
                   new DataColumn( "year", typeof( int ) )
                }.Concat(
                    valueCols.Select( col => new DataColumn( col, typeof( double ) ) )
                ) )
                .Create();
        }

        protected void CreateTimeSeries( TableSchema table, params double[] values )
        {
            var dataTable = table.Manager().Query( CurrentStockId );

            int i = 0;
            foreach ( var value in values )
            {
                DataRow row = dataTable.NewRow();
                row[ "stock_id" ] = CurrentStockId;
                row[ "year" ] = 2000 + i++;
                row[ "value" ] = value;
                dataTable.Add( row );
            }
        }

        protected long CurrentTradedStockId
        {
            get
            {
                return Interpreter.Context.Scope.Stock.TradedStockId;
            }
        }

        protected long CurrentStockId
        {
            get
            {
                return Interpreter.Context.Scope.Stock.Stock.Id;
            }
        }
    }
}
