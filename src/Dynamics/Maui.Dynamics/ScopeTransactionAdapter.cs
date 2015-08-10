using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blade.Collections;
using Blade.Reflection;

namespace Maui.Dynamics
{
    /// <summary>
    /// Enables Msl.Scope to be used in ObjectTransactions
    /// </summary>
    public class ScopeTransactionAdapter : IRollbackable
    {
        private Scope myScope = null;
        private IEnumerable<string> myVariables = null;
        private List<Action> myRollbackActions = null;

        /// <summary/>
        public ScopeTransactionAdapter( Scope scope )
            : this( scope, scope.Variables )
        {
            // we create a transaction for the complete scope
            // -> setup a rollback action which removes all variables which were not
            //    known before the transaction started
            myRollbackActions.Add( () =>
                {
                    var newVariables = myScope.Variables.Except( myVariables ).ToList();
                    newVariables.Foreach( var => myScope.RemoveVariable( var ) );
                } );
        }

        /// <summary/>
        public ScopeTransactionAdapter( Scope scope, params string[] variables )
            : this( scope, (IEnumerable<string>)variables )
        {
        }

        /// <summary/>
        public ScopeTransactionAdapter( Scope scope, IEnumerable<string> variables )
        {
            myScope = scope;
            myVariables = variables.ToList();
            myRollbackActions = new List<Action>();
        }

        /// <summary/>
        public void CreateCheckpoint()
        {
            myVariables.Foreach( CreateCheckpointForVariable );
        }

        private void CreateCheckpointForVariable( string variable )
        {
            if ( myScope.Variables.Contains( variable ) )
            {
                var oldValue = myScope[ variable ];
                myRollbackActions.Add( () => myScope[ variable ] = oldValue );
            }
            else
            {
                // the var did not exist before so remove on rollback
                myRollbackActions.Add( () => myScope.RemoveVariable( variable ) );
            }
        }

        /// <summary/>
        public void Rollback()
        {
            myRollbackActions.Foreach( action => action() );
        }
    }
}
