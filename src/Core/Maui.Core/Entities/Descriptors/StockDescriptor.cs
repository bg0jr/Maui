using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Maui.Entities.Descriptors
{
    public class StockDescriptor
    {
        public StockDescriptor()
            : this( null, null )
        {
        }

        public StockDescriptor( string isin )
            : this( isin, null )
        {
        }

        public StockDescriptor( string isin, string stockExchange )
        {
            Isin = isin;
            StockExchange = stockExchange ?? "F";
        }

        /// <summary>
        /// Usually only used to give the descriptor a human readable description.
        /// e.g. when used in config binding. Ignored when the stock is already
        /// existing in the DB and replaced by company name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        [Required]
        public string Isin
        {
            get;
            set;
        }

        /// <summary>
        /// Name or symbol of the stock exchange
        /// Default: Frankfurt
        /// </summary>
        public string StockExchange
        {
            get;
            set;
        }

        /// <summary>
        /// Symbol used for TradedStock
        /// </summary>
        public string Symbol
        {
            get;
            set;
        }
    }
}
