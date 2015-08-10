using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Collections;
using System.Linq.Expressions;

namespace Maui.UnitTests.Entities
{
    public class InMemoryObjectSet<TEntity> : IObjectSet<TEntity> where TEntity : class
    {
        private HashSet<TEntity> myData;
        private IQueryable myQuery;

        public InMemoryObjectSet()
        {
            myData = new HashSet<TEntity>();
            myQuery = myData.AsQueryable();
        }

        public InMemoryObjectSet( IEnumerable<TEntity> data )
        {
            myData = new HashSet<TEntity>( data );
            myQuery = myData.AsQueryable();
        }

        public void AddObject( TEntity item )
        {
            CheckNotNull( item );
            myData.Add( item );
        }

        private static void CheckNotNull( TEntity item )
        {
            if ( item == null )
            {
                throw new ArgumentNullException( "entity" );
            }
        }

        public void DeleteObject( TEntity item )
        {
            CheckNotNull( item );
            myData.Remove( item );
        }

        public void Attach( TEntity item )
        {
            CheckNotNull( item );
            myData.Add( item );
        }

        public void Detach( TEntity item )
        {
            CheckNotNull( item );
            this.myData.Remove( item );
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return myData.GetEnumerator();
        }

        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        {
            return myData.GetEnumerator();
        }

        Type IQueryable.ElementType
        {
            get { return myQuery.ElementType; }
        }

        Expression IQueryable.Expression
        {
            get { return myQuery.Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return myQuery.Provider; }
        }
    }

}
