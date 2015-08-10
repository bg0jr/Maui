using System;

namespace Maui.Presentation.Excel
{
    /// <summary>
    /// Interface for abstract toolbar addin description.
    /// </summary>
    public interface IToolbarButton
    {
        /// <summary/>
        string Caption { get; }
        
        /// <summary>
        /// see: http://www.outlookexchange.com/articles/toddwalker/BuiltInOLKIcons.asp
        /// </summary>
        int FaceId { get; }

        /// <summary/>
        Action OnClickHandler { get; }
    }
}
