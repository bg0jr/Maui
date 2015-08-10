using System;
using System.Linq;
using System.Text;
using Blade;

namespace Maui.Entities
{
    public partial class Stock
    {
        private StockType myType;

        public Stock()
        {
            Type = StockType.None;
        }

        public Stock( Company company, string isin )
        {
            if ( company == null )
            {
                throw new ArgumentNullException( "company" );
            }

            Company = company;
            Isin = isin;
        }

        partial void OnIsinChanging( string value )
        {
            if ( string.IsNullOrWhiteSpace( value ) )
            {
                throw new ArgumentException( "Isin must not be null, empty or whitespace only" );
            }
        }

        partial void OnIsinChanged()
        {
            var trimmedValue = Isin.TrimOrNull();
            if ( trimmedValue != Isin )
            {
                Isin = trimmedValue;
            }
        }

        public string Symbol
        {
            get
            {
                var ts = TradedStocks.FirstOrDefault();

                return ts != null ? ts.Symbol : null;
            }
        }

        public string Wpkn
        {
            get
            {
                var ts = TradedStocks.FirstOrDefault();

                return ts != null ? ts.Wpkn : null;
            }
        }

        public StockType Type
        {
            get
            {
                return myType;
            }
            set
            {
                myType = value;
                RawType = (long)myType;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append( "Stock(" );
            sb.Append( "ID = " );
            sb.Append( Id );
            sb.Append( ";ISIN = " );
            sb.Append( Isin );
            sb.Append( ";Type = " );
            sb.Append( Type );
            sb.Append( ")" );

            return sb.ToString();
        }
    }
}
