using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Data.Recognition.Spec
{
    public class Document
    {
        public Document()
        {
            DatumLocators = new List<DatumLocator>();
            Bits = new BitsCollection();
        }

        public BitsCollection Bits
        {
            get;
            private set;
        }

        public List<DatumLocator> DatumLocators
        {
            get;
            private set;
        }
    }
}
