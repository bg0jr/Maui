using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Transactions;
using Maui.Dynamics.Data;
using Maui.Dynamics;

namespace Maui.Tasks.Dynamics
{
    public static class SingleFunctions
    {
        public static T Single<T>( this  DataColumn col )
        {
            var inputTable = Interpreter.Context.TomScripting.GetManager( col.Table.TableName );
            var stock = Interpreter.Context.Scope.Stock;

            var data = inputTable.Query( stock.GetId( inputTable.Schema.OwnerIdColumn ) );

            var q = data.Rows.Select( row => row.Field<T>( col.ColumnName ) );

            if ( q.Count() == 0 )
            {
                throw new Exception( string.Format( "No data found in {0}.{1}", col.Table.TableName, col.ColumnName ) );
            }
            else if ( q.Count() == 1 )
            {
                return q.Single();
            }
            else
            {
                throw new Exception( string.Format( "Cannot turn a series of data into a scalar: {0}.{1}", col.Table.TableName, col.ColumnName ) );
            }
        }

        public static double ToF( this DataColumn column )
        {
            return column.Single<double>();
        }

        public static string ToS( this DataColumn column )
        {
            return column.Single<string>();
        }

        public static string ToH( this DataColumn column )
        {
            if ( column.DataType == typeof( double ) )
            {
                return string.Format( "{0:####0.00}", column.Single<double>() );
            }
            else
            {
                return column.Single<string>();
            }
        }

        public static void Single( this DataColumn col, object value )
        {
            var inputTable = Interpreter.Context.TomScripting.GetManager( col.Table.TableName );
            var stock = Interpreter.Context.Scope.Stock;

            long ownerId = stock.GetId( inputTable.Schema.OwnerIdColumn );
            using ( TransactionScope trans = new TransactionScope() )
            {
                object obj = value;
                if ( col.DataType == typeof( double ) )
                {
                    obj = Convert.ToDouble( value, CultureInfo.InvariantCulture );
                }

                ScopedTable result = inputTable.Query( ownerId );

                DataRow row = result.NewRow();
                row[ inputTable.Schema.OwnerIdColumn ] = ownerId;
                row[ col.ColumnName ] = obj;

                result.AddOrUpdate( row, col.ColumnName );

                trans.Complete();
            }
        }

        public static double LastYear( this DataColumn col )
        {
            var inputTable = Interpreter.Context.TomScripting.GetManager( col.Table.TableName );

            var stock = Interpreter.Context.Scope.Stock;

            var data = inputTable.Query( stock.GetId( inputTable.Schema.OwnerIdColumn ) );

            var q = from row in data.Rows
                    orderby row.GetDateString( inputTable.Schema ) descending
                    select row.Field<double>( col.ColumnName );

            return q.First();
        }
    }
}
