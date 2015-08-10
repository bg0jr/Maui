using System;
using System.Xml.Linq;
using System.Reflection;
using Blade;
using Maui;

namespace Maui.Dynamics.Types
{
    /// <summary>
    /// Base object for all MSL objects.
    /// <remarks>
    /// This will be removed later when the MSL.XML support has been stopped.
    /// </remarks>
    /// </summary>
    public class MslObject : INamedObject
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MslObject( string name )
        {
            this.Require( x => !string.IsNullOrEmpty( name ) );

            Name = name;
        }

        /// <summary>
        /// Name of the object
        /// </summary>
        public string Name { get; private set; }
    }
}
