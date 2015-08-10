using System.Linq;
using Blade.Reflection;
using NUnit.Framework;

namespace Maui.Dynamics.UnitTest
{
    [TestFixture]
    public class ScopeTransactionAdapterTest : TestBase
    {
        [Test]
        public void ExistingVariable()
        {
            var scope = new Scope();
            scope[ "t1" ] = 23;

            using ( var trans = new ObjectTransaction() )
            {
                trans.Register( new ScopeTransactionAdapter( scope, "t1" ) );

                scope[ "t1" ] = 42;

                Assert.AreEqual( 42, scope[ "t1" ] );
            }

            Assert.AreEqual( 23, scope[ "t1" ] );
        }

        [Test]
        public void NonExistingVariable()
        {
            var scope = new Scope();

            using ( var trans = new ObjectTransaction() )
            {
                trans.Register( new ScopeTransactionAdapter( scope, "t1" ) );

                scope[ "t1" ] = 42;

                Assert.AreEqual( 42, scope[ "t1" ] );
            }

            Assert.IsFalse( scope.Variables.Contains( "t1" ) );
        }

        [Test]
        public void CompleteScope()
        {
            var scope = new Scope();
            scope[ "t1" ] = 21;

            using ( var trans = new ObjectTransaction() )
            {
                trans.Register( new ScopeTransactionAdapter( scope ) );

                scope[ "t1" ] = 42; // overwrite value
                scope[ "t2" ] = 42; // new variable

                Assert.AreEqual( 42, scope[ "t1" ] );
                Assert.AreEqual( 42, scope[ "t2" ] );
            }

            Assert.AreEqual( 21, scope[ "t1" ] );
            Assert.IsFalse( scope.Variables.Contains( "t2" ) );
        }
    }
}
