using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Data.Recognition.Html
{
    public interface IHtmlDocument
    {
        Uri Url { get; }

        IHtmlElement Body { get; }
    }
}
