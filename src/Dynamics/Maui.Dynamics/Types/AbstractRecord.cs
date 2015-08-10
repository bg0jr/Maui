using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace Maui.Dynamics.Types
{
    /// <summary>
    /// Base class for record types.
    /// <remarks>
    /// TODO: do we really need this
    /// </remarks>
    /// </summary>
    public abstract class AbstractRecord
    {
        public abstract bool Contains( string key );
        public abstract object this[ string key ] { get; }
    }

}
