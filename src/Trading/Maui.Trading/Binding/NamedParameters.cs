using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;

namespace Maui.Trading.Binding
{
    /// <summary>
    /// Parameter names are handled ignoring case.
    /// </summary>
    public class NamedParameters : CaseInsensitiveMap<object>
    {
        public static readonly NamedParameters Null = new NamedParameters();

        public T GetParameter<T>( string name )
        {
            if ( !ContainsKey( name ) )
            {
                throw new ArgumentException( "No such parameter: " + name );
            }

            return (T)this[ name ];
        }
    }
}
