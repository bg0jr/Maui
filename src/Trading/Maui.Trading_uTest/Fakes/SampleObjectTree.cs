using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.UnitTests.Fakes
{
    public class SampleObjectTree
    {
        public string Name
        {
            get;
            private set;
        }

        public Child Child
        {
            get;
            private set;
        }
    }

    public class Child
    {
        public int Value
        {
            get;
            private set;
        }
    }
}
