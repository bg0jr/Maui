using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Maui.Trading.Model;

namespace Maui.Trading.Reporting.Rendering
{
    public class HtmlPlainTextRenderAction : IRenderAction<PlainTextSection>
    {
        public void Render( PlainTextSection section, IRenderingContext context )
        {
            context.Document.WriteLine( "<h2>{0}</h2>", section.Name );

            context.Document.WriteLine( "<p>" );
            context.Document.WriteLine( section.GetText() );
            context.Document.WriteLine( "</p>" );
        }
    }
}
