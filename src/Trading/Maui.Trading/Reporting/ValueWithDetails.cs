using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Reporting
{
    public class ValueWithDetails
    {
        public ValueWithDetails( object value, DetailedReport detailedReport )
        {
            Value = value;
            Details = detailedReport;
        }

        public object Value
        {
            get;
            private set;
        }

        public DetailedReport Details
        {
            get;
            private set;
        }
    }
}
