using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Dynamics
{
    /// <summary/>
    public class MauiX
    {
        /// <summary/>
        public interface IQuery { }
        /// <summary/>
        public interface IImport { }
        /// <summary/>
        public interface ICalc { }

        /// <summary/>
        public static IQuery Query { get; set; }
        /// <summary/>
        public static IImport Import { get; set; }
        /// <summary/>
        public static ICalc Calc { get; set; }
    }
}
