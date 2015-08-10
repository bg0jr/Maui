using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using Maui;
using Maui.Data.Recognition.Core;

namespace Maui.Data.UnitTests.Recognition.Core
{
    [TestFixture]
    public class PatternMatchingTests
    {
        [Test]
        public void MatchSimpleString()
        {
            string pattern = @"hiho{(\d+)}bla";
            string value = PatternMatching.MatchEmbeddedRegex( pattern, "hiho1234bla" );

            Assert.AreEqual( "1234", value );
        }

        [Test]
        public void MatchWithEscape()
        {
            string pattern = @"http://bla.de?hiho.x={(\d+)}";
            string value = PatternMatching.MatchEmbeddedRegex( pattern, "http://bla.de?hiho.x=438" );

            Assert.AreEqual( "438", value );
        }

        [Test]
        public void NoEmbeddedRegEx()
        {
            string pattern = @"http://bla.de?hiho.x=";
            string value = PatternMatching.MatchEmbeddedRegex( pattern, "http://bla.de?hiho.x=" );

            Assert.AreEqual( null, value );
        }

        [Test]
        public void NotMatched()
        {
            string pattern = @"http://bla.de?hiho.x={(\d+)}";
            string value = PatternMatching.MatchEmbeddedRegex( pattern, "http://bla.de?hiho.x=bfd" );

            Assert.AreEqual( null, value );
        }
    }
}
