using System;
using System.ComponentModel.DataAnnotations;
using Blade;
using Maui.Shell.Forms;
using Blade.Shell.Forms;

namespace Maui.Shell.Forms
{
    /// <summary>
    /// Argument value points to a Xaml description of a stock catalog.
    /// </summary>
    public class CatalogArgumentAttribute : ConfigFileArgumentAttribute
    {
        public CatalogArgumentAttribute()
        {
            Short = "-c";
            Long = "-catalog";
            Description = "Stock catalog";
        }
    }
}
