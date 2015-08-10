using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Maui.Entities;
using System.Windows.Markup;

namespace Maui.Trading.Binding.Xml
{
    [ContentProperty( "Rows" )]
    public class CurrencyTable
    {
        public CurrencyTable()
        {
            Rows = new List<Parity>();
        }

        [Required]
        public List<Parity> Rows
        {
            get;
            private set;
        }

        public double? FindMatchingParity( Currency source, Currency target )
        {
            var entry = Rows.SingleOrDefault( e => e.IsMatch( source, target ) );
            if ( entry == null )
            {
                return null;
            }

            return entry.Value;
        }
    }
}
