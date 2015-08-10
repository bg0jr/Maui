using System;
using System.Linq;
using Blade;
using Blade.Reflection;
using Maui.Data.Recognition;
using Maui.Dynamics;
using Maui.Dynamics.Data;
using Maui.Entities;
using Maui.Logging;
using Blade.Logging;

namespace Maui.Tasks.Dynamics
{
    public static class ImportFunction
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( ImportFunction ) );

        public static void Import( this IMslScript script, Datum datum )
        {
            Import( script, datum, false );
        }

        public static void Import( this IMslScript script, StockHandle stock, Datum datum )
        {
            Import( script, stock, datum, false );
        }

        public static void Import( this IMslScript script, StockHandle stock, Datum datum, bool force )
        {
            Import( script, stock, datum.Name, force );
        }

        public static void Import( this IMslScript script, StockHandle stock, string datum, bool force )
        {
            using ( var trans = new ObjectTransaction() )
            {
                trans.Override( () => Interpreter.Context.Scope.TryStock, stock );

                Import( script, datum, force );
            }
        }

        public static void Import( this IMslScript script, Datum datum, bool force )
        {
            Import( script, datum.Name, force );
        }

        public static void Import( this IMslScript script, string datum )
        {
            Import( script, datum, false );
        }

        public static void Import( this IMslScript script, string datum, bool force )
        {
            var provider = Interpreter.Context.DatumProviderFactory.Create( datum );
            if ( provider == null )
            {
                throw new Exception( "No data provider found for datum '" + datum + "'" );
            }

            if ( Interpreter.Context.Scope.TryStock != null )
            {
                // so this is called from within a model
                // e.g. a calc task or a function
                Fetch( provider, force );
            }
            else
            {
                Interpreter.Context.Scope.Catalog.ForEach( () => Fetch( provider, force ) );
            }
        }

        private static void Fetch( IDatumProvider provider, bool force )
        {
            if ( RequiredDataAvailable( provider.Datum, force ) )
            {
                myLogger.Info( "Seem to have all required data for: " + provider.Datum + " - skipping" );
                return;
            }

            myLogger.Info( string.Format( "Fetching datum '{0}' for stock = '{1}'", provider.Datum, Interpreter.Context.Scope.Stock.Stock.Isin ) );
            Engine.LogProgress( "  Fetching datum '{0}' for stock = '{1}'", provider.Datum, Interpreter.Context.Scope.Stock.ToString() );

            if ( provider == null )
            {
                throw new Exception( string.Format( "No data provider found for {0}", provider.Datum ) );
            }

            var result = provider.Fetch();

            if ( result == null )
            {
                myLogger.Error( "Unable to provide data for: {0}", provider.Datum );
            }
            else
            {
                Engine.LogProgress( "  Importing '{0}' for '{1}' from '{2}'", provider.Datum, Interpreter.Context.Scope.Stock.ToString(), result.Sites.First().Name );

                //result.Table.Dump();
                MauiX.Import.Import( Interpreter.Context.Scope.Stock, result );

                Engine.LogProgress( "  Import finished" );
            }
        }

        private static bool RequiredDataAvailable( string datum, bool force )
        {
            if ( force )
            {
                return false;
            }

            // is "from"/"to" set?
            if ( null == Interpreter.Context.Scope.TryFrom || null == Interpreter.Context.Scope.TryTo )
            {
                return false;
            }

            StockHandle stock = Interpreter.Context.Scope.Stock;
            DateTime from = Interpreter.Context.Scope.From;
            DateTime to = Interpreter.Context.Scope.To;

            var mgr = Interpreter.Context.TomScripting.GetManager( datum );

            long ownerId = stock.GetId( mgr.Schema.OwnerIdColumn );
            var result = mgr.Query( ownerId, new DateClause( from, to ), OriginClause.Default );
            if ( result.Rows.Count() == 0 )
            {
                return false;
            }

            // HACK: now we only check first and last
            var dates = result.Rows.Select( r => r.GetDate( result.Schema ) ).OrderBy( d => d );
            var first = dates.First();
            var last = dates.Last();

            if ( result.Schema.DateColumn.Equals( "year", StringComparison.OrdinalIgnoreCase ) )
            {
                return from.Year == first.Year && to.Year == last.Year;
            }
            else
            {
                return from.AlmostEquals( first, 3 ) && to.AlmostEquals( last, 3 );
            }
        }

        /*
   private void ImportFile()
        {
            string file = GetAbsolutePath( EvalAttribute( "file" ) );

            myLogger.Warn( "<import/> only contains a very simple implementation" );
            myLogger.Debug( "Importing file: " + file );

            if ( !File.Exists( file ) && !Directory.Exists( file ) )
            {
                throw new FileNotFoundException( "No such file", file );
            }

            // TODO: cleanup
            CsvFormat format = (CsvFormat)Context.Scope.LookupSymbol<IFormat>( Element.Attribute( "format" ).Value );
            Table table = Context.GetOrCreateTable( EvalAttribute( "into" ) );

            DataTable tmpTable = table.Schema.NewTempTable();
            CsvReader.Read( file, format, tmpTable );

            // XXX: hack to import the data
            var result = table.Manager.Query( 0 );
            foreach ( DataRow row in tmpTable.Rows )
            {
                var outRow = result.NewRow();
                row.ItemArray.ForeachIndex( ( v, i ) => outRow[ i ] = v );
                result.Add( outRow );
            }
        }
         */
    }
}
