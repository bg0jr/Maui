using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Proxies;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Security;

namespace Maui.Tasks.Sheets
{
    [SecurityCritical]
    internal class PrePostActionDecorator : RealProxy
    {
        private static readonly MethodInfo GetTypeMethod;

        private object myInstance;
        private Action myMethodEnterAction;
        private Action myMethodLeaveAction;

        static PrePostActionDecorator()
        {
            GetTypeMethod = typeof( object ).GetMethod( "GetType", new Type[] { } );
        }

        public PrePostActionDecorator( Type type, object instance, Action methodEnterAction, Action methodLeaveAction )
            : base( type )
        {
            myInstance = instance;
            myMethodEnterAction = methodEnterAction ?? NoAction;
            myMethodLeaveAction = methodLeaveAction ?? NoAction;
        }

        private static void NoAction()
        {
        }

        [SecurityCritical]
        public override IMessage Invoke( IMessage msg )
        {
            var call = new MethodCall( msg );

            if ( call.MethodBase.DeclaringType == typeof( object ) && call.MethodBase.Equals( GetTypeMethod ) )
            {
                return new ReturnMessage( myInstance.GetType(), new object[] { }, 0, call.LogicalCallContext, call );
            }

            myMethodEnterAction();

            var returnValue = call.MethodBase.Invoke( myInstance, call.Args );

            myMethodLeaveAction();

            return new ReturnMessage( returnValue, call.Args, call.Args.Length, call.LogicalCallContext, call );
        }
    }

    /// <summary/>
    [SecuritySafeCritical]
    public class PrePostActionDecoratorFactory
    {
        /// <summary/>
        public static T CreateDecorator<T>( T instance, Action methodEnterAction )
        {
            return CreateDecorator<T>( instance, methodEnterAction, null );
        }

        /// <summary/>
        public static T CreateDecorator<T>( T instance, Action methodEnterAction, Action methodLeaveAction )
        {
            var proxy = new PrePostActionDecorator( typeof( T ), instance, methodEnterAction, methodLeaveAction );
            return (T)proxy.GetTransparentProxy();
        }
    }
}
