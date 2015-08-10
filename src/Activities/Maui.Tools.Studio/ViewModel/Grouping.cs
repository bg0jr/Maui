using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Tools.Studio.ViewModel
{
    public abstract class Grouping
    {
        public Grouping( Type groupType, Type elementType )
        {
            GroupType = groupType;
            ElementType = elementType;
        }

        public Type GroupType
        {
            get;
            private set;
        }

        public Type ElementType
        {
            get;
            private set;
        }

        public bool Accepts( object group, object element )
        {
            if ( group == null || element == null )
            {
                return false;
            }

            return group.GetType() == GroupType && element.GetType() == ElementType;
        }

        public abstract void Release( object group, object element );

        public abstract void MoveElementToGroup( object group, object element, object oldGroup );

        public abstract void AddElemntToGroup( object group, object element );
    }

    public abstract class Grouping<TGroup, TElement> : Grouping
    {
        public Grouping()
            : base( typeof( TGroup ), typeof( TElement ) )
        {
        }

        public override void Release( object group, object element )
        {
            Release( (TGroup)group, (TElement)element );
        }

        protected abstract void Release( TGroup group, TElement element );

        public override void MoveElementToGroup( object group, object element, object oldGroup )
        {
            MoveElementToGroup( (TGroup)group, (TElement)element, (TGroup)oldGroup );
        }

        protected abstract void MoveElementToGroup( TGroup group, TElement element, TGroup oldGroup );

        public override void AddElemntToGroup( object group, object element )
        {
            AddElemntToGroup( (TGroup)group, (TElement)element );
        }

        protected abstract void AddElemntToGroup( TGroup group, TElement element );
    }
}
