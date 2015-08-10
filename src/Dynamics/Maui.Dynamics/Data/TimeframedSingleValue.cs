using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Entities;

namespace Maui.Dynamics.Data
{
    /// <summary/>
    public class TimeframedSingleValue : ITimeframedValue<double>
    {
        /// <summary/>
        public TimeframedSingleValue( DateTime tf, double value )
        {
            Date = tf;
            Value = value;
        }

        /// <summary/>
        public DateTime Date { get; private set; }

        /// <summary/>
        public double Value { get; private set; }
    }
}
