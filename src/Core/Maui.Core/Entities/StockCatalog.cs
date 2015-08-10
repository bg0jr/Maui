using System;
using System.Text;
using Blade;
using Maui;

namespace Maui.Entities
{
    public partial class StockCatalog
    {
        public StockCatalog()
        {
        }

        public StockCatalog( string name )
        {
            Name = name;
        }

        partial void OnNameChanging( string value )
        {
            ValidateName( value );
        }

        private static void ValidateName( string name )
        {
            if ( string.IsNullOrWhiteSpace( name ) )
            {
                throw new ArgumentException( "Name must not be null, empty or whitespace only" );
            }
        }

        partial void OnNameChanged()
        {
            var trimmedValue = Name.TrimOrNull();
            if ( trimmedValue != Name )
            {
                Name = trimmedValue;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append( "StockCatalog(" );
            sb.Append( "ID = " );
            sb.Append( Id );
            sb.Append( ";Name = " );
            sb.Append( Name );
            sb.Append( ")" );

            return sb.ToString();
        }
    }
}
