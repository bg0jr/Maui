using System;
using System.Collections.Generic;
using System.Linq;
using Maui.Data.Recognition;
using Maui.Entities;

namespace Maui.Dynamics
{
    /// <summary>
    /// Enhances the <see cref="EvaluatorPolicy"/> in that way that all 
    /// keys will first be forwarded to the <see cref="Scope"/>.
    /// </summary>
    public class ScopeLookupPolicy : EvaluatorPolicy
    {
        public ScopeLookupPolicy()
            : this( new Dictionary<string, string>() )
        {
        }

        public ScopeLookupPolicy( Dictionary<string, string> lut )
            : base( lut )
        {
        }

        public ScopeLookupPolicy( Func<string, string> lookup )
            : base( lookup )
        {
        }

        protected override string GetValue( string key )
        {
            string value = EvaluateValue( key );
            return ( value != null ? value : base.GetValue( key ) );
        }

        internal static string EvaluateValue( string var )
        {
            if ( Interpreter.Context.Scope.Variables.Contains( var ) )
            {
                return Interpreter.Context.Scope[ var ].ToString();
            }

            if ( var == "from" )
            {
                return ( Interpreter.Context.Scope.TryFrom == null ? string.Empty : Interpreter.Context.Scope.TryFrom.Value.ToString() );
            }

            if ( var == "to" )
            {
                return ( Interpreter.Context.Scope.TryTo == null ? string.Empty : Interpreter.Context.Scope.TryTo.Value.ToString() );
            }

            return EvaluateStock( var, Interpreter.Context.Scope.Stock );
        }

        // !! duplicate code in Maui.Tasks.StockLookupPolicy
        private static string EvaluateStock( string var, StockHandle stock )
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
