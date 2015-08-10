using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Maui.Tools.Studio.ViewModel
{
    public class SelfTrackingEntitySet<T> : ObservableCollection<T>
    {
        private List<T> myAddedItems;
        private List<T> myRemovedItems;

        public SelfTrackingEntitySet()
            : this( new T[] { } )
        {
        }

        public SelfTrackingEntitySet( IEnumerable<T> set )
            : base( set )
        {
            myAddedItems = new List<T>();
            myRemovedItems = new List<T>();
        }

        public IEnumerable<T> AddedItems
        {
            get { return myAddedItems; }
        }

        public IEnumerable<T> RemovedItems
        {
            get { return myRemovedItems; }
        }

        protected override void OnCollectionChanged( NotifyCollectionChangedEventArgs e )
        {
            if ( e.Action == NotifyCollectionChangedAction.Add )
            {
                myAddedItems.AddRange( e.NewItems.OfType<T>() );
            }

            if ( e.Action == NotifyCollectionChangedAction.Remove )
            {
                myRemovedItems.AddRange( e.OldItems.OfType<T>() );
            }

            base.OnCollectionChanged( e );
        }
    }
}
