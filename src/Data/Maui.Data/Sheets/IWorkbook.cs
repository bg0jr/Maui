using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Data.Sheets
{
    /// <summary>
    /// A workbook is similar as an excel workbook (a set of worksheets).
    /// Its just an abstraction to different implementations.
    /// </summary>
    public interface IWorkbook
    {
        /// <summary/>
        IEnumerable<IWorksheet> Worksheets { get; }

        /// <summary/>
        void Save();

        /// <summary>
        /// Configuration of the workbook. If non exists a default
        /// config will be created.
        /// </summary>
        WorkbookConfig Config { get; }
    }
}
