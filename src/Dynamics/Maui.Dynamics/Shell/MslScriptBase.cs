using System.Collections.Generic;
using Blade.Shell.Forms;
using Maui.Shell;
using Blade.Shell;

namespace Maui.Dynamics.Shell
{
    public abstract class MslScriptBase : ScriptBase, IMslScript
    {
        protected MslScriptBase()
        {
            Variables = new Dictionary<string, string>();
        }

        [Argument( Short = "-D", Description = "sets a variable to the MSL script" )]
        public IDictionary<string, string> Variables
        {
            get;
            private set;
        }

        protected override void Run()
        {
            var interpreter = new Interpreter();
            interpreter.Init( Engine.ServiceProvider );

            // just register it so that it can be cleaned up later
            Engine.ServiceProvider.RegisterService( interpreter.GetType(), interpreter );

            interpreter.Start( Variables );

            Interpret();
        }

        protected abstract void Interpret();
    }
}
