using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Reporting
{
    public abstract class AbstractSection
    {
        protected AbstractSection( string name )
        {
            Name = name;
        }

        public string Name
        {
            get;
            private set;
        }
    }
}
