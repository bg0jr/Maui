using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Maui.Trading.Binding
{
    public class Property : Member
    {
        public Property( object instance, PropertyInfo propertyInfo )
            : base( instance, propertyInfo )
        {
            PropertyInfo = propertyInfo;
        }

        public PropertyInfo PropertyInfo
        {
            get;
            private set;
        }

        public override object Value
        {
            get
            {
                return PropertyInfo.GetValue( Instance, null );
            }
            set
            {
                PropertyInfo.SetValue( Instance, value, null );
            }
        }

        public override Type ReturnType
        {
            get
            {
                return PropertyInfo.PropertyType;
            }
        }

        protected override string GetDataSourceNameFromMemberName()
        {
            return PropertyInfo.Name;
        }
    }
}
