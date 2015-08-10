using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Entities;
using Maui.Dynamics.Types;
using Blade.Collections;

namespace Maui.Dynamics
{
    /// <summary>
    /// Context (Envelop for MSL scripts, models and reports)
    /// </summary>
    public class Scope
    {
        private Dictionary<string, object> myVariables = null;
        private DateTime? myFrom = null;
        private DateTime? myTo = null;
        private StockHandle myStock = null;
        private StockCatalog myCatalog = null;

        public Scope()
            : this( null )
        {
        }

        public Scope( Scope parent )
        {
            Parent = parent;

            myVariables = new Dictionary<string, object>();
        }

        public Scope Parent { get; private set; }

        /// <summary>
        /// Necessary for variables which need to be known by MSL interpreter.
        /// </summary>
        public object this[ string key ]
        {
            get
            {
                if ( myVariables.ContainsKey( key ) )
                {
                    return myVariables[ key ];
                }

                if ( Parent != null )
                {
                    return Parent[ key ];
                }

                throw new Exception( "No such variable: " + key );
            }
            set
            {
                myVariables[ key ] = value;
            }
        }

        /// <summary>
        /// Removes the given variable from scope.
        /// Does NOT work recursively.
        /// </summary>
        public void RemoveVariable( string name )
        {
            if ( myVariables.ContainsKey( name ) )
            {
                myVariables.Remove( name );
            }
        }

        public IEnumerable<string> Variables
        {
            get
            {
                return myVariables.Keys.Union( ( Parent != null ? Parent.Variables : EmptySet.OfStrings ) );
            }
        }

        #region Context-global variables

        public DateTime? TryFrom
        {
            get { return ( myFrom == null && Parent != null ? Parent.TryFrom : myFrom ); }
        }

        public DateTime? TryTo
        {
            get { return ( myTo == null && Parent != null ? Parent.TryTo : myTo ); }
        }

        public StockCatalog Catalog
        {
            get { return ( myCatalog == null && Parent != null ? Parent.Catalog : myCatalog ); }
            set { myCatalog = value; }
        }

        /// <summary>
        /// The current stock set in the current scope.
        /// <remarks>
        /// Usually this is set when looping a catalog.
        /// </remarks>
        /// </summary>
        public StockHandle TryStock
        {
            get { return ( myStock == null && Parent != null ? Parent.Stock : myStock ); }
            set { Stock = value; }
        }

        /// <summary>
        /// Returns the current stock and throws an exception if it
        /// does not exist.
        /// </summary>
        public StockHandle Stock
        {
            get
            {
                var stock = TryStock;
                if ( stock == null )
                {
                    throw new Exception( "No current stock exists" );
                }
                return stock;
            }
            set { myStock = value; }
        }

        /// <summary>
        /// Returns the current from date and throws an exception if it
        /// does not exist.
        /// </summary>
        public DateTime From
        {
            get
            {
                var from = TryFrom;
                if ( from == null )
                {
                    throw new Exception( "No 'from' exists" );
                }
                return from.Value;
            }
            set { myFrom = value; }
        }

        /// <summary>
        /// Returns the current from date and throws an exception if it
        /// does not exist.
        /// </summary>
        public DateTime To
        {
            get
            {
                var to = TryTo;
                if ( to == null )
                {
                    throw new Exception( "No 'To' exists" );
                }
                return to.Value;
            }
            set { myTo = value; }
        }

        #endregion
    }
}
