using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Model
{
    /// <summary>
    /// Interface to an owner/handle/parent object.
    /// </summary>
    public interface IObjectIdentifier
    {
        /// <summary>
        /// Short description or symbol of the object which can be used to display it.
        /// Should not be any prosa but a short string representation of the object.
        /// e.g. in the debugger or an exception or s.th.
        /// </summary>
        string ShortDesignator { get; }

        /// <summary>
        /// Could be a complete description of the object. Should still not be any
        /// prosa but a complete string representation of the object including all
        /// parameters, arugments, options aso.
        /// </summary>
        string LongDesignator { get; }

        /// <summary>
        /// Uniq identifier of the object.
        /// </summary>
        int Guid { get; }
    }
}
