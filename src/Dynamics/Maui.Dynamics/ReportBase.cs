using System.Collections.Generic;
using System.Data;
using Maui.Dynamics.Data;

namespace Maui.Dynamics
{
    public class ReportBase
    {
        public Scope Scope
        {
            get
            {
                return Interpreter.Context.Scope;
            }
        }

        public string From
        {
            get
            {
                return Scope.From.ToShortDateString();
            }
        }

        public string To
        {
            get
            {
                return Scope.To.ToShortDateString();
            }
        }

        public IEnumerable<DataRow> Query( string table )
        {
            return Query( Interpreter.Context.TomScripting.GetManager( table ).Schema );
        }

        public IEnumerable<DataRow> Query( TableSchema schema )
        {
            if ( Interpreter.Context.Scope.TryFrom != null && Interpreter.Context.Scope.TryTo != null )
            {
                return schema.QueryByScope().Rows;
            }
            else
            {
                return schema.Manager().Query( Interpreter.Context.Scope.Stock.GetId( schema.OwnerIdColumn ) ).Rows;
            }
        }
    }
}
