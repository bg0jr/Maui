using System.IO;
using System.Linq;
using Blade;
using Maui.UnitTests;
using Maui.Data.Sheets.Csv;
using NUnit.Framework;
using System;

namespace Maui.Data.UnitTests.Sheets.Csv
{
    [TestFixture]
    public class CsvWorksheetTest : TestBase
    {
        private string myFile;
        private CsvWorksheet mySheet;
        private char mySeparator = ';';

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            myFile = Path.GetTempFileName();
            mySheet = new CsvWorksheet( new CsvWorkbook( Path.GetDirectoryName( myFile ) ), myFile, mySeparator );
        }

        [Test]
        public void RandomFillSheet()
        {
            Assert.IsTrue( mySheet.IsEmptyCell( mySheet.GetCell( "A2" ) ) );

            mySheet.SetCell( "A2", "hello" );

            Assert.AreEqual( "hello", mySheet.GetCell( "A2" ) );
        }

        [Test]
        public void ResizeSheet()
        {
            mySheet.SetCell( "A2", "hello" );
            mySheet.SetCell( "B4", "hello" );
            mySheet.SetCell( "D8", "hello" );

            mySheet.Print();

            var rowsColumns = GetRowsColumns();

            Assert.AreEqual( 8, rowsColumns.Item1 );
            Assert.AreEqual( 4, rowsColumns.Item2 );

            mySheet.SetCell( "D8", null );

            rowsColumns = GetRowsColumns();

            Assert.AreEqual( 4, rowsColumns.Item1 );
            Assert.AreEqual( 2, rowsColumns.Item2 );
            Assert.AreEqual( "hello", mySheet.GetCell( "A2" ) );
            Assert.AreEqual( "hello", mySheet.GetCell( "B4" ) );
        }

        private Tuple<int, int> GetRowsColumns()
        {
            mySheet.Save();

            var lines = File.ReadAllLines( myFile );

            return new Tuple<int, int>(
                lines.Length,
                lines[ 0 ].ToCharArray().Count( c => c == mySeparator ) + 1 );
        }
    }
}
