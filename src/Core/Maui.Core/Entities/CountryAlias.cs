using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Entities
{
    public partial class CountryAlias
    {
        public CountryAlias()
        {
        }

        public CountryAlias( Country country, string name )
        {
            Country = country;
            Name = name;
        }
    }
}
