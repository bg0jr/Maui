using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Model
{
    public interface IRandomAccessSet<T> : IEnumerable<T>
    {
        int Count { get; }
        T this[ int index ] { get; }
    }
}
