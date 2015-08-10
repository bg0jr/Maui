using System;
using System.Collections.Generic;
using System.Data;

namespace Maui.Dynamics.Data
{
    public class DateIdCache<TId>
    {
        private Dictionary<DateTime, TId> myCache = null;

        public DateIdCache()
        {
            myCache = new Dictionary<DateTime, TId>();
        }

        public void Fill( ScopedTable table )
        {
            foreach ( DataRow row in table.Rows )
            {
                DateTime date = row.GetDate( table.Schema );
                if ( myCache.ContainsKey( date ) )
                {
                    throw new InvalidOperationException( "date is not uniq for stock and origin" );
                }
                myCache[ date ] = row.Field<TId>( table.Schema.IdColumn );
            }
        }

        public void Clear()
        {
            myCache.Clear();
        }

        public bool Contains( DateTime date )
        {
            return myCache.ContainsKey( date );
        }

        public TId this[ DateTime date ]
        {
            get
            {
                return myCache[ date ];
            }
        }
    }
}
