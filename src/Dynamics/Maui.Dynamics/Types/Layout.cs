using System;
using System.Text;
using System.Data;
using System.Xml.Linq;
using System.Reflection;
using System.IO;

namespace Maui.Dynamics.Types
{
    /// <summary>
    /// Defines a layout for the reporting engine.
    /// </summary>
    public class Layout : MslObject
    {
        /// <summary>
        /// Creates a new layout with the given name and stylesheet.
        /// </summary>
        public Layout( string name, string stylesheet )
            : base( name )
        {
            stylesheet = Interpreter.ResolveFile( stylesheet );
            if ( !File.Exists( stylesheet ) )
            {
                throw new FileNotFoundException( "stylesheet not found", stylesheet );
            }
            Stylesheet = stylesheet;
        }

        /// <summary>
        /// The stylesheet which implements the layout.
        /// </summary>
        public string Stylesheet { get; private set; }
    }
}
