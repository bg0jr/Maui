using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Blade;
using Blade.IO;
using Maui.Trading.Utils;

namespace Maui.Trading.Reporting.Rendering
{
    public class HtmlReportRenderAction : IRenderAction<Report>
    {
        private class AdvancedHtmlHeader
        {
            private IRenderingContext myContext;
            private IFileSystem myFileSystem;
            private IDirectory myDocumentRoot;
            private string myBacksteps;

            public AdvancedHtmlHeader( IRenderingContext context )
            {
                myContext = context;

                myFileSystem = new Blade.IO.RealFS.FileSystemImpl();
                myDocumentRoot = myFileSystem.Directory( context.DocumentRoot );

                var documentUrlRoot = myFileSystem.Directory( myContext.DocumentUrlRoot );
                myBacksteps = documentUrlRoot.GetPathBackToRoot( myDocumentRoot );
            }

            public void AddJavaScript( string fileName )
            {
                myContext.Document.WriteLine( "<script type=\"text/javascript\" src=\"{0}{1}\"></script>", myBacksteps, fileName );

                CopyResource( fileName );
            }

            public void AddStyleSheet( string fileName, string media )
            {
                string mediaAttr = media == null ? string.Empty : string.Format( "MEDIA=\"{0}\"", media );

                myContext.Document.WriteLine( "<link rel=\"stylesheet\" href=\"{0}{1}\" TYPE=\"text/css\" {2}>", myBacksteps, fileName, mediaAttr );

                CopyResource( fileName );
            }

            private void CopyResource( string resourceFileName )
            {
                var file = myDocumentRoot.File( resourceFileName );
                if ( file.Exists )
                {
                    return;
                }

                var resource = GetType().Namespace + ".Resources." + file.Name;

                GetType().Assembly.CopyEmbeddedTextResource( resource, file );
            }
        }

        public void Render( Report report, IRenderingContext context )
        {
            context.Document.WriteLine( "<html>" );

            RenderHeader( report, context );
            RenderBody( report, context );

            context.Document.WriteLine( "</html>" );
        }

        private void RenderHeader( Report report, IRenderingContext context )
        {
            context.Document.WriteLine( "<head>" );
            context.Document.WriteLine( "<title>{0}</title>", report.Title );

            // TODO: that should be handled differently. it should be possible to add 
            // special plugins to the renderer which are then handled by this renderaction.
            var header = new AdvancedHtmlHeader( context );
            header.AddJavaScript( "tabs.js" );
            header.AddStyleSheet( "tabs.style.css", "screen" );
            header.AddStyleSheet( "tabs.style-print.css", "print" );
            header.AddStyleSheet( "table.style.css", null );

            context.Document.WriteLine( "</head>" );
        }

        private void RenderBody( Report report, IRenderingContext ctx )
        {
            ctx.Document.WriteLine( "<body>" );
            ctx.Document.WriteLine( "<h1>{0}</h1>", report.Title );

            RenderSections( report.Sections, ctx );

            ctx.Document.WriteLine( "</body>" );
        }

        private void RenderSections( IList<AbstractSection> sections, IRenderingContext ctx )
        {
            foreach ( var section in sections )
            {
                ctx.Render( section );
            }
        }
    }
}
