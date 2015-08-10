using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.UnitTests.Fakes
{
    public class ClassWithListOfComplexTypes
    {
        public string Name
        {
            get;
            private set;
        }

        public IEnumerable<ComplexType> Values
        {
            get;
            private set;
        }
    }
}
