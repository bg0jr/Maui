using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Entities;

namespace Maui.Dynamics.Data
{
    /// <summary/>
    public class TimeSeries : GenericTimeSeries<double>
    {
        /// <summary/>
        public TimeSeries( IEnumerable<TimeframedSingleValue> values )
            : base( values.Cast<ITimeframedValue<double>>() )
        {
        }
    }
}
