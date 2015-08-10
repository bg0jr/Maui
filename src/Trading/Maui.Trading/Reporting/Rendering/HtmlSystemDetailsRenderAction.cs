using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Maui.Trading.Indicators;

namespace Maui.Trading.Reporting.Rendering
{
    public class HtmlSystemDetailsRenderAction : IRenderAction<SystemDetailsSection>
    {
        public void Render( SystemDetailsSection section, IRenderingContext context )
        {
            RenderIndicatorResult( section.Indicator, context );
        }

        private void RenderIndicatorResult( IndicatorResult result, IRenderingContext context )
        {
            context.Document.WriteLine( "<h2>Indicators</h2>");
            
            RenderIndicatorSummary( result, context );
            RenderIndicatorDetails( result.Report, context );
        }

        private static void RenderIndicatorSummary( IndicatorResult result, IRenderingContext context )
        {
            context.Document.WriteLine( "<table>" );

            context.Document.WriteLine( "<tr><td>Signal</td><td>{0}</td></tr>", HtmlRenderingUtils.GetDisplayText( result.Signal ) );
            context.Document.WriteLine( "<tr><td>Gain/Risk ratio</td><td>{0}</td></tr>", HtmlRenderingUtils.GetDisplayText( result.GainRiskRatio ) );
            context.Document.WriteLine( "<tr><td>Expected gain</td><td>{0}</td></tr>", HtmlRenderingUtils.GetDisplayText( result.ExpectedGain ) );

            context.Document.WriteLine( "</table>" );
        }

        private void RenderIndicatorDetails( Report report, IRenderingContext context )
        {
            using ( var childCtx = context.CreateChildContext( report.Name + ".html" ) )
            {
                childCtx.Render( report );

                context.Document.WriteLine( "<a href=\"{0}\">Details</a>", childCtx.RelativeDocumentUrl( context ) );
            }
        }
    }
}
