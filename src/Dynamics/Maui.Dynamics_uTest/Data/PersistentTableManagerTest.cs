using System.IO;
using System.Transactions;
using Maui;
using Maui.Dynamics.Data;
using NUnit.Framework;
using Maui.Data.SQL.SQLite;

namespace Maui.Dynamics.Data.UnitTests
{
    [TestFixture]
    public class PersistentTableManagerTest : AbstractTableManagerTest
    {
        private string myDbFile = null;
        private TransactionScope myTrans = null;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            myDbFile = Path.GetTempFileName();

            // lets keep this here for performance reason
            myDB = new SQLiteDatabaseSC( myDbFile );
            myDB.CreateDatabase();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            myDB.Dispose();

            BlockingDelete( myDbFile );
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var serviceProvider = new ServiceProvider();
            serviceProvider.RegisterService( "TOM Database", myDB );

            myScriptingInterface = new ScriptingInterface();
            myScriptingInterface.Init( serviceProvider );

            myIsPersistent = true;

            // use transactions for performance reasons only
            myTrans = new TransactionScope();
        }

        [TearDown]
        public override void TearDown()
        {
            myScriptingInterface.Dispose();
            if ( Transaction.Current != null )
            {
                myTrans.Dispose();
            }

            base.TearDown();
        }
    }
}
