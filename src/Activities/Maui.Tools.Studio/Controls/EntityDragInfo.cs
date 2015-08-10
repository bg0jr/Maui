using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Maui.Tools.Studio.Controls
{
    public class EntityDragInfo
    {
        public static readonly string DataFormat = "EntityDragInfo";

        public EntityDragInfo( object entity )
        {
            Entity = entity;
        }

        public object Entity
        {
            get;
            private set;
        }

        public DragDropEffects Action
        {
            get;
            set;
        }

        public DataObject ToDataObject()
        {
            return new DataObject( DataFormat, this );
        }
    }
}
