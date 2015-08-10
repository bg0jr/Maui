using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Model
{
    public class CaseInsensitiveMap<TValue> : Map<string, TValue>
    {
        protected override string GetKey( string key )
        {
            return key.ToLower();
        }
    }
}
