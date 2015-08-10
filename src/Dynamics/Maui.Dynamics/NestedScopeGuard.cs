using System;
using Maui.Dynamics;

namespace Maui.Dynamics
{
    public sealed class NestedScopeGuard : IDisposable
    {
        public NestedScopeGuard()
        {
            Interpreter.Context.Scope = new Scope( Interpreter.Context.Scope );
        }

        public Scope Scope
        {
            get
            {
                return Interpreter.Context.Scope;
            }
        }

        public void Dispose()
        {
            Interpreter.Context.Scope = Interpreter.Context.Scope.Parent;
        }
    }
}
