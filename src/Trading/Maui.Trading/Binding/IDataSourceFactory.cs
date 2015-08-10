using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Binding
{
    public interface IDataSourceFactory
    {
        bool CanCreate( string name, Type type );
        T Create<T>( string name, NamedParameters ctorArgs);
        object Create( string name, Type type, NamedParameters ctorArgs );
    }
}
