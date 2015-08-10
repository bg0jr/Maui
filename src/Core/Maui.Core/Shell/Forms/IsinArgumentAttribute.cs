using System;
using System.ComponentModel.DataAnnotations;
using Blade;
using Maui.Shell.Forms;
using Blade.Shell.Forms;

namespace Maui.Shell.Forms
{
    /// <summary>
    /// Argument value specifies Isin of a stock.
    /// </summary>
    public class IsinArgumentAttribute : ArgumentAttribute
    {
        public IsinArgumentAttribute()
        {
            Short = "-isin";
            Description = "ISIN of the stock";
        }
    }
}
