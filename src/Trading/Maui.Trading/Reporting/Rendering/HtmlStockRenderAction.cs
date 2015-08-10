using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Maui.Trading.Model;
using Maui.Entities;

namespace Maui.Trading.Reporting.Rendering
{
    public class HtmlStockRenderAction : IRenderAction<StockSection>
    {
        public void Render( StockSection section, IRenderingContext context )
        {
            context.Document.WriteLine( "<h2>Stock</h2>" );

            RenderStockDetails( section.Stock, context.Document );
        }

        private void RenderStockDetails( StockHandle stock, TextWriter document )
        {
            var details = new Dictionary<string, object>();
            details[ "Name" ] = stock.Name;
            details[ "Isin" ] = stock.Isin;
            details[ "Currency" ] = stock.StockExchange.Currency.Name;

            HtmlRenderingUtils.RenderDictionary( details, document );
        }
    }
}
