using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Entities.Modules
{
    public class DibaBroker : IBroker
    {
        public double CalculateCommission( int quantity, double price )
        {
            // in euros
            const double courtage = 0.0025d;
            const double minCourtage = 9.9d;
            const double maxCourtage = 49.9d;

            // depends on the stock exchange
            //const double floorFee = Config.Instance.Get<double>( "Brokers.DiBa.FloorFee", 2.5d );

            double fee = price * quantity * courtage;
            fee = Math.Max( fee, minCourtage );
            fee = Math.Min( fee, maxCourtage );

            return fee;
        }
    }
}
