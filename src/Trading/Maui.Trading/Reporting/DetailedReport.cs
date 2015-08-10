using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Reporting
{
    public class DetailedReport : Report
    {
        public DetailedReport( string reference, string name, string title )
            : base( name, title )
        {
            Reference = reference;
        }

        /// <summary>
        /// Defines the item the details belong to (e.g. stock isin)
        /// </summary>
        public string Reference
        {
            get;
            private set;
        }
    }
}
