using System;
using System.Globalization;
using System.IO;
using System.Threading;
using Blade;
using Blade.Testing.NMock;
using Maui;
using NUnit.Framework;

namespace Maui.UnitTests
{
    public abstract class TestBase
    {
        protected AutoMockery myMockery = null;

        public TestBase()
        {
            string dir = Path.GetFileNameWithoutExtension( GetType().Assembly.CodeBase );
            dir = dir.Replace( "Maui.", string.Empty );
            dir = dir.Replace( "_iTest", string.Empty );
            dir = dir.Replace( "_uTest", string.Empty );
            TestDataRoot = Path.Combine( GetType().GetAssemblyPath(), "TestData", dir );
        }

        protected string TestDataRoot { get; private set; }
        protected string MauiHome { get { return MauiEnvironment.Root; } }

        [SetUp]
        public virtual void SetUp()
        {
            myMockery = new AutoMockery();
        }

        [TearDown]
        public virtual void TearDown()
        {
            myMockery.Dispose();
            myMockery = null;
        }

        /// <summary>
        /// Accepted formats:
        /// "2008-07-07"
        /// "2008-07-07 00:00"
        /// </summary>
        protected DateTime GetDate( string s )
        {
            s = s.Trim();

            if ( !s.Contains( ":" ) )
            {
                // contains to time -> add it
                s = s += " 00:00";
            }
            return DateTime.Parse( s, CultureInfo.InvariantCulture );
        }

        public static void BlockingDelete( string file )
        {
            3.Times( i =>
            {
                if ( File.Exists( file ) )
                {
                    try
                    {
                        File.Delete( file );
                        Thread.Sleep( 100 );
                    }
                    catch
                    {
                    }
                }
            } );
        }
    }
}
