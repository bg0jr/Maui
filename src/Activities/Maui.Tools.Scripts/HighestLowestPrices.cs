using System;
using System.Linq;
using Maui.Dynamics;
using Maui.Dynamics.Data;
using Maui.Dynamics.Presets;
using Maui.Entities;
using Maui.Shell;
using Maui.Shell.Forms;
using System.ComponentModel.DataAnnotations;
using Blade.Shell;

namespace Maui.Tools.Scripts
{
    public class HighestLowestPrices : ScriptBase
    {
        [IsinArgument, Required]
        public string Isin
        {
            get;
            set;
        }
        
        protected override void Run()
        {
            DatumDefines.StockPrice.Create();

            var stock = StockHandle.GetOrCreate( Isin => this.Isin );

            DumpHighestLowestPricesPerYear( stock );
        }

        private void DumpHighestLowestPricesPerYear( StockHandle stock )
        {
            var series = MauiX.Query.GetStockPrices( stock, DateClause.All, Timeframes.DAY );

            var highLowByYear = series
                .GroupBy( price => price.Date.Year )
                .Select( group => new
                {
                    Year = group.Key,
                    Lowest = group.Min( p => p.Close ),
                    Highest = group.Max( p => p.Close )
                } );

            Console.WriteLine( "{0,5} {1,10} {2,10}", "year", "lowest", "highest" );
            foreach ( var highLowGroup in highLowByYear.OrderBy( g => g.Year ) )
            {
                Console.WriteLine( "{0,5} {1,10:#0.00} {2,10:#0.00}", highLowGroup.Year, highLowGroup.Lowest, highLowGroup.Highest );
            }
        }
    }
}
