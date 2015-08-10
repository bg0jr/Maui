using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Maui.Trading.Binding;

namespace Maui.Trading.UnitTests.Binding
{
    [TestFixture]
    public class DataSourceAttributeTests
    {
        [Test]
        public void GetConstructorArguments_Null_ReturnsEmptyParameters()
        {
            var attr = new DataSourceAttribute();

            var args = attr.GetConstructorArguments();

            Assert.That( args, Is.Not.Null );
            Assert.That( args.Any(), Is.False );
        }

        [Test]
        public void GetConstructorArguments_WhenCalled_NamedParametersAreCreated()
        {
            var attr = new DataSourceAttribute();
            attr.ConstructorArugments = new object[] { "a", 1, "b", "2" };

            var args = attr.GetConstructorArguments();

            Assert.That( args.GetParameter<int>( "a" ), Is.EqualTo( 1 ) );
            Assert.That( args[ "b" ], Is.EqualTo( "2" ) );
        }
    }
}
