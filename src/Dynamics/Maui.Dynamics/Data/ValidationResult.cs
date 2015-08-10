using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Dynamics.Data
{
    [Flags]
    public enum ValidationResult
    {
        None = 0x0,
        OwnerIdRequired = 0x1,
        DateRequired = 0x2,
        ValueColumnsMissing = 0x4
    }
}
