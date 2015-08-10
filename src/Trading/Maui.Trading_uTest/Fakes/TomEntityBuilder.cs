using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Entities;

namespace Maui.Trading.UnitTests.Fakes
{
    public class TomEntityBuilder
    {
        public static StockHandle CreateStockHandle( string companyName, string isin )
        {
            return CreateStockHandle( companyName, isin, "Euro" );
        }

        public static StockHandle CreateStockHandle( string companyName, string isin, string currency )
        {
            var company = new Company( companyName );
            var stock = new Stock( company, isin );
            var exchange = new StockExchange( "Xetra", "DE", new Currency( currency, currency ) );
            var tradedStock = new TradedStock( stock, exchange );

            return new StockHandle( tradedStock );
        }
    }
}
