using Maui.Data.Sheets;

namespace Maui.Presentation.Excel
{
    /// <summary>
    /// Interface of the plugins to the outer-world.
    /// </summary>
    public interface IWorksheetContext
    {
        /// <summary/>
        IWorkbook ActiveWorkbook { get; }
    }
}
