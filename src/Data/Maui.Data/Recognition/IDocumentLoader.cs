using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Data.Recognition
{
    public interface IDocumentLoader
    {
        IDocument Load( Uri uri );
    }
}
