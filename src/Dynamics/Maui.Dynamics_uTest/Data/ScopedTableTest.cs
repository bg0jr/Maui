using System;
using System.Data;
using System.Linq;
using Blade;
using Maui.Dynamics.Data;
using NUnit.Framework;
using Maui.UnitTests.Entities;

namespace Maui.Dynamics.Data.UnitTests
{
    [TestFixture]
    public class ScopedTableTest : TestBase
    {
        [Test]
        public void NotNullOk()
        {
            var col = new DataColumn( "stock_id", typeof( long ) );
            col.AllowDBNull = false;

            TableSchema schema = new TableSchema( "test1",
                col,
                new DataColumn( "value", typeof( double ) ) );

            DataTable t = schema.NewTempTable();

            ScopedTable table = new ScopedTable( schema, t, null );

            DataRow row = table.NewRow();
            row[ "stock_id" ] = 1;
            row[ "value" ] = 23.0d;

            table.Add( row );

            var rows = table.Rows.ToList();
            Assert.AreEqual( 1, rows.Count );
            Assert.AreEqual( 1, row[ "stock_id" ] );
            Assert.AreEqual( 23.0d, row[ "value" ] );
        }

        [Test]
        [ExpectedException( ExpectedException = typeof( Exception ), ExpectedMessage = "Column must not be null: test1.stock_id", MatchType = MessageMatch.Contains )]
        public void NotNullIsNull()
        {
            var col = new DataColumn( "stock_id", typeof( long ) );
            col.AllowDBNull = false;

            TableSchema schema = new TableSchema( "test1",
                col,
                new DataColumn( "value", typeof( double ) ) );

            DataTable t = schema.NewTempTable();

            ScopedTable table = new ScopedTable( schema, t, null );

            DataRow row = table.NewRow();
            row[ "value" ] = 23.0d;

            table.Add( row );
        }

        [Test]
        public void SetTimestamp()
        {
            TableSchema schema = new TableSchema( "test1",
                new DataColumn( "stock_id", typeof( long ) ),
                new DataColumn( "timestamp", typeof( string ) ),
                new DataColumn( "value", typeof( double ) ) );

            DataTable t = schema.NewTempTable();

            ScopedTable table = new ScopedTable( schema, t, null );

            DataRow row = table.NewRow();
            row[ "stock_id" ] = 1;
            row[ "value" ] = 23.0d;

            table.Add( row );

            Assert.IsTrue( DateTime.Now.AlmostEquals( row.GetDate( "timestamp" ), 1 ) );
        }

        [Test]
        public void AddOrUpdateWithDate()
        {
            TableSchema schema = new TableSchema( "test1",
                new DataColumn( "stock_id", typeof( long ) ),
                new DataColumn( "date", typeof( string ) ),
                new DataColumn( "value", typeof( double ) ) );

            DataTable t = schema.NewTempTable();

            ScopedTable table = new ScopedTable( schema, t, null );

            // initial row
            {
                DataRow row = table.NewRow();
                row[ "stock_id" ] = 1;
                row.SetDate( schema, DateTime.Parse( "2002-01-01 00:00" ) );
                row[ "value" ] = 23.0d;

                table.AddOrUpdate( row );
            }

            // same id, same date => update
            {
                DataRow row = table.NewRow();
                row[ "stock_id" ] = 1;
                row.SetDate( schema, DateTime.Parse( "2002-01-01 00:00" ) );
                row[ "value" ] = 42.0d;

                table.AddOrUpdate( row );
            }

            // same id, new date => add
            {
                DataRow row = table.NewRow();
                row[ "stock_id" ] = 1;
                row.SetDate( schema, DateTime.Parse( "2002-01-02 00:00" ) );
                row[ "value" ] = 25.0d;

                table.AddOrUpdate( row );
            }

            // new stock_id, same date => add
            {
                DataRow row = table.NewRow();
                row[ "stock_id" ] = 2;
                row.SetDate( schema, DateTime.Parse( "2002-01-02 00:00" ) );
                row[ "value" ] = 37.0d;

                table.AddOrUpdate( row );
            }

            var rows = table.Rows.ToList();
            Assert.AreEqual( 3, rows.Count );

            Assert.AreEqual( 1, rows[ 0 ][ schema.OwnerIdColumn ] );
            Assert.AreEqual( 42.0d, rows[ 0 ][ "value" ] );

            Assert.AreEqual( 1, rows[ 1 ][ schema.OwnerIdColumn ] );
            Assert.AreEqual( 25.0d, rows[ 1 ][ "value" ] );

            Assert.AreEqual( 2, rows[ 2 ][ schema.OwnerIdColumn ] );
            Assert.AreEqual( 37.0d, rows[ 2 ][ "value" ] );
        }

        /// <summary>
        /// No origin clause means passing identity.
        /// </summary>
        [Test]
        public void OriginNull()
        {
            TableSchema schema = new TableSchema( "OriginTest",
                new DataColumn( "stock_id", typeof( long ) ),
                new DataColumn( "date", typeof( string ) ),
                new DataColumn( "datum_origin_id", typeof( long ) ),
                new DataColumn( "value", typeof( double ) ) );

            DataTable t = schema.NewTempTable();

            ScopedTable table = new ScopedTable( schema, t, null );
            AddRow( table, 1, "2002-01-02 00:00", 1, 2.1d );
            AddRow( table, 1, "2002-01-02 00:00", 2, 2.2d );
            AddRow( table, 1, "2002-01-02 00:00", 3, 2.3d );
            AddRow( table, 1, "2003-01-02 00:00", 1, 3.1d );
            AddRow( table, 1, "2003-01-02 00:00", 3, 3.3d );
            AddRow( table, 1, "2004-01-02 00:00", 1, 4.1d );

            var rows = table.Rows.ToList();

            Assert.AreEqual( 6, rows.Count );
            Assert.AreEqual( 2.1d, (double)rows[ 0 ][ "value" ], 0.000001d );
            Assert.AreEqual( 2.2d, (double)rows[ 1 ][ "value" ], 0.000001d );
            Assert.AreEqual( 2.3d, (double)rows[ 2 ][ "value" ], 0.000001d );
            Assert.AreEqual( 3.1d, (double)rows[ 3 ][ "value" ], 0.000001d );
            Assert.AreEqual( 3.3d, (double)rows[ 4 ][ "value" ], 0.000001d );
            Assert.AreEqual( 4.1d, (double)rows[ 5 ][ "value" ], 0.000001d );
        }

        [Test]
        public void OriginNoRankingNoMerge()
        {
            TableSchema schema = new TableSchema( "OriginTest",
                new DataColumn( "stock_id", typeof( long ) ),
                new DataColumn( "date", typeof( string ) ),
                new DataColumn( "datum_origin_id", typeof( long ) ),
                new DataColumn( "value", typeof( double ) ) );

            DataTable t = schema.NewTempTable();

            ScopedTable table = new ScopedTable( schema, t, new OriginClause() );
            AddRow( table, 1, "2002-01-02 00:00", 1, 2.1d );
            AddRow( table, 1, "2002-01-02 00:00", 2, 2.2d );
            AddRow( table, 1, "2002-01-02 00:00", 3, 2.3d );
            AddRow( table, 1, "2003-01-02 00:00", 1, 3.1d );
            AddRow( table, 1, "2003-01-02 00:00", 3, 3.3d );
            AddRow( table, 1, "2004-01-02 00:00", 1, 4.1d );

            var rows = table.Rows.ToList();
            Assert.AreEqual( 3, rows.Count );
            Assert.AreEqual( 2.1d, (double)rows[ 0 ][ "value" ], 0.000001d );
            Assert.AreEqual( 3.1d, (double)rows[ 1 ][ "value" ], 0.000001d );
            Assert.AreEqual( 4.1d, (double)rows[ 2 ][ "value" ], 0.000001d );
        }

        [Test]
        public void OriginRankingNoMerge()
        {
            TableSchema schema = new TableSchema( "OriginTest",
                new DataColumn( "stock_id", typeof( long ) ),
                new DataColumn( "date", typeof( string ) ),
                new DataColumn( "datum_origin_id", typeof( long ) ),
                new DataColumn( "value", typeof( double ) ) );

            DataTable t = schema.NewTempTable();

            ScopedTable table = new ScopedTable( schema, t, new OriginClause( 2, 1, 3 ) );
            AddRow( table, 1, "2002-01-02 00:00", 1, 2.1d );
            AddRow( table, 1, "2002-01-02 00:00", 2, 2.2d );
            AddRow( table, 1, "2002-01-02 00:00", 3, 2.3d );
            AddRow( table, 1, "2003-01-02 00:00", 1, 3.1d );
            AddRow( table, 1, "2003-01-02 00:00", 2, 3.2d );
            AddRow( table, 1, "2004-01-02 00:00", 2, 4.2d );

            var rows = table.Rows.ToList();
            Assert.AreEqual( 3, rows.Count );
            Assert.AreEqual( 2.2d, (double)rows[ 0 ][ "value" ], 0.000001d );
            Assert.AreEqual( 3.2d, (double)rows[ 1 ][ "value" ], 0.000001d );
            Assert.AreEqual( 4.2d, (double)rows[ 2 ][ "value" ], 0.000001d );
        }

        [Test]
        public void OriginRankingWithMerge()
        {
            TableSchema schema = new TableSchema( "OriginTest",
                new DataColumn( "stock_id", typeof( long ) ),
                new DataColumn( "date", typeof( string ) ),
                new DataColumn( "datum_origin_id", typeof( long ) ),
                new DataColumn( "value", typeof( double ) ) );

            DataTable t = schema.NewTempTable();

            var originClause = new OriginClause( true, 2, 1 );

            ScopedTable table = new ScopedTable( schema, t, originClause );

            AddRow( table, 1, "2002-01-02 00:00", 1, 2.1d );
            AddRow( table, 1, "2002-01-02 00:00", 2, 2.2d );
            AddRow( table, 1, "2002-01-02 00:00", 3, 2.3d );
            AddRow( table, 1, "2003-01-02 00:00", 1, 3.1d );
            AddRow( table, 1, "2003-01-02 00:00", 3, 3.3d );
            AddRow( table, 1, "2004-01-02 00:00", 3, 4.3d );

            var rows = table.Rows.ToList();
            Assert.AreEqual( 3, rows.Count );
            Assert.AreEqual( 2.2d, (double)rows[ 0 ][ "value" ], 0.000001d );
            Assert.AreEqual( 3.1d, (double)rows[ 1 ][ "value" ], 0.000001d );
            Assert.AreEqual( 4.3d, (double)rows[ 2 ][ "value" ], 0.000001d );
        }

        [Test]
        public void OriginAddOrUpdate()
        {
            TableSchema schema = new TableSchema( "OriginTest",
                new DataColumn( "stock_id", typeof( long ) ),
                new DataColumn( "date", typeof( string ) ),
                new DataColumn( "datum_origin_id", typeof( long ) ),
                new DataColumn( "value", typeof( double ) ) );

            DataTable t = schema.NewTempTable();

            ScopedTable table = new ScopedTable( schema, t, null );

            AddRow( table, 1, "2002-01-02 00:00", 1, 2.1d );
            AddRow( table, 1, "2002-01-02 00:00", 1, 2.2d );
            AddRow( table, 1, "2002-01-02 00:00", 1, 2.3d );
            AddRow( table, 1, "2003-01-02 00:00", 1, 3.1d );
            AddRow( table, 1, "2003-01-02 00:00", 3, 3.3d );
            AddRow( table, 1, "2004-01-02 00:00", 3, 4.3d );

            var rows = table.Rows.ToList();
            Assert.AreEqual( 4, rows.Count );
            Assert.AreEqual( 2.3d, (double)rows[ 0 ][ "value" ], 0.000001d );
            Assert.AreEqual( 3.1d, (double)rows[ 1 ][ "value" ], 0.000001d );
            Assert.AreEqual( 3.3d, (double)rows[ 2 ][ "value" ], 0.000001d );
            Assert.AreEqual( 4.3d, (double)rows[ 3 ][ "value" ], 0.000001d );
        }

        private void AddRow( ScopedTable table, long ownerId, string date, long origin, double value )
        {
            DataRow row = table.NewRow();
            row[ table.Schema.OwnerIdColumn ] = ownerId;
            row.SetDate( table.Schema, DateTime.Parse( date ) );
            row[ table.Schema.OriginColumn ] = origin;
            row[ "value" ] = value;

            table.AddOrUpdate( row );
        }

    }
}
