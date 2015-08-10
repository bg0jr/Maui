using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Reporting
{
    /// <summary>
    /// Generic report class. Content added by generic sections.
    /// </summary>
    public class Report
    {
        public Report( string name, string title )
        {
            Name = name;
            Title = title;
            Sections = new List<AbstractSection>();
        }

        public string Name
        {
            get;
            private set;
        }

        public string Title
        {
            get;
            private set;
        }

        public virtual IList<AbstractSection> Sections
        {
            get;
            private set;
        }
    }
}
