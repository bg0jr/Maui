using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Maui.Trading.Model
{
    public interface IMap<TKey, TElement> : IEnumerable<KeyValuePair<TKey, TElement>>, IEnumerable
    {
        int Count { get; }

        TElement this[ TKey key ] { get; }

        bool Contains( TKey key );
    }
}
