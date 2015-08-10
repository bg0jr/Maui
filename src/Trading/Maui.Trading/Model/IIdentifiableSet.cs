using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Model
{
    public interface IIdentifiableSet<TIdentifier, TElement> : IEnumerable<TElement>
    {
        TIdentifier Identifier
        {
            get;
        }
    }
}
