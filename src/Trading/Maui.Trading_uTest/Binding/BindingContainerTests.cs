using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Maui.Trading.Binding;
using Maui.Trading.Binding.Xml;
using Maui.Trading.UnitTests.Fakes;

namespace Maui.Trading.UnitTests.Binding
{
    [TestFixture]
    public class BindingContainerTests
    {
        [Test]
        public void BindDifferentDataSources()
        {
            var container = CreateBindingContainer();

            var indicator = new SampleIndicator();
            container.Bind( indicator );

            Assert.That( indicator.SampleDouble.Name, Is.EqualTo( "SampleDouble" ) );
            Assert.That( indicator.SetOfDoubles.Name, Is.EqualTo( "SetOfDoubles" ) );
            Assert.That( indicator.AnotherSampleDouble.Name, Is.EqualTo( "D1" ) );
            Assert.That( indicator.AnotherSetOfDoubles.Name, Is.EqualTo( "D2" ) );
        }

        private static BindingContainer CreateBindingContainer()
        {
            var dataSourceFactory = new FakeDataSourceFactory();

            var container = new BindingContainer( dataSourceFactory );

            return container;
        }
    }
}
