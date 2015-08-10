using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using HtmlAgilityPack;
using Maui.Data.Recognition.Core;
using Maui.Logging;
using Maui.Shell;
using Maui.Shell.Forms;
using Blade.Logging;
using Blade.Shell.Forms;
using Blade.Shell;

namespace Maui.Tools.Scripts
{
    public class MkOffline : ScriptBase
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( MkOffline ) );

        private HtmlDocument myDocument = null;
        private string myOutputFile = null;

        [Argument( Short = "-n", Description = "Only download the given page" )]
        public bool DownloadOnly
        {
            get;
            set;
        }

        [Argument( Short = "-url", Description = "URL to the page to download" ), Required]
        public Uri InputUri
        {
            get;
            set;
        }

        protected override void Run()
        {
            LoadDocument();

            MakeDocumentOfflineUsable();

            StoreOfflineDocument();

            PrintFinishedBanner();
        }

        private void LoadDocument()
        {
            bool deleteInput = false;
            string inputFile = InputUri.ToString();

            if ( InputUri.Scheme != Uri.UriSchemeFile )
            {
                deleteInput = true;
                inputFile = Path.GetTempFileName();

                WebUtil.DownloadTo( InputUri, inputFile );
            }

            myDocument = new HtmlDocument();
            myDocument.Load( inputFile );

            if ( deleteInput )
            {
                File.Delete( inputFile );
            }
        }

        private void MakeDocumentOfflineUsable()
        {
            if ( DownloadOnly )
            {
                return;
            }

            myDocument.RemoveWebReferences();
        }

        private void StoreOfflineDocument()
        {
            myOutputFile = GetOutputFile( InputUri );
            myDocument.Save( myOutputFile );
        }

        private string GetOutputFile( Uri inputUrl )
        {
            return inputUrl.Scheme == Uri.UriSchemeFile
                ? inputUrl.LocalPath + ".offline.html"
                : "output.html";
        }

        private void PrintFinishedBanner()
        {
            Console.WriteLine( "Output written to: " + myOutputFile );

            Console.WriteLine( "" );
            Console.WriteLine( "Remaining steps:" );
            Console.WriteLine( "  - manually check for: <style type=\"text/css\"><!--@import url()--></style>" );
        }
    }
}

