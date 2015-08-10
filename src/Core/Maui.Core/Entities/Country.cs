using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Entities
{
    public partial class Country
    {
        public Country()
        {
        }

        public Country( long lcid )
        {
            LCID = lcid;
        }
    }
}
