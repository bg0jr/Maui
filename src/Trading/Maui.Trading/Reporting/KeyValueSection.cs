using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Reporting
{
    /// <summary>
    /// Generic section for key-value-pair based data.
    /// </summary>
    public class KeyValueSection : AbstractSection
    {
        public KeyValueSection( string name )
            : base( name )
        {
            Entries = new Dictionary<string, object>();
        }

        public Dictionary<string, object> Entries
        {
            get;
            private set;
        }
    }
}
