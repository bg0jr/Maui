using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using Maui.Reflection;
using Blade.Reflection;

namespace Maui.Trading.Binding
{
    public class ObjectTreeWalker
    {
        private IObjectVisitor myVisitor;
        private BindingFlags myBindingOfInterest = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public ObjectTreeWalker( IObjectVisitor visitor )
        {
            myVisitor = visitor;
        }

        public void Visit( object instance )
        {
            if ( IsTraversableContainer( instance ) )
            {
                VisitContainerElements( instance );
            }
            else
            {
                VisitSingleObject( instance );
            }
        }

        private void VisitContainerElements( object instance )
        {
            var array = instance as Array;
            if ( array != null )
            {
                foreach ( var element in array )
                {
                    VisitSingleObject( element );
                }
            }
            else
            {
                foreach ( var element in (IEnumerable)instance )
                {
                    VisitSingleObject( element );
                }
            }
        }

        private void VisitSingleObject( object instance )
        {
            var members = GetAllMembers( instance, instance.GetType() );
            if ( !members.Any() )
            {
                return;
            }

            VisitMembers( members );

            VisitChildren( members );
        }

        private IEnumerable<Member> GetAllMembers( object instance, Type type )
        {
            if ( type == null || type == typeof( object ) )
            {
                return new List<Member>();
            }

            return GetFields( instance, type )
                .Concat( GetAllMembers( instance, type.BaseType ) )
                .ToList();
        }

        private IEnumerable<Member> GetFields( object instance, Type type )
        {
            var fields = type.GetFields( myBindingOfInterest )
                .Where( f => f.IsDeclaredByType( type ) )
                .Select( f => new Field( instance, f ) );
            return fields;
        }

        private void VisitMembers( IEnumerable<Member> members )
        {
            foreach ( var member in members )
            {
                myVisitor.Visit( member );
            }
        }

        private void VisitChildren( IEnumerable<Member> members )
        {
            var membersToStepDeeper = members
                .Where( m => IsTraversableMember( m ) )
                .Where( m => myVisitor.StepInto( m ) );

            var children = membersToStepDeeper
                .Select( m => m.Value );

            foreach ( var child in children )
            {
                Visit( child );
            }
        }

        private bool IsTraversableMember( Member member )
        {
            var value = member.Value;
            if ( value == null )
            {
                return false;
            }

            if ( typeof( ValueType ).IsAssignableFrom( value.GetType() ) )
            {
                return false;
            }

            if ( IsTraversableContainer( value ) )
            {
                return true;
            }

            if ( value.GetType().IsSystemType() )
            {
                return false;
            }

            return true;
        }

        private bool IsTraversableContainer( object value )
        {
            var type = value.GetType();
            if ( !type.IsCollection() )
            {
                return false;
            }

            if ( typeof( IDictionary<,> ).IsAssignableFrom( type ) )
            {
                // "Dictionaries not supported"
                return false;
            }

            var elementType = type.GetElementType();
            if ( elementType == null )
            {
                elementType = type.GetGenericArguments().First();
            }

            if ( elementType.Namespace.StartsWith( "System." ) )
            {
                return false;
            }

            return true;
        }
    }
}
