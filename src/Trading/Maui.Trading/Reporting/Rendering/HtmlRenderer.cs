using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Maui.Trading.Reporting.Charting;

namespace Maui.Trading.Reporting.Rendering
{
    public class HtmlRenderer
    {
        private List<RenderAction> myRenderActions;

        public HtmlRenderer()
        {
            myRenderActions = new List<RenderAction>();

            AddRenderAction( new HtmlReportRenderAction() );
            AddRenderAction( new HtmlStockRenderAction() );
            AddRenderAction( new HtmlKeyValueRenderAction() );
            AddRenderAction( new HtmlIndicatorCollectionRenderAction() );
            AddRenderAction( new HtmlGenericChartRenderAction() );
            AddRenderAction( new HtmlPlainTextRenderAction() );
            AddRenderAction( new HtmlSystemDetailsRenderAction() );
            AddRenderAction( new HtmlAdditionalReportsRenderAction() );
            AddRenderAction( new HtmlTableRenderAction() );
        }

        private void AddRenderAction<T>( IRenderAction<T> renderAction )
        {
            myRenderActions.Add( RenderAction.Create( renderAction ) );
        }

        public void Render( Report report, string outputDirectory )
        {
            var context = new RenderingContext( this );
            context.DocumentRoot = outputDirectory;

            using ( var childCtx = context.CreateChildContext( report.Name + ".html" ) )
            {
                childCtx.Render( report );
            }
        }

        private class RenderingContext : IRenderingContext
        {
            private HtmlRenderer myRenderer;

            public RenderingContext( HtmlRenderer renderer )
            {
                myRenderer = renderer;
            }

            public string DocumentRoot
            {
                get;
                set;
            }

            public TextWriter Document
            {
                get;
                set;
            }

            public void Render( object reportNode )
            {
                if ( reportNode == null )
                {
                    return;
                }

                var renderAction = myRenderer.myRenderActions.FirstOrDefault( action => action.CanRender( reportNode ) );
                if ( renderAction == null )
                {
                    // dont throw exception here - just ignore this section
                    // e.g. for sections containing MovingAverage points we do not want
                    // to render those (we will have explicit chart section for this)
                    return;
                }

                renderAction.Render( reportNode, this );
            }

            public IRenderingContext CreateChildContext()
            {
                return new RenderingContext( myRenderer )
                {
                    DocumentRoot = DocumentRoot
                };
            }

            public string DocumentUrl
            {
                get;
                set;
            }

            public string DocumentUrlRoot
            {
                get
                {
                    return Path.GetDirectoryName( DocumentUrl );
                }
            }

            public void Dispose()
            {
                if ( Document != null )
                {
                    Document.Close();
                    Document = null;
                }

                DocumentRoot = null;

                // do not "null" it -> used to link child documents
                //DocumentUrl = null;
            }
        }

        private class RenderAction : IRenderAction<object>
        {
            private Action<object, IRenderingContext> myRenderAction;

            private RenderAction( Type reportNodeType, Action<object, IRenderingContext> renderAction )
            {
                ReportNodeType = reportNodeType;
                myRenderAction = renderAction;
            }

            public Type ReportNodeType
            {
                get;
                private set;
            }

            public bool CanRender( object reportNode )
            {
                return ReportNodeType.IsAssignableFrom( reportNode.GetType() );
            }

            public void Render( object reportNode, IRenderingContext context )
            {
                myRenderAction( reportNode, context );
            }

            public static RenderAction Create<T>( IRenderAction<T> renderAction )
            {
                return new RenderAction( typeof( T ), ( node, ctx ) => renderAction.Render( (T)node, ctx ) );
            }
        }
    }
}
