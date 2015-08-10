using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Entities
{
    /// <summary/>
    public interface ITimeframedValue<TValue>
    {
        /// <summary/>
        DateTime Date { get; }
        /// <summary/>
        TValue Value { get; }
    }
}
