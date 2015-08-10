using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Entities;

namespace Maui.Trading.Model
{
    public class StockObjectIdentifier : IObjectIdentifier
    {
        public StockObjectIdentifier( StockHandle stock )
        {
            Isin = stock.Isin;
            StockExchangeSymbol = stock.StockExchange.Symbol;

            ShortDesignator = Isin + "+" + StockExchangeSymbol;
            LongDesignator = ShortDesignator;
            Guid = LongDesignator.GetHashCode();
        }

        public string Isin
        {
            get;
            private set;
        }

        public string StockExchangeSymbol
        {
            get;
            private set;
        }

        public string ShortDesignator
        {
            get;
            private set;
        }

        public string LongDesignator
        {
            get;
            private set;
        }

        public int Guid
        {
            get;
            private set;
        }
    }
}
