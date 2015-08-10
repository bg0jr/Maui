using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Entities;

namespace Maui.Trading.Binding
{
    public interface ICurrencyDataSource : IDataSource
    {
        /// <summary>
        /// Returns the factor which needs to be applied to a price from source currency
        /// to get it in target currency.
        /// </summary>
        double? GetParity( Currency source, Currency target );
    }
}
