using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui
{
    public interface IBannerInfoProvider
    {
        KeyValuePair<string, string> GetBannerInformation();
    }
}
