﻿using System;
using Maui.UnitTests;
using NUnit.Framework;
using Maui.Data.Recognition;
using Maui.Data.Recognition.Spec;

namespace  Maui.Data.UnitTests.Recognition.Spec
{
    [TestFixture]
    public class AbstractDimensionalFormatTest : TestBase
    {
        private class DummyFormat : AbstractDimensionalFormat
        {
            public DummyFormat()
                : base( "dummy" )
            {
            }

            public override IFormat Clone()
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void SkipRowsIsImmutable()
        {
            var format = new DummyFormat();

            var skipRows = new int[] { 1, 2, 3 };
            format.SkipRows = skipRows;

            skipRows[ 1 ] = 42;

            Assert.AreEqual( 1, format.SkipRows[ 0 ] );
            Assert.AreEqual( 2, format.SkipRows[ 1 ] );
            Assert.AreEqual( 3, format.SkipRows[ 2 ] );
        }

        [Test]
        public void SkipColumnsIsImmutable()
        {
            var format = new DummyFormat();

            var skipColumns = new int[] { 1, 2, 3 };
            format.SkipColumns = skipColumns;

            skipColumns[ 1 ] = 42;

            Assert.AreEqual( 1, format.SkipColumns[ 0 ] );
            Assert.AreEqual( 2, format.SkipColumns[ 1 ] );
            Assert.AreEqual( 3, format.SkipColumns[ 2 ] );
        }
    }
}
