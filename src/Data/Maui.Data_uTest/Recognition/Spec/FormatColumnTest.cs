﻿using System;
using Maui.UnitTests;
using NUnit.Framework;
using Maui.Data.Recognition;
using Maui.Data.Recognition.Spec;

namespace Maui.Data.UnitTests.Recognition.Spec
{
    [TestFixture]
    public class FormatColumnTest : TestBase
    {
        [Test]
        [ExpectedException( typeof( ArgumentException ) )]
        public void CreateInvalid()
        {
            FormatColumn col = new FormatColumn( (string)null );
        }

        [Test]
        public void Create()
        {
            FormatColumn col = new FormatColumn( "test" );
            Assert.AreEqual( "test", col.Name );
            Assert.IsNull( col.Format );
            Assert.AreEqual( typeof( string ), col.Type );

            col = new FormatColumn( "test", typeof( int ) );
            Assert.AreEqual( typeof( int ), col.Type );

            col = new FormatColumn( "test", typeof( int ), "000.000" );
            Assert.AreEqual( "000.000", col.Format );
        }

        [Test]
        [ExpectedException( typeof( ArgumentException ) )]
        public void CopyNull()
        {
            FormatColumn col = new FormatColumn( (FormatColumn)null );
        }

        [Test]
        public void Copy()
        {
            FormatColumn col = new FormatColumn( "test", typeof( int ), "000.000" );
            FormatColumn col2 = new FormatColumn( col );

            Assert.AreEqual( "test", col2.Name );
            Assert.AreEqual( typeof( int ), col2.Type );
            Assert.AreEqual( "000.000", col2.Format );
        }
    }
}
