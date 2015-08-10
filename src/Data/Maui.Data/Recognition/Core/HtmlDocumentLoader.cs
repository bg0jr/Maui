using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Maui.Data.Recognition.Html;
using HtmlAgilityPack;
using Maui.Data.Recognition.Html.AgilityPack;

namespace Maui.Data.Recognition.Core
{
    /// <summary>
    /// Attention: Currently it still has a lot of trouble parsing HTML !! HtmlPath from WindowsExplorer does not match!
    /// </summary>
    class HtmlDocumentLoader : IDocumentLoader
    {
        public IDocument Load( Uri uri )
        {
            TuneHtmlParser();

            return new HtmlDocumentHandle( LoadDocument( uri ) );
        }

        private static void TuneHtmlParser()
        {
            // http://htmlagilitypack.codeplex.com/workitem/23074
            HtmlNode.ElementsFlags.Remove( "form" );
        }

        private IHtmlDocument LoadDocument( Uri url )
        {
            var client = WebUtil.CreateWebClient();

            var doc = new HtmlDocument();
            using ( var stream = client.OpenRead( url ) )
            {
                doc.Load( stream );
            }
            return new HtmlDocumentAdapter( url, doc );
        }
    }
}
