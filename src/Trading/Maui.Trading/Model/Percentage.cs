using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Model
{
    public class Percentage : RangeValue, IComparable<Percentage>
    {
        public static readonly Percentage Zero = new Percentage( 0 );
        public static readonly Percentage Hundred = new Percentage( 100 );

        public Percentage( int value )
            : base( 0, 100, value )
        {
        }

        public int CompareTo( Percentage other )
        {
            if ( Value < other.Value )
            {
                return -1;
            }
            else if ( Value > other.Value )
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
