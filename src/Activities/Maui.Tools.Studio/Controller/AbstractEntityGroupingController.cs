using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui;
using Maui.Tools.Studio.ViewModel;
using Maui.Logging;

namespace Maui.Tools.Studio.Controller
{
    public abstract class AbstractEntityGroupingController : IEntityGroupingController
    {
        private EntityGroupingManager myGroupingMgr;

        protected AbstractEntityGroupingController( EntityGroupingManager groupingManager )
        {
            myGroupingMgr = groupingManager;
        }

        public bool IsGroupingAllowed( object element, object group )
        {
            return myGroupingMgr.IsGroupingAllowed( element, group );
        }

        public void ReleaseGrouping( object group, object element )
        {
            myGroupingMgr.ReleaseGrouping( group, element );
        }

        public void MoveElementToGroup( object group, object element, object oldGroup )
        {
            myGroupingMgr.MoveElementToGroup( group, element, oldGroup );
        }

        public void AddElemntToGroup( object group, object element )
        {
            myGroupingMgr.AddElemntToGroup( group, element );
        }

        public abstract void CreateGroup();

        public abstract void Delete( object group, object item );
    }
}
