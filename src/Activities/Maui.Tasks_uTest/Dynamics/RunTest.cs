using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using Maui.Tasks.Dynamics;
using Maui.Dynamics.Types;
using Maui.Entities;
using Maui.Dynamics.Data;
using NUnit.Framework;
using Maui.Dynamics.UnitTest;
using Maui.Dynamics;

namespace Maui.Tasks.UnitTests.Dynamics
{
    [TestFixture]
    public class RunTaskTest : TestBase, IMslScript
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            // TODO: fix
            //Currency cur = new Currency( "Euro", "Euro" );
            //myMockery.Return( myTomInterface.CurrencyCollection.FindByName( string.Empty ), cur );
        }

        [Test( Description = "Tests RunFunction with simple model" )]
        public void RunModel()
        {
            var theValue = 2.5d;
            var result = 0d;

            Action Model = () =>
            {
                result = theValue;
            };

            AddDummyStock();

            Interpreter.Context.Scope.Catalog = new StockCatalog( "catalog" );
            Interpreter.Context.Scope.Catalog.TradedStocks.Add( Interpreter.Context.Scope.Stock.TradedStock );

            this.RunModel( Model );

            Assert.AreEqual( 2.5d, result );
        }

        [Test( Description = "Tests RunFunction with model and without catalog" )]
        [ExpectedException( typeof( Exception ), ExpectedMessage = "Catalog required", MatchType = MessageMatch.Contains )]
        public void RunModelWithoutCatalog()
        {
            var theValue = 2.5d;
            var result = 0d;

            Action Model = () =>
            {
                result = theValue;
            };

            this.RunModel( Model );
        }
    }
}
