using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Evaluation
{
    public class TradingLog
    {
        private IList<Order> myOrders;

        public TradingLog()
        {
            myOrders = new List<Order>();
        }

        public IEnumerable<Order> Orders
        {
            get
            {
                return myOrders;
            }
        }

        public void Add( Order order )
        {
            myOrders.Add( order );
        }
    }
}
