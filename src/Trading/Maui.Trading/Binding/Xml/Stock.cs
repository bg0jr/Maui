using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Maui.Trading.Binding.Xml
{
    public class Stock
    {
        [Required]
        public string Isin
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
}
