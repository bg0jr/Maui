using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Maui.Trading.Binding.Xml
{
    public abstract class AbstractDataSource
    {
        [Required]
        public string Name
        {
            get;
            set;
        }

        public string Currency
        {
            get;
            set;
        }
    }
}
