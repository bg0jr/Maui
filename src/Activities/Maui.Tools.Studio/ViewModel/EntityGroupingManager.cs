using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Maui.Tools.Studio.ViewModel
{
    public class EntityGroupingManager
    {
#pragma warning disable 649
        [ImportMany]
        private List<Grouping> myGroupings;
#pragma warning restore 649

        public bool IsGroupingAllowed( object element, object group )
        {
            if ( element == group )
            {
                // recursive self-grouping is not allowed
                return false;
            }

            return myGroupings.SingleOrDefault( g => g.Accepts( group, element ) ) != null;
        }

        public void ReleaseGrouping( object group, object element )
        {
            if ( element == group )
            {
                throw new NotSupportedException( "Group and element are the same instance" );
            }

            var grouping = myGroupings.Single( g => g.Accepts( group, element ) );
            grouping.Release( group, element );
        }

        public void MoveElementToGroup( object group, object element, object oldGroup )
        {
            if ( element == group )
            {
                throw new NotSupportedException( "Group and element are the same instance" );
            }

            var grouping = myGroupings.Single( g => g.Accepts( group, element ) );
            grouping.MoveElementToGroup( group, element, oldGroup );
        }

        public void AddElemntToGroup( object group, object element )
        {
            if ( element == group )
            {
                throw new NotSupportedException( "Group and element are the same instance" );
            }

            var grouping = myGroupings.Single( g => g.Accepts( group, element ) );
            grouping.AddElemntToGroup( group, element );
        }
    }
}
