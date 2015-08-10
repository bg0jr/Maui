using System;
using Maui;

namespace Maui.Entities
{
    public enum OrderType
    {
        MarketPrice,
        Limit,
        Stop,
        // <summary>
        // A plage de déclenchement (french market only).
        // (stop_limited)
        // </summary>
        //APD,
        /// <summary>
        /// Tout ou rien (french market)
        /// </summary>
        TR,
        /// <summary>
        /// A tout prix (french market)
        /// </summary>
        ATP,
        TheoricAtOpen,
        TheoricAtHigh,
        TheoricAtLow,
        TheoricAtClose,
    }

    partial class Order
    {
        /// <summary>
        /// XXXXX: how to handle this ? - we do not want to a standard constructor
        /// but EntityFramework forces us to have one
        /// </summary>
        public Order()
            :this(null,null)
        {
        }

        public Order( TradedStock code )
            : this( code, null )
        {
        }

        public Order( TradedStock code, string source )
        {
            Source = source;
            TradedStock = code;
        }

        public OrderType Type
        {
            get { return (OrderType)TypeInternal; }
            set { TypeInternal = (short)value; }
        }

        // maybe we should add some caching here
        public DateTime SubmissionDate
        {
            get { return TypeConverter.StringToDate( SubmissionDateInternal ); }
            set { SubmissionDateInternal = TypeConverter.DateToString( value ); }
        }
    }
}
