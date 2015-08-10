using System.IO;
using Maui.Data.Recognition;
using Maui.Data.Recognition.Core;
using Maui.Data.Recognition.Html;
using Maui;
using NUnit.Framework;
using Maui.Data.Recognition.Spec;

namespace Maui.Data.UnitTests.Recognition.Html
{
    public class TestBase : Maui.UnitTests.TestBase
    {
        private ServiceProvider myServiceProvider;
        private IDocumentBrowser myBrowser;

        [TestFixtureSetUp]
        public virtual void FixtureSetUp()
        {
            myServiceProvider = new ServiceProvider();
            myBrowser = myServiceProvider.Browser();
        }

        [TestFixtureTearDown]
        public virtual void FixtureTearDown()
        {
            myServiceProvider.Reset();
        }

        protected IHtmlDocument LoadDocument( string name )
        {
            string file = Path.Combine( TestDataRoot, "Recognition", "Html" );
            file = Path.Combine( file, name );

            var navi = new Navigation( DocumentType.Html, new NavigatorUrl( UriType.Request, file ) );
            var doc = (HtmlDocumentHandle)myBrowser.GetDocument( navi );

            return doc.Content;
        }
    }
}
