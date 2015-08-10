using System;
using System.ComponentModel.DataAnnotations;
using Blade;
using Maui.Shell.Forms;
using Blade.Shell.Forms;

namespace Maui.Shell.Forms
{
    /// <summary>
    /// Argument value specifies a stock exchange.
    /// </summary>
    public class StockExchangeArgumentAttribute : ArgumentAttribute
    {
        public StockExchangeArgumentAttribute()
        {
            Short = "-se";
            Long = "-exchange";
            Description = "Name or symbol of the stock exchange";
        }
    }
}
