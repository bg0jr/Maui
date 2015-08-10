using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Entities;

namespace Maui.Trading.Evaluation
{
    public enum OrderType
    {
        None,
        Buy,
        Sell
    }

    public class Order
    {
        public Order( OrderType type, DateTime timestamp, double price, int quantity, double fee )
        {
            Type = type;
            Timestamp = timestamp;
            Price = price;
            Quantity = quantity;
            Fee = fee;
        }

        public OrderType Type { get; private set; }
        public DateTime Timestamp { get; private set; }
        public double Price { get; private set; }
        public int Quantity { get; private set; }
        public double Fee { get; private set; }

        public double BruttoValue
        {
            get
            {
                return Price * Quantity;
            }
        }

        public double NettoValue
        {
            get
            {
                return BruttoValue - Fee;
            }
        }

        public static Order Buy( DateTime timestamp, double price, int quantity, IBroker broker )
        {
            double fee = broker.CalculateCommission( quantity, price );
            return new Order( OrderType.Buy, timestamp, price, quantity, fee );
        }

        public static Order Sell( DateTime timestamp, double price, int quantity, IBroker broker )
        {
            double fee = broker.CalculateCommission( quantity, price );
            return new Order( OrderType.Sell, timestamp, price, quantity, fee );
        }
    }
}
