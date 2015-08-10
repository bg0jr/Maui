using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Maui.Trading.Reporting
{
    public class TableColumn
    {
        public TableColumn( string name )
        {
            Name = name;
        }

        public string Name
        {
            get;
            private set;
        }

        public HorizontalAlignment TextAlignment
        {
            get;
            set;
        }
    }
}
