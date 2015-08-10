using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Maui.Trading.Indicators;

namespace Maui.Trading.Reporting.Rendering
{
    public class HtmlIndicatorCollectionRenderAction : IRenderAction<IndicatorCollectionSection>
    {
        public void Render( IndicatorCollectionSection section, IRenderingContext context )
        {
            context.Document.WriteLine( "<h2>{0}</h2>", section.Name );

            RenderIndicatorsTable( section.IndicatorResults, context );
        }

        private void RenderIndicatorsTable( IEnumerable<IndicatorResult> indicatorResults, IRenderingContext context )
        {
            context.Document.WriteLine( "<table id=\"newspaper-a\">" );

            RenderTableHeader( context );
            RenderTableBody( indicatorResults, context );

            context.Document.WriteLine( "</table>" );
        }

        private void RenderTableHeader( IRenderingContext context )
        {
            context.Document.WriteLine( "<tr>" );

            context.Document.WriteLine( "<th>Indicator</th>" );
            context.Document.WriteLine( "<th>Signal</th>" );
            context.Document.WriteLine( "<th>Gain/Risk ratio</th>" );
            context.Document.WriteLine( "<th>Expected gain</th>" );

            context.Document.WriteLine( "</tr>" );
        }

        private void RenderTableBody( IEnumerable<IndicatorResult> indicatorResults, IRenderingContext context )
        {
            foreach ( var result in indicatorResults )
            {
                RenderIndicatorSummeryRow( result, context );
            }
        }

        private void RenderIndicatorSummeryRow( IndicatorResult result, IRenderingContext context )
        {
            context.Document.WriteLine( "<tr>" );

            context.Document.WriteLine( "<td>{0}</td>", result.Indicator );
            RenderSignal( result, context );
            context.Document.WriteLine( "<td align=\"right\">{0}</td>", HtmlRenderingUtils.GetDisplayText( result.GainRiskRatio ) );
            context.Document.WriteLine( "<td align=\"right\">{0}</td>", HtmlRenderingUtils.GetDisplayText( result.ExpectedGain ) );

            context.Document.WriteLine( "</tr>" );
        }

        private void RenderSignal( IndicatorResult result, IRenderingContext context )
        {
            if ( result.Report == null )
            {
                context.Document.WriteLine( "<td>{0}</td>", HtmlRenderingUtils.GetDisplayText( result.Signal ) );
            }
            else
            {
                using ( var childCtx = context.CreateChildContext( result.Indicator, result.Report.Name + ".html" ) )
                {
                    childCtx.Render( result.Report );

                    context.Document.WriteLine( "<td><a href=\"{0}\">{1}</a></td>",
                        childCtx.RelativeDocumentUrl( context ), HtmlRenderingUtils.GetDisplayText( result.Signal ) );
                }
            }
        }
    }
}
