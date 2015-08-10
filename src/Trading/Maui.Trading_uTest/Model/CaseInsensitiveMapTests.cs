using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Maui.Trading.Model;

namespace Maui.Trading.UnitTests.Model
{
    [TestFixture]
    public class CaseInsensitiveMapTests
    {
        [Test]
        public void SetValue_SameKeyDifferentCasing_Overwrites()
        {
            var map = new CaseInsensitiveMap<int>();
            map[ "a" ] = 1;

            map[ "A" ] = 2;

            Assert.That( map[ "a" ], Is.EqualTo( 2 ) );
        }

        [Test]
        public void Add_SameKeyDifferentCasing_Throws()
        {
            var map = new CaseInsensitiveMap<int>();
            map[ "a" ] = 1;

            Assert.Throws<ArgumentException>( () => map.Add( "A", 2 ) );
        }

        [Test]
        public void ContainsKey_SameKeyDifferentCasing_Found()
        {
            var map = new CaseInsensitiveMap<int>();
            map[ "a" ] = 1;

            Assert.That( map.ContainsKey( "A" ), Is.True );
        }

        [Test]
        public void Keys_KeyAddedInUpperCase_ReturnsLowercaseKey()
        {
            var map = new CaseInsensitiveMap<int>();
            map[ "A" ] = 1;

            Assert.That( map.Keys.Single(), Is.EqualTo( "a" ) );
        }

        [Test]
        public void Remove_SameKeyDifferentCasing_Removed()
        {
            var map = new CaseInsensitiveMap<int>();
            map[ "a" ] = 1;

            map.Remove( "A" );

            Assert.That( map, Is.Empty );
        }

        [Test]
        public void TryGetValue_SameKeyDifferentCasing_Found()
        {
            var map = new CaseInsensitiveMap<int>();
            map[ "a" ] = 1;

            int value;
            bool found = map.TryGetValue( "A", out value );

            Assert.That( found, Is.True );
            Assert.That( value, Is.EqualTo( 1 ) );
        }

        [Test]
        public void Add_KeyValuePairWithSameKeyDifferentCasing_Throws()
        {
            var map = new CaseInsensitiveMap<int>();
            map[ "a" ] = 1;

            Assert.Throws<ArgumentException>( () => map.Add( new KeyValuePair<string, int>( "A", 2 ) ) );
        }

        [Test]
        public void Contains_KeyValuePairWithSameKeyDifferentCasing_Found()
        {
            var map = new CaseInsensitiveMap<int>();
            map[ "a" ] = 1;

            Assert.That( map.Contains( new KeyValuePair<string, int>( "A", 1 ) ), Is.True );
        }

        [Test]
        public void Contains_SameKeyDifferentCasing_Found()
        {
            var map = new CaseInsensitiveMap<int>();
            map[ "a" ] = 1;

            Assert.That( map.Contains( "A" ), Is.True );
        }

        [Test]
        public void Remove_KeyValuePairWithSameKeyDifferentCasing_Removed()
        {
            var map = new CaseInsensitiveMap<int>();
            map[ "a" ] = 1;

            map.Remove( new KeyValuePair<string, int>( "A", 1 ) );

            Assert.That( map, Is.Empty );
        }
    }
}
