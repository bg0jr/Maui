using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blade.Reflection;
using Maui;

namespace Maui.UnitTests.Configuration
{
    public static class ObjectTransactionExtensions
    {
        public static void Register( this ObjectTransaction trans, Type ns, string key )
        {
            trans.Register( Engine.ServiceProvider.ConfigurationSC().CreateAccessor( ns, key ) );
        }

        public static void Override( this ObjectTransaction trans, Type ns, string key, string value )
        {
            trans.Override( Engine.ServiceProvider.ConfigurationSC().CreateAccessor( ns, key ), value );
        }
    }
}
