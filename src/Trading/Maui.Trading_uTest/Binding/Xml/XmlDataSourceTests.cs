using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Maui.Trading.Binding;
using Maui.Trading.Binding.Xml;
using Maui.Trading.Model;
using Maui.Trading.UnitTests.Fakes;
using NUnit.Framework;
using Blade.Testing;

namespace Maui.Trading.UnitTests.Binding.Xml
{
    [TestFixture]
    public class XmlDataSourceTests
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
            yield return XElement.Load( this.TestEnvironment().GetTestResource( "SanDisk.xaml" ) );
        }
        
        [Test]
        public void GetPrices()
        {
            IEnumerableDataSource<SimplePrice> dataSource = new XmlDataSource<SimplePrice>( "Prices", myDataStore );

            var stock = TomEntityBuilder.CreateStockHandle( "SanDisk", "US80004C1018", "Dollar" );
            var prices = new TimedValueSet<DateTime, double>( dataSource.ForStock( stock ) );

            Assert.That( prices[ new DateTime( 2010, 12, 13 ) ].Value, Is.EqualTo( 4.63 ) );
            Assert.That( prices[ new DateTime( 2010, 12, 12 ) ].Value, Is.EqualTo( 4.53 ) );
            Assert.That( prices[ new DateTime( 2010, 12, 11 ) ].Value, Is.EqualTo( 4.43 ) );
            Assert.That( prices[ new DateTime( 2010, 12, 10 ) ].Value, Is.EqualTo( 4.33 ) );
        }

        [Test]
        public void GetSingleValue_MultipleCurrenciesAvailable()
        {
            ISingleDataSource<int> dataSource = new XmlDataSource<int>( "SingleValue", myDataStore );

            var stock = TomEntityBuilder.CreateStockHandle( "SanDisk", "US80004C1018", "Dollar" );
            var value = dataSource.ForStock( stock );

            Assert.That( value, Is.EqualTo( 456 ) );
        }
    }
}
