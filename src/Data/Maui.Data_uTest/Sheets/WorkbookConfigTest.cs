using System.IO;
using Maui.Data.Sheets;
using Maui.Data.Sheets.Csv;
using Maui.UnitTests;
using NUnit.Framework;

namespace Maui.Data.UnitTests.Sheets
{
    [TestFixture]
    public class WorkbookConfigTest : TestBase
    {
        private CsvWorksheet mySheet;
        private WorkbookConfig myConfig;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var file = Path.GetTempFileName();
            mySheet = new CsvWorksheet( new CsvWorkbook( Path.GetDirectoryName( file ) ), file, ";" );
            myConfig = new WorkbookConfig( mySheet );

            // set a default section 
            var section = new ConfigSection( "Test1" );
            section.SetProperty( "a", "2" );
            section.SetProperty( "blub", "hiho" );

            myConfig.SetSection( section );
        }

        [Test]
        public void ReadWrite()
        {
            // written already in the setup - just check

            // check the sheet
            Assert.AreEqual( "[Test1]", mySheet.GetCell( "A1" ).ToString() );
            Assert.AreEqual( "a", mySheet.GetCell( "A2" ).ToString() );
            Assert.AreEqual( "2", mySheet.GetCell( "B2" ).ToString() );
            Assert.AreEqual( "blub", mySheet.GetCell( "A3" ).ToString() );
            Assert.AreEqual( "hiho", mySheet.GetCell( "B3" ).ToString() );

            // re-read and check
            var section = myConfig.GetSection( "Test1" );
            Assert.AreEqual( "Test1", section.Name );
            Assert.AreEqual( "2", section.GetProperty( "a" ) );
            Assert.AreEqual( "hiho", section.GetProperty( "blub" ) );
        }

        [Test]
        public void Overwrite()
        {
            var section = myConfig.GetSection( "Test1" );
            section.SetProperty( "a", "3" );

            Assert.AreEqual( "Test1", section.Name );
            Assert.AreEqual( "3", section.GetProperty( "a" ) );
            Assert.AreEqual( "hiho", section.GetProperty( "blub" ) );
        }

        [Test]
        public void ReadNonExistant()
        {
            var section = myConfig.GetSection( "Test1" );

            Assert.AreEqual( null, section.GetProperty( "non-existant" ) );
        }
    }
}
