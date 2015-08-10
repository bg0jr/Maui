using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Windows.Markup;

namespace Maui.Trading.Binding.Xml
{
    [ContentProperty( "Values" )]
    public class CollectionDataSource : AbstractDataSource
    {
        public CollectionDataSource()
        {
            Values = new List<object>();
        }

        public List<object> Values
        {
            get;
            set;
        }
    }
}
