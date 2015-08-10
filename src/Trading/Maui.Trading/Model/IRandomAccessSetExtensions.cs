using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Model
{
    public static class IRandomAccessSetExtensions
    {
        public static bool Any<T>( this IRandomAccessSet<T> self )
        {
            return self.Count != 0;
        }

        public static T First<T>( this IRandomAccessSet<T> self )
        {
            if ( !self.Any() )
            {
                throw new InvalidOperationException( "Sequence contains no elements" );
            }
            return self[ 0 ];
        }

        public static T FirstOrDefault<T>( this IRandomAccessSet<T> self )
        {
            if ( !self.Any() )
            {
                return default( T );
            }
            return self[ 0 ];
        }

        public static T Last<T>( this IRandomAccessSet<T> self )
        {
            if ( !self.Any() )
            {
                throw new InvalidOperationException( "Sequence contains no elements" );
            }
            return self[ self.Count - 1 ];
        }

        public static T LastOrDefault<T>( this IRandomAccessSet<T> self )
        {
            if ( !self.Any() )
            {
                return default( T );
            }
            return self[ self.Count - 1 ];
        }
    }
}
