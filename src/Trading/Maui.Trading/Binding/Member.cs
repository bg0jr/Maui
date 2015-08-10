using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Maui.Reflection;
using Blade.Reflection;

namespace Maui.Trading.Binding
{
    public abstract class Member
    {
        protected Member( object instance, MemberInfo memberInfo )
        {
            if ( instance == null )
            {
                throw new ArgumentNullException( "instance" );
            }
            if ( memberInfo== null )
            {
                throw new ArgumentNullException( "memberInfo" );
            }

            Instance = instance;
            MemberInfo = memberInfo;
        }

        public object Instance
        {
            get;
            private set;
        }

        public MemberInfo MemberInfo
        {
            get;
            private set;
        }

        public abstract object Value
        {
            get;
            set;
        }

        public abstract Type ReturnType
        {
            get;
        }

        public virtual DataSourceAttribute GetDataSourceAttribute()
        {
            return MemberInfo.GetAttribute<DataSourceAttribute>( false );
        }

        public string DataSourceName
        {
            get
            {
                var attr = GetDataSourceAttribute();
                if ( attr.Datum != null )
                {
                    return attr.Datum;
                }

                return GetDataSourceNameFromMemberName();
            }
        }

        protected abstract string GetDataSourceNameFromMemberName();
    }
}
