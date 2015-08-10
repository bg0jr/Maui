using System.Text.RegularExpressions;
using Maui.UnitTests;
using NUnit.Framework;
using Maui.Data.Recognition;
using Maui.Data.Recognition.Spec;

namespace Maui.Data.UnitTests.Recognition.Spec
{
    [TestFixture]
    public class PathSingleValueFormatTest : TestBase
    {
        [Test]
        public void Clone()
        {
            var format = new PathSingleValueFormat( "Ariva.Symbol" );
            format.Path = @"/BODY[0]/DIV[4]/DIV[0]/DIV[3]/DIV[2]";
            format.ValueFormat = new ValueFormat( typeof( int ), "00000000", new Regex( @"Symbol: (\W+)" ) );

            var clone = (PathSingleValueFormat)format.Clone();

            Assert.AreEqual( format.Name, clone.Name );
            Assert.AreEqual( format.Path, clone.Path );
            Assert.AreEqual( format.ValueFormat, format.ValueFormat );
        }
    }
}
