using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui
{
    /// <summary>
    /// An initializer class name has to follow the convention:
    /// Step_[order]_[description]
    /// The order should be a number of two digits. Valid ranges:
    /// 01-10 very basic services like DB,Config, Logging
    /// 11-50 other framework services ( actually the order should not important here)
    /// 51-99 application specific services
    /// 
    /// An "_" in the description will be treated as space.
    /// </summary>
    public abstract class AbstractEngineInitializer
    {
        public abstract void Init();
        public virtual void Fini()
        {
            // nothing to do
        }
    }
}
