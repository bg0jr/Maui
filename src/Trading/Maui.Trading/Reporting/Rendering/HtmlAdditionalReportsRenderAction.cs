using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Maui.Trading.Indicators;

namespace Maui.Trading.Reporting.Rendering
{
    public class HtmlAdditionalReportsRenderAction : IRenderAction<AdditionalReportsSection>
    {
        public void Render( AdditionalReportsSection section, IRenderingContext context )
        {
            context.Document.WriteLine( "<h2>{0}</h2>", section.Name );

            RenderReports( section.AdditionalReports, context );
        }

        private void RenderReports( IList<Report> reports, IRenderingContext context )
        {
            context.Document.WriteLine( "<ul>" );
            
            foreach(var report in reports)
            {
                context.Document.WriteLine( "<li>" );

                RenderReport( report, context );

                context.Document.WriteLine( "</li>" );
            }

            context.Document.WriteLine( "</ul>" );
        }

        private void RenderReport( Report report, IRenderingContext context )
        {
            using ( var childCtx = context.CreateChildContext( report.Name + ".html" ) )
            {
                childCtx.Render( report );

                context.Document.WriteLine( "<a href=\"{0}\">{1}</a>", childCtx.RelativeDocumentUrl( context ), report.Name );
            }
        }
    }
}
