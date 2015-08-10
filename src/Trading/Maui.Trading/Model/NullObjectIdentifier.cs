using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Model
{
    public class NullObjectIdentifier : IObjectIdentifier
    {
        public static readonly NullObjectIdentifier Null = new NullObjectIdentifier();

        private NullObjectIdentifier()
        {
        }
        public string ShortDesignator
        {
            get { return string.Empty; }
        }

        public string LongDesignator
        {
            get { return string.Empty; }
        }

        public int Guid
        {
            get { return 0; }
        }
    }
}
