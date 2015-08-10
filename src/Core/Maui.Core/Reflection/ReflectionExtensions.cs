using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Globalization;

namespace Maui.Reflection
{
    /// <summary/>
    public static class ReflectionExtensions
    {
        /// <summary/>
        public static bool IsDeclaredByType( this MemberInfo member, Type type )
        {
            return member.DeclaringType == type;
        }

        /// <summary/>
        public static bool IsBackingField( this FieldInfo field )
        {
            // <Name>k__BackingField
            return field.Name.StartsWith( "<" ) && field.Name.EndsWith( ">k__BackingField" );
        }

        /// <summary/>
        public static PropertyInfo GetPropertyForBackingField( this FieldInfo field )
        {
            var propertyName = field.GetPropertyNameForBackingField();

            return field.DeclaringType.GetProperty( propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );
        }

        /// <summary/>
        public static string GetPropertyNameForBackingField( this FieldInfo field )
        {
            // <Name>k__BackingField

            var nameEnd = field.Name.IndexOf( '>' );
            var propertyName = field.Name.Substring( 1, nameEnd - 1 );

            return propertyName;
        }

        /// <summary/>
        public static bool IsSystemType( this Type type )
        {
            return type.Namespace == "System" || type.Namespace.StartsWith( "System." );
        }
    }
}
