using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.UnitTests.Fakes
{
    public class ClassWithArray
    {
        public string Name
        {
            get;
            private set;
        }

        public int[] Values
        {
            get;
            private set;
        }
    }

    public class ClassWithList
    {
        public string Name
        {
            get;
            private set;
        }

        public List<string> Values
        {
            get;
            private set;
        }
    }
}
