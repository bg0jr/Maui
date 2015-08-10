using System.IO;
using System.Xml.Schema;
using Maui.Configuration;
using NUnit.Framework;

namespace Maui.UnitTests.Configuration
{
    [TestFixture]
    public class ConfigurationSCTest : TestBase
    {
        private ConfigurationSC mySC = null;

        public override void SetUp()
        {
            base.SetUp();

            mySC = ConfigurationSC.Instance;
            mySC.Init( new ServiceProvider() );
        }

        [Test]
        public void LoadValidXml()
        {
            mySC.Import( Path.Combine( TestDataRoot, "Maui.Services.Msl.xml" ), true );

            Assert.AreEqual( "false", mySC.TryGetValue( "Maui.Services.Msl.MslInterpreter.DatumOrigin.EnableMerge" ) );
        }

        [Test]
        public void LoadXmlExplicitNS()
        {
            mySC.Import( Path.Combine( TestDataRoot, "Maui.Services.Msl.explicitNS.xml" ), true );

            Assert.AreEqual( "false", mySC.TryGetValue( "Maui.Services.Msl.MslInterpreter.DatumOrigin.EnableMerge" ) );
        }

        [Test]
        [ExpectedException( ExpectedException = typeof( XmlSchemaValidationException ), ExpectedMessage = "'default' attribute is not declared", MatchType = MessageMatch.Contains )]
        public void LoadInvalidXml()
        {
            mySC.Import( Path.Combine( TestDataRoot, "Maui.Services.Msl.wrong.xml" ), true );
        }

        [Test]
        public void CreateAccessor()
        {
            mySC.Import( Path.Combine( TestDataRoot, "Maui.Services.Msl.xml" ), true );

            var accessor = mySC.CreateAccessor( "Maui.Services.Msl.MslInterpreter.DatumOrigin.EnableMerge" );

            Assert.AreEqual( "false", accessor.Value );

            accessor.Value = "true";

            Assert.AreEqual( "true", accessor.Value );
        }
    }
}
