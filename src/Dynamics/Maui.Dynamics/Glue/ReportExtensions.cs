using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Dynamics
{
    public static class ReportExtensions
    {
        public static string ToH( this object obj )
        {
            if ( obj is float || obj is double )
            {
                return string.Format( "{0:####0.00}", obj );
            }
            else
            {
                return obj.ToString();
            }
        }
    }
}
