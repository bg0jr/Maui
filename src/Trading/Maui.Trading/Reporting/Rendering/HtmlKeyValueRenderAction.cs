using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Maui.Trading.Model;

namespace Maui.Trading.Reporting.Rendering
{
    public class HtmlKeyValueRenderAction : IRenderAction<KeyValueSection>
    {
        public void Render( KeyValueSection section, IRenderingContext context )
        {
            context.Document.WriteLine( "<h2>{0}</h2>", section.Name );

            HtmlRenderingUtils.RenderDictionary( section.Entries, context.Document );
        }
    }
}
