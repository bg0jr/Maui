using System;
using System.Collections.Generic;
using System.Linq;
using Blade;
using Blade.Collections;
using Maui.Dynamics;
using Maui.Dynamics.Data;
using Maui.Entities;
using Maui.Logging;
using Maui.Shell;
using Maui.Dynamics.Shell;
using Blade.Logging;

namespace Maui.Tools.Scripts
{
    public class TopFlop : MslScriptBase
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( TopFlop ) );

        private static string[] Catalogs = { "DAX.F", "MDAX.F", "SDAX.F", "TECDAX.F" };
        private static int Year = 2009;

        private class Result
        {
            public StockHandle Stock;
            public double YearYield;
            public double? DecemberYield;
        }

        protected override void Interpret()
        {
            var report = new List<Result>();

            Catalogs.Foreach( catalog =>
            {
                this.Scope().Catalog = FindCatalog( catalog );
                this.Scope().Catalog.ForEach( stock =>
                {
                    var result = GetYield( stock );
                    if ( result != null )
                    {
                        report.Add( result );
                    }
                } );
            } );

            report.OrderByDescending( entry => entry.YearYield )
                .Foreach( entry => Console.WriteLine( "{0} => {1} / {2}",
                    entry.Stock, entry.YearYield.ToH(),
                    entry.DecemberYield.HasValue ? entry.DecemberYield.Value.ToH() : "n.a." ) );
        }

        private static StockCatalog FindCatalog( string catalog )
        {
            using ( var tom = Engine.ServiceProvider.CreateEntityRepository() )
            {
                return tom.StockCatalogs.SingleOrDefault( c => c.Name == catalog );
            }
        }

        private Result GetYield( StockHandle stock )
        {
            var series = MauiX.Query.GetStockPrices( stock,
                new DateClause( DateTimeExtensions.FirstOfYear( Year ), DateTimeExtensions.LastOfYear( Year ) ),
                Timeframes.DAY );

            if ( series.Count <= 2 )
            {
                Console.WriteLine( "Not enough data for: " + stock );
                return null;
            }

            return new Result()
                {
                    Stock = stock,
                    YearYield = MauiX.Calc.Yield( series.First, series.Last ),
                    DecemberYield = CalculateDecemberYield( series ),
                };
        }

        private double? CalculateDecemberYield( StockPriceSeries series )
        {
            var dec1 = series.FindByNearestFollowingDate( new DateTime( Year, 12, 1 ) );
            var dec31 = series.FindByNearestPrecedingDate( new DateTime( Year, 12, 31 ) );

            if ( dec1 == null || dec31 == null ||
                !dec1.Date.AlmostEquals( new DateTime( Year, 12, 1 ), 3 ) ||
                !dec31.Date.AlmostEquals( new DateTime( Year, 12, 31 ), 3 ) )
            {
                //myLogger.Warn( "Not enough data! ignoring stock: " + stock );
                return null;
            }

            //Console.WriteLine( "{0} => {1} / {2}", stock, dec1, dec31 );

            return MauiX.Calc.Yield( dec1, dec31 );
        }
    }
}
