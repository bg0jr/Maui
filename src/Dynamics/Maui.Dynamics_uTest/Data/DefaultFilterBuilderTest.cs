using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Maui.Data.SQL;
using Maui;
using Maui.Dynamics.Data;
using NUnit.Framework;
using Maui.UnitTests.Entities;

namespace Maui.Dynamics.Data.UnitTests
{
    [TestFixture]
    public class DefaultFilterBuilderTest : TestBase
    {
        private ScriptingInterface myScriptingInterface = null;
        private IDatabaseSC myDB = null;
        private TableSchema mySchema = null;
        private ITableManager myMgr = null;


        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            myDB = myMockery.NewMock<IDatabaseSC>();

            var serviceProvider = new ServiceProvider();
            serviceProvider.RegisterService( "TOM Database", myDB );

            myScriptingInterface = new ScriptingInterface();
            myScriptingInterface.Init( serviceProvider );

            // prepare data
            mySchema = CreateSchema( "test1" );
            myMgr = myScriptingInterface.GetManager( mySchema );
            myMgr.CreateTable();

            // add some data
            ScopedTable table = myMgr.Query( 0 );
            AddRow( table, 0, 0, new DateTime( 2001, 1, 1 ), 1 );
            AddRow( table, 0, 0, new DateTime( 2002, 1, 1 ), 2 );
            AddRow( table, 0, 1, new DateTime( 2002, 1, 1 ), 12 );
            AddRow( table, 0, 0, new DateTime( 2003, 1, 1 ), 3 );
            AddRow( table, 0, 1, new DateTime( 2003, 1, 1 ), 13 );
            AddRow( table, 0, 2, new DateTime( 2003, 1, 1 ), 23 );
            AddRow( table, 0, 0, new DateTime( 2004, 1, 1 ), 4 );
            AddRow( table, 0, 1, new DateTime( 2005, 1, 1 ), 15 );

            table = myMgr.Query( 1 );
            AddRow( table, 1, 1, new DateTime( 2002, 1, 1 ), 112 );
        }

        [TearDown]
        public override void TearDown()
        {
            myScriptingInterface.Dispose();

            base.TearDown();
        }

        [Test]
        public void Empty()
        {
            // select
            var rows = myMgr.Query( 0, new DateClause( new DateTime( 2005, 1, 1 ) ) ).Rows.ToList();

            Assert.AreEqual( 1, rows.Count );
            Assert.AreEqual( 15, rows[ 0 ][ "value" ] );

            RowFilter filter = new DefaultFilterBuilder( mySchema ).EmptyFilter;
            rows = filter( rows ).ToList();

            Assert.AreEqual( 0, rows.Count );
        }

        [Test]
        public void PassThrough()
        {
            // select
            var rows = myMgr.Query( 0, new DateClause( new DateTime( 2003, 1, 1 ) ) ).Rows.ToList();

            Assert.AreEqual( 3, rows.Count );
            Assert.AreEqual( 3, rows[ 0 ][ "value" ] );
            Assert.AreEqual( 13, rows[ 1 ][ "value" ] );
            Assert.AreEqual( 23, rows[ 2 ][ "value" ] );

            RowFilter filter = new DefaultFilterBuilder( mySchema ).PassThroughFilter;
            rows = filter( rows ).ToList();

            Assert.AreEqual( 3, rows.Count );
            Assert.AreEqual( 3, rows[ 0 ][ "value" ] );
            Assert.AreEqual( 13, rows[ 1 ][ "value" ] );
            Assert.AreEqual( 23, rows[ 2 ][ "value" ] );
        }

        [Test]
        public void OriginSingle()
        {
            // select
            var rows = myMgr.Query( 0, new DateClause( new DateTime( 2003, 1, 1 ) ) ).Rows.ToList();

            Assert.AreEqual( 3, rows.Count );
            Assert.AreEqual( 3, rows[ 0 ][ "value" ] );
            Assert.AreEqual( 13, rows[ 1 ][ "value" ] );
            Assert.AreEqual( 23, rows[ 2 ][ "value" ] );

            RowFilter filter = new DefaultFilterBuilder( mySchema ).CreateOriginFilter( 2 );
            rows = filter( rows ).ToList();

            Assert.AreEqual( 1, rows.Count );
            Assert.AreEqual( 23, rows[ 0 ][ "value" ] );
        }

        [Test]
        public void OriginPrios()
        {
            // select
            var rows = myMgr.Query( 0, new DateClause( new DateTime( 2003, 1, 1 ) ) ).Rows.ToList();

            Assert.AreEqual( 3, rows.Count );
            Assert.AreEqual( 3, rows[ 0 ][ "value" ] );
            Assert.AreEqual( 13, rows[ 1 ][ "value" ] );
            Assert.AreEqual( 23, rows[ 2 ][ "value" ] );

            var prios = new List<long>();
            prios.Add( 2 );
            RowFilter filter = new DefaultFilterBuilder( mySchema ).CreateOriginFilter( prios );
            var filteredRows = filter( rows ).ToList();

            Assert.AreEqual( 1, filteredRows.Count );
            Assert.AreEqual( 23, filteredRows[ 0 ][ "value" ] );

            prios.Clear();
            prios.Add( 1 );
            filter = new DefaultFilterBuilder( mySchema ).CreateOriginFilter( prios );
            filteredRows = filter( rows ).ToList();

            Assert.AreEqual( 1, filteredRows.Count );
            Assert.AreEqual( 13, filteredRows[ 0 ][ "value" ] );
        }

        [Test]
        public void DateRange()
        {
            // select
            var rows = myMgr.Query( 0, new DateClause( new DateTime( 2002, 1, 1 ), new DateTime( 2003, 1, 1 ) ) ).Rows.ToList();

            Assert.AreEqual( 5, rows.Count );
            Assert.AreEqual( 2, rows[ 0 ][ "value" ] );
            Assert.AreEqual( 12, rows[ 1 ][ "value" ] );
            Assert.AreEqual( 3, rows[ 2 ][ "value" ] );
            Assert.AreEqual( 13, rows[ 3 ][ "value" ] );
            Assert.AreEqual( 23, rows[ 4 ][ "value" ] );

            // this should be an identity filter
            RowFilter filter = new DefaultFilterBuilder( mySchema ).CreateDateRangeFilter( new DateTime( 2002, 1, 1 ), new DateTime( 2003, 1, 1 ) );
            rows = filter( rows ).ToList();

            Assert.AreEqual( 5, rows.Count );
            Assert.AreEqual( 2, rows[ 0 ][ "value" ] );
            Assert.AreEqual( 12, rows[ 1 ][ "value" ] );
            Assert.AreEqual( 3, rows[ 2 ][ "value" ] );
            Assert.AreEqual( 13, rows[ 3 ][ "value" ] );
            Assert.AreEqual( 23, rows[ 4 ][ "value" ] );
        }

        [Test]
        public void YearOriginNoMerge()
        {
            // lets change the sorting to check robustness of the code
            var from = new DateTime( 2003, 1, 1 );
            var to = new DateTime( 2002, 1, 1 );

            // select
            var rows = myMgr.Query( 0, new DateClause( from, to ) ).Rows.ToList();

            Assert.AreEqual( 5, rows.Count );
            Assert.AreEqual( 2, rows[ 0 ][ "value" ] );
            Assert.AreEqual( 12, rows[ 1 ][ "value" ] );
            Assert.AreEqual( 3, rows[ 2 ][ "value" ] );
            Assert.AreEqual( 13, rows[ 3 ][ "value" ] );
            Assert.AreEqual( 23, rows[ 4 ][ "value" ] );

            // no merge 
            var prios = new List<long>();
            prios.Add( 1 );
            RowFilter filter = new DefaultFilterBuilder( mySchema ).CreateYearOriginFilter( from, to, prios, false );
            var filteredRows = filter( rows ).ToList();

            Assert.AreEqual( 2, filteredRows.Count );
            Assert.AreEqual( 12, filteredRows[ 0 ][ "value" ] );
            Assert.AreEqual( 13, filteredRows[ 1 ][ "value" ] );
        }

        [Test]
        public void YearOriginMerge()
        {
            var from = new DateTime( 2002, 1, 1 );
            var to = new DateTime( 2004, 1, 1 );

            // select
            var rows = myMgr.Query( 0, new DateClause( from, to ) ).Rows.ToList();

            Assert.AreEqual( 6, rows.Count );
            Assert.AreEqual( 2, rows[ 0 ][ "value" ] );
            Assert.AreEqual( 12, rows[ 1 ][ "value" ] );
            Assert.AreEqual( 3, rows[ 2 ][ "value" ] );
            Assert.AreEqual( 13, rows[ 3 ][ "value" ] );
            Assert.AreEqual( 23, rows[ 4 ][ "value" ] );
            Assert.AreEqual( 4, rows[ 5 ][ "value" ] );

            // no merge -> should be empty then
            var prios = new List<long>();
            prios.Add( 1 );
            RowFilter filter = new DefaultFilterBuilder( mySchema ).CreateYearOriginFilter( from, to, prios, false );
            var filteredRows = filter( rows ).ToList();

            Assert.AreEqual( 0, filteredRows.Count );

            // with merge
            prios.Add( 0 );
            filter = new DefaultFilterBuilder( mySchema ).CreateYearOriginFilter( from, to, prios, true );
            filteredRows = filter( rows ).ToList();

            Assert.AreEqual( 3, filteredRows.Count );
            Assert.AreEqual( 12, filteredRows[ 0 ][ "value" ] );
            Assert.AreEqual( 13, filteredRows[ 1 ][ "value" ] );
            Assert.AreEqual( 4, filteredRows[ 2 ][ "value" ] );
        }

        protected TableSchema CreateSchema( string name )
        {
            return new TableSchema( name, false,
                    new DataColumn( "stock_id", typeof( long ) ),
                    new DataColumn( "datum_origin_id", typeof( long ) ),
                    new DataColumn( "date", typeof( string ) ),
                    new DataColumn( "value", typeof( int ) ) );
        }

        protected void AddRow( ScopedTable table, long id, long originId, DateTime date, int value )
        {
            DataRow row = table.NewRow();
            row[ table.Schema.OwnerIdColumn ] = id;
            row[ table.Schema.OriginColumn ] = originId;
            row.SetDate( table.Schema, date );
            row[ "Value" ] = value;

            table.Add( row );
        }
    }
}
