using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Maui.Trading.Binding.Xml;
using Maui.Trading.Model;
using System.Xml.Linq;
using Maui.Trading.Binding;
using Maui.Trading.UnitTests.Fakes;
using Maui.Entities;
using Blade.Testing;

namespace Maui.Trading.UnitTests.Binding.Xml
{
    [TestFixture]
    public class XmlCurrencyDataSourceTests
    {
        private XmlDataStore myDataStore;

        [SetUp]
        public void SetUp()
        {
            myDataStore = new XmlDataStore();

            foreach ( var ds in GetEmbeddedDataSources() )
            {
                myDataStore.Add( ds );
            }
        }

        private IEnumerable<XElement> GetEmbeddedDataSources()
        {
            yield return XElement.Load( this.TestEnvironment().GetTestResource( "CurrencyTable.xaml" ) );
        }
        
        [Test]
        public void GetParity()
        {
            ICurrencyDataSource dataSource = new XmlCurrencyDataSource( myDataStore );

            var euro = new Currency( "Euro", "€" );
            var dollar = new Currency( "Dollar", "$" );

            var parity = dataSource.GetParity( dollar, euro );

            int priceInDollar = 20;
            Assert.AreEqual( priceInDollar * parity.Value, 10, 0.0000001d );
        }
    }
}
