using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Dynamics
{
    internal class PythonScript : IMslScript
    {
        public PythonScript( string scriptFile, IList<string> args )
        {
            File = scriptFile;
            Arguments = args;
        }

        public string File { get; private set; }
        public IList<string> Arguments { get; private set; }
    }
}
