using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Data.Sheets
{
    /// <summary>
    /// A worksheet is a cell-based data sheet.
    /// It can be read and written cell-based.
    /// Cell positions are specified excel like (e.g.
    /// "A2" means first column and second row)
    /// </summary>
    public interface IWorksheet
    {
        /// <summary/>
        string Name { get; }

        /// <summary/>
        object GetCell( string position );

        /// <summary/>
        T GetCell<T>( string position );

        /// <summary/>
        void SetCell( string position, object value );

        /// <summary/>
        bool IsEmptyCell( object cell );

        /// <summary/>
        void Save();

        /// <summary/>
        int GetRowCount();

        /// <summary/>
        int GetColumnCount();
    }
}
