using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Windows.Markup;

namespace Maui.Trading.Binding.Xml
{
    [ContentProperty( "Value" )]
    public class SingleDataSource : AbstractDataSource
    {
        [Required]
        public object Value
        {
            get;
            set;
        }
    }
}
