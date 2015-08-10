using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Model
{
    public class ClosedInterval
    {
        public static ClosedInterval<int> FromMinMax( int min, int max )
        {
            return new ClosedInterval<int>( min, max );
        }

        public static ClosedInterval<int> FromOffsetLength( int offset, int length )
        {
            if ( length == 0 )
            {
                return new ClosedInterval<int>();
            }

            int min = offset;
            int max = offset + length - 1;

            if ( min > max )
            {
                int t = min;
                min = max;
                max = t;
            }

            return new ClosedInterval<int>( min, max );
        }

        public static ClosedInterval<int> FromDynamic<T>( IEnumerable<T> set, Func<T, int, bool> skipWhile, Func<T, int, bool> takeWhile )
        {
            var list = set.ToList();
            int? min = null;
            int? max = null;

            for ( int i = 0; i < list.Count; ++i )
            {
                if ( skipWhile( list[ i ], i ) )
                {
                    continue;
                }

                if ( !takeWhile( list[ i ], i ) )
                {
                    break;
                }

                if ( min == null )
                {
                    min = i;
                }
                max = i;
            }

            if ( min == null || max == null )
            {
                throw new ArgumentOutOfRangeException( "Cannot create indices for the given set" );
            }

            return new ClosedInterval<int>( min.Value, max.Value );
        }

        public static ClosedInterval<int> FromDynamic<T>( IEnumerable<T> set, Func<T, bool> skipWhile, Func<T, bool> takeWhile )
        {
            return FromDynamic( set, ( item, index ) => skipWhile( item ), ( item, index ) => takeWhile( item ) );
        }
    }
}
