using System;
using System.Collections.Generic;
using System.Linq;
using Maui.Data.Recognition;
using Maui.Entities;

namespace Maui.Tasks
{
    public class StockLookupPolicy : EvaluatorPolicy
    {
        private StockHandle myStock;

        public StockLookupPolicy( StockHandle stock )
            : this( stock, new Dictionary<string, string>() )
        {
        }

        public StockLookupPolicy( StockHandle stock, Dictionary<string, string> lut )
            : base( lut )
        {
            myStock = stock;
        }

        protected override string GetValue( string key )
        {
            string value = EvaluateValue( key, myStock );
            return ( value != null ? value : base.GetValue( key ) );
        }

        // !! duplicate code in Maui.Scripting.ScopeLookupPolicy
        private static string EvaluateValue( string var, StockHandle stock )
        {
            if ( var.StartsWith( "stock." ) )
            {
                return stock[ var.Substring( "stock.".Length ) ].ToString();
            }

            // further special vars
            if ( var == "today.de" )
            {
                return string.Format( "{0}.{1}.{2}", DateTime.Today.Day, DateTime.Today.Month, DateTime.Today.Year );
            }

            // TODO: that should be located near provider or navigation or whatever
            if ( var == "ariva.stockexchange.id" )
            {
                // Xetra: boerse_id = 6 
                // Frankfurt: boerse_id = 1 
                // Nasdaq: boerse_id = 40 
                // NYSE: boerse_id = 21 
                var stockExchange = stock.StockExchange;
                if ( stockExchange.Symbol == "DE" )
                {
                    return "6";
                }
                else if ( stockExchange.Symbol == "F" )
                {
                    return "1";
                }
                else if ( stockExchange.Symbol == "NASDAQ" )
                {
                    return "40";
                }
                else if ( stockExchange.Symbol == "NYSE" )
                {
                    return "21";
                }
            }

            return null;
        }
    }
}
