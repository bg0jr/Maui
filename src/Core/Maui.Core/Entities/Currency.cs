using System;
using Blade;

namespace Maui.Entities
{
    public partial class Currency
    {
        private Currency()
        {
        }

        public Currency( string name, string symbol )
        {
            Name = name;
            Symbol = symbol;
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

        partial void OnSymbolChanging( string value )
        {
            ValidateSymbol( value );
        }

        private static void ValidateSymbol( string value )
        {
            if ( string.IsNullOrWhiteSpace( value ) )
            {
                throw new ArgumentException( "Symbol must not be null, empty or whitespace only" );
            }
        }

        partial void OnSymbolChanged()
        {
            var trimmedValue = Symbol.TrimOrNull();
            if ( trimmedValue != Symbol )
            {
                Symbol = trimmedValue;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
