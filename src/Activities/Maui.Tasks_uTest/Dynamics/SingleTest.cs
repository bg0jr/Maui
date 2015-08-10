using System;
using System.Data;
using Maui.Tasks.Dynamics;
using Maui.Dynamics.Data;
using NUnit.Framework;
using Maui.Dynamics.UnitTest;
using Maui.Dynamics;

namespace Maui.Tasks.UnitTests.Dynamics
{
    [TestFixture]
    public class SingleTest : TestBase
    {
        private TableSchema myTable = null;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            AddDummyStock();
        }

        [Test( Description = "Tests series::last-year" )]
        public void LastYearValue()
        {
            CreateDummyTable( true );

            var eps = myTable[ "eps" ].LastYear();

            Assert.AreEqual( 20.5d, eps );
        }

        [Test( Description = "Tests series::last-year with empty table" )]
        [ExpectedException( typeof( InvalidOperationException ), ExpectedMessage = "Sequence contains no elements", MatchType = MessageMatch.Contains )]
        public void LastYearValueEmptyTable()
        {
            CreateDummyTable( false );

            var eps = myTable[ "eps" ].LastYear();

            Assert.AreEqual( 20.5d, eps );
        }

        private void CreateDummyTable( bool addData )
        {
            // define the table
            myTable = new TableSchema( "dummy",
                    new DataColumn( "stock_id", typeof( long ) ),
                    new DataColumn( "date", typeof( string ) ),
                    new DataColumn( "eps", typeof( double ) )
                 );

            myTable.Create();

            if ( addData )
            {
                var table = myTable.Manager().Query( CurrentStockId );

                // add dummy values to table
                DataRow row = table.NewRow();
                row[ "stock_id" ] = CurrentStockId;
                row[ "date" ] = "2007";
                row[ "eps" ] = 20.5d;
                table.Add( row );

                row = table.NewRow();
                row[ "stock_id" ] = CurrentStockId;
                row[ "date" ] = "2005";
                row[ "eps" ] = 12.5d;
                table.Add( row );

                row = table.NewRow();
                row[ "stock_id" ] = CurrentStockId;
                row[ "date" ] = "2006";
                row[ "eps" ] = 17.3d;
                table.Add( row );
            }
        }
    }
}
