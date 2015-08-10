using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;
using Maui.Trading.Binding;
using Blade;

namespace Maui.Trading.Binding
{
    public class DataSourceAttribute : Attribute
    {
        public DataSourceAttribute()
        {
            Datum = null;
        }

        public string Datum
        {
            get;
            set;
        }

        public object[] ConstructorArugments
        {
            get;
            set;
        }

        public NamedParameters GetConstructorArguments()
        {
            if ( ConstructorArugments == null )
            {
                return NamedParameters.Null;
            }

            if ( ConstructorArugments.Length % 2 != 0 )
            {
                throw new ArgumentException( "ConstructorArguments must have even length" );
            }

            var args = new NamedParameters();

            for ( int i = 0; i < ConstructorArugments.Length; ++i )
            {
                args[ ConstructorArugments[ i ].ToString() ] = ConstructorArugments[ i + 1 ];
                i++;
            }

            AddTypedConstructorArugments( args );

            return args;
        }

        protected virtual void AddTypedConstructorArugments( NamedParameters args )
        {
            // hook for derived classes to add types arguments
        }
    }
}
