using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Binding
{
    public interface IObjectVisitor
    {
        void Visit( Member member );
        bool StepInto( Member member );
    }
}
