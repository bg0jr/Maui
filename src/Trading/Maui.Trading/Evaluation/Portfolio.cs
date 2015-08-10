using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Entities;

namespace Maui.Trading.Evaluation
{
    public class Portfolio
    {
        private double myCash;
        private IBroker myBroker;
        private int myQuantity;
        private TradingLog myTradingLog;

        public Portfolio( double cash, IBroker broker, TradingLog tradingLog )
        {
            myCash = cash;
            myBroker = broker;
            myTradingLog = tradingLog;

            myQuantity = 0;
        }

        internal void Buy( DateTime timestamp, double price )
        {
            int quantity = (int)Math.Floor( myCash / price );
            if ( quantity < 1 )
            {
                // not able to trade
                return;
            }

            var order = Order.Buy( timestamp, price, quantity, myBroker );
            double remainingCash = myCash - order.BruttoValue - order.Fee;

            while ( remainingCash < 0 )
            {
                --quantity;

                order = Order.Buy( timestamp, price, quantity, myBroker );
                remainingCash = myCash - order.BruttoValue - order.Fee;
            }

            myQuantity = quantity;
            myCash = remainingCash;

            myTradingLog.Add( order );
        }

        internal void Sell( DateTime timestamp, double price )
        {
            if ( myQuantity == 0 )
            {
                return;
            }

            var order = Order.Sell( timestamp, price, myQuantity, myBroker );

            myCash = order.NettoValue;

            myTradingLog.Add( order );

            myQuantity = 0;
        }

        internal double GetValue( double price )
        {
            double fee = myBroker.CalculateCommission( myQuantity, price );
            return myQuantity * price - fee + myCash;
        }
    }
}
