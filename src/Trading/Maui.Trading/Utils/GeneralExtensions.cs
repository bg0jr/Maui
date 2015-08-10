using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Utils
{
    public static class GeneralExtensions
    {
        public static IEnumerable<T> Join<T>( this T self, params T[] others )
        {
            var set = new List<T>();
            set.Add( self );
            set.AddRange( others );
            return set;
        }

    }
}
