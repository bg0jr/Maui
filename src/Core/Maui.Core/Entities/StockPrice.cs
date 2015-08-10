using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui;

namespace Maui.Entities
{
    public partial class StockPrice : ITimeframedValue<StockPrice>
    {
        private StockPrice()
        {
        }

        public StockPrice( long id, TradedStock ts, DateTime date, double? open, double? high, double? low, double close, long? volume )
            : this( ts, date, open, high, low, close, volume )
        {
            Id = id;
        }

        public StockPrice( TradedStock ts, DateTime date, double? open, double? high, double? low, double close, long? volume )
        {
            TradedStock = ts;

            Date = date;

            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
        }

        // maybe we should add some caching here
        public DateTime Date
        {
            get { return TypeConverter.StringToDate( DateInternal ); }
            set { DateInternal = TypeConverter.DateToString( value ); }
        }

        public double? First { get { return Open; } }

        public double Last { get { return Close; } }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append( "StockPrice(" );
            sb.Append( "ID = " );
            sb.Append( Id );
            sb.Append( ";Date = " );
            sb.Append( Date );
            sb.Append( ";Close = " );
            sb.Append( Close );
            sb.Append( ")" );

            return sb.ToString();
        }

        public StockPrice Value
        {
            get { return this; }
        }
    }
}
