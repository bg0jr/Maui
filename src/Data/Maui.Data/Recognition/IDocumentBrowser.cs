using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Maui.Data.Recognition.Spec;

namespace Maui.Data.Recognition
{
    public interface IDocumentBrowser
    {
        /// <summary>
        /// Returns the document specified by given navigation.
        /// Supports searching with wildcards if only one url is given.
        /// </summary>
        IDocument GetDocument( Navigation navi );
    }
}
