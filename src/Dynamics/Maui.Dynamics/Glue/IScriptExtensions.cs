using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Dynamics
{
    public static class IScriptExtensions
    {
        public static Scope Scope( this IMslScript script )
        {
            return Interpreter.Context.Scope;
        }
    }
}
