using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Maui.Trading.Model;
using Maui.Entities;
using Maui.Trading.Utils;
using Maui;

namespace Maui.Trading.Binding.Tom
{
    public class TomStockPricesDataSource : IEnumerableDataSource<SimplePrice>
    {
        public string Name
        {
            get
            {
                return DataSourceNames.Prices;
            }
        }

        IEnumerable<SimplePrice> IEnumerableDataSource<SimplePrice>.ForStock( StockHandle stock )
        {
            return FetchPrices( stock );
        }

        private IEnumerable<SimplePrice> FetchPrices( StockHandle stock )
        {
            using ( var tom = Engine.ServiceProvider.CreateEntityRepository() )
            {
                // refetch traded stock as the tom context the stock is coming from might be disposed already
                var tradedStock = tom.GetObjectByKey<TradedStock>( stock.TradedStock.EntityKey );

                var prices = tradedStock.StockPrices
                    .Select( price => new SimplePrice( price.Date, price.Close ) )
                    .ToList();

                return prices;
            }
        }
    }
}
