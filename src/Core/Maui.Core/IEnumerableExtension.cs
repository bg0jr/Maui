using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui
{
    public static class IEnumerableExtension
    {
        public static bool Empty<T>( this IEnumerable<T> set )
        {
            return !set.Any();
        }
    }
}
