using Maui;
using Maui.Dynamics.Data;
using NUnit.Framework;
using Maui.Data.SQL;

namespace Maui.Dynamics.Data.UnitTests
{
    [TestFixture]
    public class InMemoryTableManagerTest : AbstractTableManagerTest
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            myDB = myMockery.NewMock<IDatabaseSC>();

            var serviceProvider = new ServiceProvider();
            serviceProvider.RegisterService( "TOM Database", myDB );

            myScriptingInterface = new ScriptingInterface();
            myScriptingInterface.Init( serviceProvider );

            myIsPersistent = false;
        }

        [TearDown]
        public override void TearDown()
        {
            myScriptingInterface.Dispose();

            base.TearDown();
        }
    }
}
