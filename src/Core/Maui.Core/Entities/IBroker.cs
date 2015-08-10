using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Entities
{
    public interface IBroker
    {
        double CalculateCommission( int quantity, double price);
    }
}
