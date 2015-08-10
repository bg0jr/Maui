using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Maui.Trading.Indicators;
using Maui.Trading.Model;
using Maui.Trading.Modules;
using Maui.Entities;

namespace Maui.Trading.Reporting.Rendering
{
    public class HtmlTableRenderAction : IRenderAction<TableSection>
    {
        private class RenderAction
        {
            private IRenderingContext myContext;
            private TableSection myTable;

            public RenderAction( IRenderingContext context, TableSection section )
            {
                myContext = context;
                myTable = section;
            }

            public void Render()
            {
                myContext.Document.WriteLine( "<h2>{0}</h2>", myTable.Name );

                RenderTable();
            }

            private void RenderTable()
            {
                myContext.Document.WriteLine( "<table id=\"newspaper-a\">" );

                RenderTableHeader();
                RenderTableBody();

                myContext.Document.WriteLine( "</table>" );
            }

            private void RenderTableHeader()
            {
                myContext.Document.WriteLine( "<thead><tr>" );

                foreach ( var column in myTable.Header.Columns )
                {
                    myContext.Document.WriteLine( "<th scope=\"col\">{0}</th>", column.Name );
                }

                myContext.Document.WriteLine( "</tr></thead>" );
            }

            private void RenderTableBody()
            {
                myContext.Document.WriteLine( "<tbody>" );

                foreach ( var row in myTable.View.Rows )
                {
                    RenderRow( row );
                }

                myContext.Document.WriteLine( "</tbody>" );
            }

            private void RenderRow( TableRow row )
            {
                myContext.Document.WriteLine( "<tr>" );

                foreach ( var column in myTable.Header.Columns )
                {
                    myContext.Document.WriteLine( "<td align=\"{0}\">", column.TextAlignment );
                    RenderCellValue( row, column );
                    myContext.Document.WriteLine( "</td>" );
                }

                myContext.Document.WriteLine( "</tr>" );
            }

            private void RenderCellValue( TableRow row, TableColumn column )
            {
                var value = row[ column ];

                if ( value is ValueWithDetails)
                {
                    Render( (ValueWithDetails)value );
                }
                else 
                {
                    myContext.Document.WriteLine( HtmlRenderingUtils.GetDisplayText( value ) );
                }
            }

            private void Render( ValueWithDetails valueWithDetails )
            {
                if ( valueWithDetails.Details == null )
                {
                    myContext.Document.WriteLine( HtmlRenderingUtils.GetDisplayText( valueWithDetails.Value) );
                }
                else
                {
                    using ( var childCtx = myContext.CreateChildContext( valueWithDetails.Details.Reference, valueWithDetails.Details.Name + ".html" ) )
                    {
                        childCtx.Render( valueWithDetails.Details );

                        myContext.Document.WriteLine( "<a href=\"{0}\">{1}</a>", childCtx.RelativeDocumentUrl( myContext ), HtmlRenderingUtils.GetDisplayText( valueWithDetails.Value ) );
                    }
                }
            }
        }

        public void Render( TableSection section, IRenderingContext context )
        {
            var renderAction = new RenderAction( context, section );
            renderAction.Render();
        }
    }
}
