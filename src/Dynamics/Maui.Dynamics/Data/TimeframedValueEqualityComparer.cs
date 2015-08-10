using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Entities;

namespace Maui.Dynamics.Data
{
    /// <summary/>
    public class TimeframedValueEqualityComparer<T, TElement> : IEqualityComparer<T> where T : ITimeframedValue<TElement>
    {
        /// <summary/>
        public bool Equals( T x, T y )
        {
            return x.Date == y.Date;
        }

        /// <summary/>
        public int GetHashCode( T obj )
        {
            return obj.Date.GetHashCode();
        }
    }
}
