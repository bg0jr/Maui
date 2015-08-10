using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Maui.Trading.Binding.Decorators;
using Maui.Trading.Binding.Xml;
using Maui.Trading.Binding;
using Maui.Trading.Binding.Tom;
using Maui.Trading.Model;

namespace Maui.Trading.UnitTests.Binding.Decorators
{
    [TestFixture]
    public class StackDataSourceFactoryTests
    {
        private IDataSourceFactory myPrimaryDsFactory;
        private IDataSourceFactory mySecondaryDsFactory;

        [SetUp]
        public void SetUp()
        {
            var xmlDataStore = new XmlDataStore();
            myPrimaryDsFactory = new XmlDataSourceFactory( xmlDataStore );

            mySecondaryDsFactory = new TomDataSourceFactory();
        }

        [TearDown]
        public void TearDown()
        {
            myPrimaryDsFactory = null;
            mySecondaryDsFactory = null;
        }

        [Test]
        public void Create_SingleDataSource_ReturnsStackDataSourceImplementation()
        {
            var factory = new StackDataSourceFactory( myPrimaryDsFactory, mySecondaryDsFactory );
            var dataSource = factory.Create( "Prices", typeof( ISingleDataSource<SimplePrice> ), NamedParameters.Null );

            Assert.That( dataSource, Is.InstanceOf<StackSingleDataSource<SimplePrice>>() );
        }

        [Test]
        public void Create_EnumerableDataSource_ReturnsStackDataSourceImplementation()
        {
            var factory = new StackDataSourceFactory( myPrimaryDsFactory, mySecondaryDsFactory );
            var dataSource = factory.Create( "Prices", typeof( IEnumerableDataSource<SimplePrice> ), NamedParameters.Null );

            Assert.That( dataSource, Is.InstanceOf<StackEnumerableDataSource<SimplePrice>>() );
        }

        [Test]
        public void Create_WhenCalled_InnerDataSourcesArePassed()
        {
            var factory = new StackDataSourceFactory( myPrimaryDsFactory, mySecondaryDsFactory );
            var dataSource = factory.Create( "Prices", typeof( IEnumerableDataSource<SimplePrice> ), NamedParameters.Null );

            var typedDS = (StackEnumerableDataSource<SimplePrice>)dataSource;

            Assert.That( typedDS.DataSources.Count(), Is.EqualTo( 2 ) );
        }
    }
}
