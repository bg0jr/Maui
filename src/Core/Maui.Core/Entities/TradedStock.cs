using System;
using System.Text;
using Blade;
using Maui;

namespace Maui.Entities
{
    public partial class TradedStock
    {
        private TradedStock()
        {
        }

        public TradedStock( Stock stock, StockExchange se )
        {
            Stock = stock;
            StockExchange = se;
        }

        partial void OnSymbolChanging( string value )
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

        partial void OnWpknChanged()
        {
            var trimmedValue = Wpkn.TrimOrNull().ToNullIfEmpty();
            if ( trimmedValue != Wpkn )
            {
                Wpkn = trimmedValue;
            }
        }

        public string Isin
        {
            get
            {
                return Stock.Isin;
            }
        }

        public Company Company
        {
            get
            {
                return Stock.Company;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append( "TradedStock(" );
            sb.Append( "ID = " );
            sb.Append( Id );
            sb.Append( ";ISIN = " );
            sb.Append( Stock.Isin );
            sb.Append( ";WPKN = " );
            sb.Append( Wpkn );
            sb.Append( ";Symbol = " );
            sb.Append( Symbol );
            sb.Append( ")" );

            return sb.ToString();
        }
    }
}
