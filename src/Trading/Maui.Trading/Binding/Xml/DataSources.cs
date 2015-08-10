using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Maui.Trading.Binding.Xml
{
    public class DataSources
    {
        public DataSources()
        {
            Sources = new List<AbstractDataSource>();
        }

        public Stock Stock
        {
            get;
            set;
        }

        public List<AbstractDataSource> Sources
        {
            get;
            private set;
        }
    }
}
