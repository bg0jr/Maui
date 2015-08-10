using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Maui.Data.Recognition.Core
{
    class TextDocumentLoader : IDocumentLoader
    {
        public IDocument Load( Uri uri )
        {
            var localFile = WebUtil.Download( uri );

            return new TextDocument( localFile );
        }
    }
}
