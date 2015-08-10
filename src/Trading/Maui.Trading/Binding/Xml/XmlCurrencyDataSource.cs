using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Entities;
using Blade;
using System.Xml.Linq;
using Blade.Binding;

namespace Maui.Trading.Binding.Xml
{
    public class XmlCurrencyDataSource : ICurrencyDataSource
    {
        private CurrencyTable myCurrencyTable;

        public XmlCurrencyDataSource( XmlDataStore dataStore )
        {
            DbC.ThrowIfArgumentNull( dataStore, "dataStore" );

            var ds = (SingleDataSource)dataStore.GetData( Name ).SingleOrDefault();
            myCurrencyTable = (CurrencyTable)ds.Value;
        }

        public string Name
        {
            get { return "CurrencyParity"; }
        }

        public double? GetParity( Currency source, Currency target )
        {
            return myCurrencyTable.FindMatchingParity( source, target );
        }
    }
}
