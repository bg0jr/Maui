using Maui.Data.Sheets;
using MSExcel = Microsoft.Office.Interop.Excel;

namespace Maui.Presentation.Excel
{
    /// <summary>
    /// Default plugin context implementation.
    /// Interface of the plugins to the outer-world.
    /// </summary>
    internal class PluginContext : IWorksheetContext
    {
        private MSExcel.Application myApplication;

        /// <summary/>
        public PluginContext( MSExcel.Application application )
        {
            myApplication = application;
        }

        /// <summary/>
        public IWorkbook ActiveWorkbook
        {
            get { return new ExcelWorkbook( myApplication ); }
        }
    }
}
