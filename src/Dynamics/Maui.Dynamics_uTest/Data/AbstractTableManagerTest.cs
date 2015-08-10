using System;
using System.Data;
using System.Linq;
using System.Transactions;
using Blade;
using Maui.Data.SQL;
using Maui.Dynamics.Data;
using NUnit.Framework;
using Maui.UnitTests.Entities;

namespace Maui.Dynamics.Data.UnitTests
{
    public abstract class AbstractTableManagerTest : TestBase
    {
        protected ScriptingInterface myScriptingInterface = null;
        protected IDatabaseSC myDB = null;
        protected bool myIsPersistent = false;

        [Test]
        public void NonExistingTable()
        {
            // no table available yet
            ITableManager mgr = myScriptingInterface.GetManager( "test1" );
            Assert.IsNull( mgr );
        }

        [Test]
        public void CreateTable()
        {
            TableSchema schema = CreateSchema( "test1" );
            Assert.AreEqual( ValidationResult.None, schema.Validate() );

            ITableManager mgr = myScriptingInterface.GetManager( schema );
            Assert.IsNotNull( mgr );
            Assert.That( mgr, Is.InstanceOf( myIsPersistent ? typeof( PersistentTableManager ) : typeof( InMemoryTableManager ) ) );

            mgr.CreateTable();

            // now we should be able to get the manager by name
            mgr = myScriptingInterface.GetManager( "test1" );
            Assert.IsNotNull( mgr );
            Assert.AreEqual( "test1", mgr.Name );

            // but of course not using the wrong name
            Assert.IsNull( myScriptingInterface.GetManager( "test2" ) );
        }

        [Test]
        public void SelectEmptyTable()
        {
            TableSchema schema = CreateSchema( "test1" );
            ITableManager mgr = myScriptingInterface.GetManager( schema );
            mgr.CreateTable();

            // select on empty table
            var result = mgr.Query( 0 ).Rows;
            Assert.AreEqual( 0, result.Count() );
        }

        [Test]
        public void Select()
        {
            TableSchema schema = CreateSchema( "test1" );
            ITableManager mgr = myScriptingInterface.GetManager( schema );
            mgr.CreateTable();

            // add some data
            ScopedTable table = mgr.Query( 0 );
            AddRow( table, 0, new DateTime( 2001, 1, 1 ), 1 );
            AddRow( table, 0, new DateTime( 2002, 1, 1 ), 2 );

            table = mgr.Query( 1 );
            AddRow( table, 1, new DateTime( 2010, 1, 1 ), 12 );

            // select
            var rows = mgr.Query( 0 ).Rows.ToList();

            Assert.AreEqual( 2, rows.Count );

            Assert.AreNotEqual( DBNull.Value, rows[ 0 ][ "id" ] );
            Assert.AreNotEqual( DBNull.Value, rows[ 1 ][ "id" ] );
            Assert.AreNotEqual( rows[ 0 ][ "id" ], rows[ 1 ][ "id" ] );

            Assert.AreEqual( 0, rows[ 0 ][ "stock_id" ] );
            Assert.AreEqual( new DateTime( 2001, 1, 1 ), rows[ 0 ].GetDate( schema ) );
            Assert.AreEqual( 1, rows[ 0 ][ "value" ] );

            Assert.AreEqual( 0, rows[ 1 ][ "stock_id" ] );
            Assert.AreEqual( new DateTime( 2002, 1, 1 ), rows[ 1 ].GetDate( schema ) );
            Assert.AreEqual( 2, rows[ 1 ][ "value" ] );

            Assert.IsTrue( rows.All( r => r.RowState == DataRowState.Unchanged ) );
        }

        [Test]
        public void SelectMultiple()
        {
            TableSchema schema = CreateSchema( "test1" );
            ITableManager mgr = myScriptingInterface.GetManager( schema );
            mgr.CreateTable();

            // add some data
            ScopedTable table = mgr.Query( 0 );
            AddRow( table, 0, new DateTime( 2001, 1, 1 ), 1 );
            AddRow( table, 0, new DateTime( 2002, 1, 1 ), 2 );
            AddRow( table, 1, new DateTime( 2010, 1, 1 ), 2 );

            // first select
            var table1 = mgr.Query( 0 ).Rows.ToList();

            Assert.AreEqual( 2, table1.Count() );
            Assert.AreEqual( 1, table1[ 0 ][ "value" ] );
            Assert.AreEqual( 2, table1[ 1 ][ "value" ] );
            Assert.IsTrue( table1.All( r => r.RowState == DataRowState.Unchanged ) );

            // now select again
            var table2 = mgr.Query( 0 ).Rows.ToList();

            Assert.AreEqual( 2, table1.Count() );
            Assert.AreEqual( 1, table1[ 0 ][ "value" ] );
            Assert.AreEqual( 2, table1[ 1 ][ "value" ] );
            Assert.IsTrue( table1.All( r => r.RowState == DataRowState.Unchanged ) );
        }

        [Test]
        public void SelectFromTo()
        {
            TableSchema schema = CreateSchema( "test1" );
            ITableManager mgr = myScriptingInterface.GetManager( schema );
            mgr.CreateTable();

            // add some data
            ScopedTable table = mgr.Query( 0 );
            AddRow( table, 0, new DateTime( 2001, 1, 1 ), 1 );
            AddRow( table, 0, new DateTime( 2002, 1, 1 ), 2 );
            AddRow( table, 0, new DateTime( 2003, 1, 1 ), 3 );
            AddRow( table, 0, new DateTime( 2004, 1, 1 ), 4 );
            AddRow( table, 0, new DateTime( 2005, 1, 1 ), 5 );
            table = mgr.Query( 1 );
            AddRow( table, 1, new DateTime( 2005, 1, 1 ), 22 );

            // select
            var rows = mgr.Query( 0, new DateClause( new DateTime( 2002, 1, 1 ), new DateTime( 2005, 1, 1 ) ) ).Rows.ToList();

            Assert.AreEqual( 4, rows.Count );
            Assert.AreEqual( 2002, rows[ 0 ].GetDate( schema ).Year );
            Assert.AreEqual( 2003, rows[ 1 ].GetDate( schema ).Year );
            Assert.AreEqual( 2004, rows[ 2 ].GetDate( schema ).Year );
            Assert.AreEqual( 2005, rows[ 3 ].GetDate( schema ).Year );
            Assert.IsTrue( rows.All( r => r.RowState == DataRowState.Unchanged ) );

            // select with swapped to and from to check robustnes
            rows = mgr.Query( 0, new DateClause( new DateTime( 2005, 1, 1 ), new DateTime( 2002, 1, 1 ) ) ).Rows.ToList();

            Assert.AreEqual( 4, rows.Count );
            Assert.AreEqual( 2002, rows[ 0 ].GetDate( schema ).Year );
            Assert.AreEqual( 2003, rows[ 1 ].GetDate( schema ).Year );
            Assert.AreEqual( 2004, rows[ 2 ].GetDate( schema ).Year );
            Assert.AreEqual( 2005, rows[ 3 ].GetDate( schema ).Year );
        }


        /// <summary>
        /// for the date column we do not handle time
        /// so lets check that the time is cut of when given
        /// </summary>
        [Test]
        public void SelectDateTime()
        {
            TableSchema schema = CreateSchema( "test1" );
            ITableManager mgr = myScriptingInterface.GetManager( schema );
            mgr.CreateTable();

            // add some data
            ScopedTable table = mgr.Query( 0 );
            AddRow( table, 0, DateTime.Now, 25 );

            // select
            var rows = mgr.Query( 0, new DateClause( DateTime.Now ) ).Rows.ToList();

            Assert.AreEqual( 1, rows.Count );

            Assert.AreEqual( 25, rows[ 0 ][ "value" ] );
        }

        [Test]
        public void Modify()
        {
            TableSchema schema = CreateSchema( "test1" );
            ITableManager mgr = myScriptingInterface.GetManager( schema );
            mgr.CreateTable();

            // add some data
            var table = mgr.Query( 0 );
            AddRow( table, 0, new DateTime( 2001, 1, 1 ), 1 );
            AddRow( table, 0, new DateTime( 2002, 1, 1 ), 2 );
            AddRow( table, 0, new DateTime( 2003, 1, 1 ), 3 );
            AddRow( table, 1, new DateTime( 2010, 1, 1 ), 2 );

            // modify some data
            {
                table = mgr.Query( 0 );

                // modify
                table.Rows.First()[ "value" ] = 25;

                // delete
                var row1 = table.Rows.ElementAt( 1 );
                var row2 = table.Rows.ElementAt( 2 );
                row1.Delete();
                row2.Delete();

                // add
                AddRow( table, 0, new DateTime( 2004, 1, 1 ), 4 );
            }

            // check modifications
            var rows = mgr.Query( 0 ).Rows.ToList();
            Assert.AreEqual( 2, rows.Count );
            Assert.AreEqual( 25, rows[ 0 ][ "value" ] );
            Assert.AreEqual( 4, rows[ 1 ][ "value" ] );
        }

        [Test]
        public void AddWithNoDate()
        {
            TableSchema schema = CreateSchema( "test1" );
            ITableManager mgr = myScriptingInterface.GetManager( schema );
            mgr.CreateTable();

            // add some data
            using ( TransactionScope trans = new TransactionScope() )
            {
                var table = mgr.Query( 0 );
                AddRow( table, 0, new DateTime( 2001, 1, 1 ), 1 );
                AddRow( table, 0, new DateTime( 2002, 1, 1 ), 2 );

                // refetch
                table = mgr.Query( 0 );

                // add two times without date
                DataRow row = table.NewRow();
                row[ table.Schema.OwnerIdColumn ] = 0;
                row[ "Value" ] = 33;
                table.Add( row );

                row = table.NewRow();
                row[ table.Schema.OwnerIdColumn ] = 0;
                row[ "Value" ] = 44;
                table.Add( row );

                trans.Complete();
            }

            // check modifications
            var rows = mgr.Query( 0 ).Rows.ToList();
            Assert.AreEqual( 4, rows.Count );
            Assert.AreEqual( 1, rows[ 0 ][ "value" ] );
            Assert.AreEqual( 2, rows[ 1 ][ "value" ] );
            Assert.AreEqual( 33, rows[ 2 ][ "value" ] );
            Assert.AreEqual( 44, rows[ 3 ][ "value" ] );
        }

        [Test]
        public void AddOrUpdate()
        {
            TableSchema schema = CreateSchema( "test1" );
            ITableManager mgr = myScriptingInterface.GetManager( schema );
            mgr.CreateTable();

            // add some data
            using ( TransactionScope trans = new TransactionScope() )
            {
                var table = mgr.Query( 0 );
                AddRow( table, 0, new DateTime( 2001, 1, 1 ), 1 );
                AddRow( table, 0, new DateTime( 2002, 1, 1 ), 2 );

                // add with same date
                AddRow( table, 0, new DateTime( 2002, 1, 1 ), 3 );

                // AddOrUpdate() with same date
                var row = table.NewRow();
                row[ table.Schema.OwnerIdColumn ] = 0;
                row.SetDate( table.Schema, new DateTime( 2002, 1, 1 ) );
                row[ "Value" ] = 77;
                table.AddOrUpdate( row );

                trans.Complete();
            }

            // check modifications
            var rows = mgr.Query( 0 ).Rows.ToList();
            Assert.AreEqual( 3, rows.Count );
            Assert.AreEqual( 2001, rows[ 0 ].GetDate( schema ).Year );
            Assert.AreEqual( 1, rows[ 0 ][ "value" ] );
            Assert.AreEqual( 2002, rows[ 1 ].GetDate( schema ).Year );
            Assert.AreEqual( 77, rows[ 1 ][ "value" ] );
            Assert.AreEqual( 2002, rows[ 2 ].GetDate( schema ).Year );
            Assert.AreEqual( 77, rows[ 2 ][ "value" ] );
        }

        [Test]
        public void ReadAfterDelete()
        {
            ITableManager mgr = myScriptingInterface.GetManager( CreateSchema( "test1" ) );
            mgr.CreateTable();

            // add some data -> no commit
            var table = mgr.Query( 0 );
            AddRow( table, 0, new DateTime( 2001, 1, 1 ), 1 );
            AddRow( table, 0, new DateTime( 2002, 1, 1 ), 2 );
            AddRow( table, 0, new DateTime( 2003, 1, 1 ), 3 );

            table.Rows.ElementAt( 1 ).Delete();

            // check modifications
            var rows = table.Rows.ToList();
            Assert.AreEqual( 2, rows.Count );
            Assert.AreEqual( 1, rows[ 0 ][ "value" ] );
            Assert.AreEqual( 3, rows[ 1 ][ "value" ] );
        }

        [Test]
        public void Transactions()
        {
            // kill possible running transactions because
            // nested transactions are not supported yet
            if ( Transaction.Current != null )
            {
                Transaction.Current.Dispose();
                Transaction.Current = null;
            }

            ITableManager mgr = myScriptingInterface.GetManager( CreateSchema( "test1" ) );
            mgr.CreateTable();

            var table = mgr.Query( 0 );

            using ( TransactionScope trans = new TransactionScope() )
            {
                // add some data -> no commit
                AddRow( table, 0, new DateTime( 2001, 1, 1 ), 1 );
                AddRow( table, 0, new DateTime( 2002, 1, 1 ), 2 );
                AddRow( table, 0, new DateTime( 2003, 1, 1 ), 3 );

                // lets check nested transactions
                // !!nested transactions are not supported yet!!
                /*
                using ( TransactionScope innerTrans = new TransactionScope() )
                {
                    AddRow( table, 0, new DateTime( 2004, 1, 1 ), 3 );
                }
                Assert.AreEqual( 3, table.Rows.Count() );
                 */

                trans.Complete();
            }

            using ( TransactionScope trans = new TransactionScope() )
            {
                table.Rows.ElementAt( 1 ).Delete();
            }

            // Re-Fetch after Rollback required.
            table = mgr.Query( 0 );

            // check modifications
            var rows = table.Rows.ToList();
            Assert.AreEqual( 3, rows.Count );
            Assert.AreEqual( 1, rows[ 0 ][ "value" ] );
            Assert.AreEqual( 2, rows[ 1 ][ "value" ] );
            Assert.AreEqual( 3, rows[ 2 ][ "value" ] );

            using ( TransactionScope trans = new TransactionScope() )
            {
                table.Rows.ElementAt( 1 ).Delete();
                trans.Complete();
            }

            // check modifications
            rows = table.Rows.ToList();
            Assert.AreEqual( 2, rows.Count );
            Assert.AreEqual( 1, rows[ 0 ][ "value" ] );
            Assert.AreEqual( 3, rows[ 1 ][ "value" ] );
        }

        [Test]
        public void TableWithoutDate()
        {
            TableSchema schema = new TableSchema( "test2", myIsPersistent,
                    new DataColumn( "stock_id", typeof( long ) ),
                    new DataColumn( "value", typeof( string ) ) );

            ITableManager mgr = myScriptingInterface.GetManager( schema );
            mgr.CreateTable();

            // add some data
            var table = mgr.Query( 0 );
            AddRow( table, 0, "1" );
            AddRow( table, 0, "2" );
            AddRow( table, 0, "3" );
            AddRow( table, 1, "2" );

            // modify some data
            {
                table = mgr.Query( 0 );
                Assert.AreEqual( 3, table.Rows.Count() );

                // modify
                table.Rows.First()[ "value" ] = "25";

                // delete
                var row1 = table.Rows.ElementAt( 1 );
                var row2 = table.Rows.ElementAt( 2 );
                row1.Delete();
                row2.Delete();

                Assert.AreEqual( 1, mgr.Query( 0 ).Rows.Count() );

                // add
                AddRow( table, 0, "4" );
            }

            // check modifications
            var rows = mgr.Query( 0 ).Rows.ToList();
            Assert.AreEqual( 2, rows.Count );
            Assert.AreEqual( "25", rows[ 0 ][ "value" ] );
            Assert.AreEqual( "4", rows[ 1 ][ "value" ] );

            // AddOrUpdate
            var row = table.NewRow();
            row[ table.Schema.OwnerIdColumn ] = 0;
            row[ "Value" ] = "77";
            table.AddOrUpdate( row );

            rows = mgr.Query( 0 ).Rows.ToList();
            Assert.AreEqual( 2, rows.Count );
            Assert.AreEqual( "77", rows[ 0 ][ "value" ] );
            Assert.AreEqual( "77", rows[ 1 ][ "value" ] );
        }

        protected TableSchema CreateSchema( string name )
        {
            this.Require( x => myIsPersistent != null );

            return new TableSchema( name, myIsPersistent,
                    new DataColumn( "stock_id", typeof( long ) ),
                    new DataColumn( "date", typeof( string ) ),
                    new DataColumn( "value", typeof( int ) ) );
        }

        protected void AddRow( ScopedTable table, long id, DateTime date, int value )
        {
            DataRow row = table.NewRow();
            row[ table.Schema.OwnerIdColumn ] = id;
            row.SetDate( table.Schema, date );
            row[ "Value" ] = value;

            table.Add( row );
        }

        protected void AddRow( ScopedTable table, long id, string value )
        {
            DataRow row = table.NewRow();
            row[ table.Schema.OwnerIdColumn ] = id;
            row[ "Value" ] = value;

            table.Add( row );
        }
    }
}
