using System;
using System.Collections.Generic;
using System.Text;

namespace Maui
{
    public interface IServiceComponent : IDisposable
    {
        void Init( ServiceProvider serviceProvider );
    }
}
