using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Reporting.Rendering
{
    public interface IRenderAction<T>
    {
        void Render( T reportNode, IRenderingContext context );
    }
}
