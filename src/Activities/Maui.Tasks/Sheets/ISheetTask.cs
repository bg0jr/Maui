using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Data.Sheets;

namespace Maui.Tasks.Sheets
{
    /// <summary>
    /// Marks a script as "sheet script" and defines its interface.
    /// </summary>
    public interface ISheetTask 
    {
        /// <summary>
        /// Open/activate the sheet. At the time of calling the environment
        /// for the sheet has been set up and the sheet can start working.
        /// </summary>
        void Open( IWorkbook workbook );

        /// <summary>
        /// Close/deactivate the sheet. The sheet needs to stop all activities
        /// at this point so that the environment can be shut down.
        /// </summary>
        void Close();

        /// <summary>
        /// Calculates dynamic content of the sheet.
        /// </summary>
        void Calculate();

        /// <summary>
        /// Allows the script sheet to define a set of result sheets.
        /// Optional. If a script does not implement this property, 
        /// e.g. no console output will be generated.
        /// </summary>
        IEnumerable<string> ResultSheets { get; }
    }
}
