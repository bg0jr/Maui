using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Data.Recognition.Spec;

namespace Maui.Data.Recognition
{
    public interface IDatumProviderFactory
    {
        DatumLocatorRepository LocatorRepository { get; }
        IDatumProvider Create( string datum );
        IDatumProvider Create( DatumLocator datumLocator );
    }
}
