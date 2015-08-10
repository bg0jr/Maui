using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Maui.Reflection;
using Blade.Reflection;

namespace Maui.Trading.Binding
{
    public class Field : Member
    {
        public Field( object instance, FieldInfo fieldInfo )
            : base( instance, fieldInfo )
        {
            if ( fieldInfo == null )
            {
                throw new ArgumentNullException( "fieldInfo" );
            }

            FieldInfo = fieldInfo;
        }

        public FieldInfo FieldInfo
        {
            get;
            private set;
        }

        public override object Value
        {
            get
            {
                return FieldInfo.GetValue( Instance );
            }
            set
            {
                FieldInfo.SetValue( Instance, value );
            }
        }

        public override DataSourceAttribute GetDataSourceAttribute()
        {
            MemberInfo memberToCheckForAttribute = FieldInfo;

            if ( FieldInfo.IsBackingField() )
            {
                memberToCheckForAttribute = FieldInfo.GetPropertyForBackingField();
            }

            return memberToCheckForAttribute.GetAttribute<DataSourceAttribute>( false );
        }

        public override Type ReturnType
        {
            get
            {
                return FieldInfo.FieldType;
            }
        }

        protected override string GetDataSourceNameFromMemberName()
        {
            return FieldInfo.IsBackingField() ? FieldInfo.GetPropertyNameForBackingField() : FieldInfo.Name;
        }
    }
}
