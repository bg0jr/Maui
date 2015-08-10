using System.Data;
using Maui.Dynamics.Data;

namespace Maui.Dynamics
{
    public static class TableSchemaExtensions
    {
        public static TableSchema Create( this TableSchema schema )
        {
            schema.Manager().CreateTable();
            return schema;
        }

        public static ITableManager Manager( this TableSchema schema )
        {
            return Interpreter.Context.TomScripting.GetManager( schema );
        }

        public static TableSchema RewriteOwnerId( this TableSchema schema, string source )
        {
            var table = Interpreter.Context.TomScripting.GetManager( source );
            schema[ schema.OwnerIdColumn ].ColumnName = table.Schema.OwnerIdColumn;

            return schema;
        }

        public static TableSchema RewriteOwnerId( this TableSchema schema, DataColumn source )
        {
            schema.RewriteOwnerId( source.Table.TableName );

            return schema;
        }

        /// <summary>
        /// Scope: current stock, from, to and origin
        /// </summary>
        public static ScopedTable QueryByScope( this TableSchema schema )
        {
            return schema.Manager().Query( 
                Interpreter.Context.Scope.Stock.GetId( schema.OwnerIdColumn ), 
                new DateClause( 
                    Interpreter.Context.Scope.From, 
                    Interpreter.Context.Scope.To ),
                OriginClause.Default );
        }
    }
}
