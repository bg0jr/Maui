using System.Text.RegularExpressions;
using Blade.IO;
using Maui.Data.Recognition;
using Maui;
using NUnit.Framework;
using Maui.Data.Recognition.Spec;

namespace Maui.Data.UnitTests.Recognition.Core
{
    [TestFixture]
    public class HtmlDocumentTest : TestBase
    {
        private IDocumentBrowser myWebScrapSC = null;

        public override void SetUp()
        {
            base.SetUp();

            var serviceProvider = new ServiceProvider();
            myWebScrapSC = serviceProvider.Browser();
        }

        public override void TearDown()
        {
            myWebScrapSC = null;

            base.TearDown();
        }

        [Test]
        public void WpknFromAriva()
        {
            var inputFile = OS.CombinePaths( TestDataRoot, "Recognition", "Core", "ariva.overview.US0138171014.html" );
            var doc = myWebScrapSC.GetDocument( new Navigation( DocumentType.Html, inputFile ) );

            var format = new PathSingleValueFormat( "Ariva.Wpkn" );
            format.Path = @"/BODY[0]/DIV[4]/DIV[0]/DIV[3]/DIV[0]";
            format.ValueFormat = new ValueFormat( typeof( int ), "00000000", new Regex( @"WKN: (\d+)" ) );

            var table = doc.ExtractTable( format );

            Assert.AreEqual( 1, table.Rows.Count );

            Assert.AreEqual( 850206, table.Rows[ 0 ][ 0 ] );
        }

    }
}
