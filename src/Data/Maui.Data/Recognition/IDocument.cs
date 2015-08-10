using System.Data;
using System;
using Maui.Data.Recognition.Spec;

namespace Maui.Data.Recognition
{
    public interface IDocument
    {
        string Location { get; }
        DataTable ExtractTable( IFormat format );
    }
}
