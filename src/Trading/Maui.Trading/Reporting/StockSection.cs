using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;
using Maui.Entities;

namespace Maui.Trading.Reporting
{
    /// <summary>
    /// Represents details about a stock (standing data). Can be used
    /// anywhere in the report
    /// </summary>
    public class StockSection : AbstractSection
    {
        public StockSection( StockHandle stock )
            : base( "Stock" )
        {
            Stock = stock;
        }

        public StockHandle Stock
        {
            get;
            private set;
        }
    }
}
