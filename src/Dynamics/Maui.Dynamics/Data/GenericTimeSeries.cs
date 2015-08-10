using System.Collections.Generic;
using Maui.Entities;

namespace Maui.Dynamics.Data
{
    /// <summary/>
    public class GenericTimeSeries<T> : AbstractTimeSeries<ITimeframedValue<T>, T>
    {
        /// <summary/>
        public GenericTimeSeries( IEnumerable<ITimeframedValue<T>> values )
            : base( values )
        {
        }
    }

}
