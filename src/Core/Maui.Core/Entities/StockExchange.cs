using System;
using Blade;

namespace Maui.Entities
{
    public partial class StockExchange
    {
        private StockExchange()
        {
        }

        public StockExchange( string name, string symbol, Currency currency )
        {
            Name = name;
            Symbol = symbol;
            Currency = currency;
        }

        partial void OnNameChanging( string value )
        {
            if ( string.IsNullOrWhiteSpace( value ) )
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

        public bool MatchesNameOrSymbol( string nameOrSymbol )
        {
            return MatchesNameOrSymbol( nameOrSymbol, StringComparison.Ordinal );
        }

        public bool MatchesNameOrSymbol( string nameOrSymbol, StringComparison compareOptions )
        {
            if ( Name.Equals( nameOrSymbol, compareOptions ) )
            {
                return true;
            }
            if ( Symbol.Equals( nameOrSymbol, compareOptions ) )
            {
                return true;
            }
            return false;
        }
    }
}
