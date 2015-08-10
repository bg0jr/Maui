using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Model
{
    public class Map<TKey, TElement> : IMap<TKey, TElement>, IDictionary<TKey, TElement>
    {
        private IDictionary<TKey, TElement> myMap;

        public Map( params KeyValuePair<TKey, TElement>[] parameters )
        {
            myMap = new Dictionary<TKey, TElement>();

            foreach ( var param in parameters )
            {
                Add( param );
            }
        }

        public Map( IMap<TKey, TElement> map )
            : this( map.ToArray() )
        {
        }

        public Map( IDictionary<TKey, TElement> map )
            : this( map.ToArray() )
        {
        }

        public void Add( TKey key, TElement value )
        {
            myMap.Add( GetKey( key ), value );
        }

        public bool ContainsKey( TKey key )
        {
            return myMap.ContainsKey( GetKey( key ) );
        }

        public ICollection<TKey> Keys
        {
            get { return myMap.Keys; }
        }

        public bool Remove( TKey key )
        {
            return myMap.Remove( GetKey( key ) );
        }

        public bool TryGetValue( TKey key, out TElement value )
        {
            return myMap.TryGetValue( GetKey( key ), out value );
        }

        public ICollection<TElement> Values
        {
            get { return myMap.Values; }
        }

        public TElement this[ TKey key ]
        {
            get
            {
                return myMap[ GetKey( key ) ];
            }
            set
            {
                myMap[ GetKey( key ) ] = value;
            }
        }

        public void Add( KeyValuePair<TKey, TElement> item )
        {
            myMap.Add( new KeyValuePair<TKey, TElement>( GetKey( item.Key ), item.Value ) );
        }

        public void Clear()
        {
            myMap.Clear();
        }

        public bool Contains( KeyValuePair<TKey, TElement> item )
        {
            return myMap.Contains( new KeyValuePair<TKey, TElement>( GetKey( item.Key ), item.Value ) );
        }

        public void CopyTo( KeyValuePair<TKey, TElement>[] array, int arrayIndex )
        {
            myMap.CopyTo( array, arrayIndex );
        }

        public int Count
        {
            get { return myMap.Count; }
        }

        public bool IsReadOnly
        {
            get { return myMap.IsReadOnly; }
        }

        public bool Remove( KeyValuePair<TKey, TElement> item )
        {
            return myMap.Remove( new KeyValuePair<TKey, TElement>( GetKey( item.Key ), item.Value ) );
        }

        public IEnumerator<KeyValuePair<TKey, TElement>> GetEnumerator()
        {
            return myMap.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return myMap.GetEnumerator();
        }

        public bool Contains( TKey key )
        {
            return myMap.ContainsKey( GetKey( key ) );
        }

        /// <summary>
        /// allows derived classes to apply internal transformations on a key 
        /// before used (e.g. transforming into lower case)
        /// </summary>
        protected virtual TKey GetKey( TKey key )
        {
            return key;
        }
    }
}
