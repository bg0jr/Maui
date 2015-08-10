using System.Collections.Generic;
using System.Data;
using System.Linq;
using Maui;

namespace Maui.Dynamics.Data
{
    public static class SeriesFactory
    {
        public static TimeSeries Create( TableSchema schema, IEnumerable<DataRow> rows, string valueColumn )
        {
            return new TimeSeries( rows.Select( row => 
                new TimeframedSingleValue( row.GetDate( schema ), (double)row[ valueColumn ] ) ) );
        }
    }
}
