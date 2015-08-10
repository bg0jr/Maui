using System.Collections.Generic;
using System.Data;

namespace Maui.Dynamics.Data
{
    public delegate IEnumerable<DataRow> RowFilter( IEnumerable<DataRow> rows );

    public static class RowFilterExtensions
    {
        /// <summary>
        /// Does rows = F1( F2( rows ) )
        /// </summary>
        public static RowFilter Compose( this RowFilter f1, RowFilter f2 )
        {
            return a => f1( f2( a ) );
        }
    }
}
