using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Indicators;
using Blade;
using Maui.Trading.Modules.Calculators;
using Maui.Trading.Model;
using Maui.Trading.Utils;

namespace Maui.Trading.Modules
{
    public class DefaultCalculatorFactory : ICalculatorFactory
    {
        public bool CanCreate( string name )
        {
            return name.EqualsI( "sma" );
        }

        public ISeriesCalculator Create( string name, params object[] ctorArgs )
        {
            var calculator = CreateSeriesCalculator( name, ctorArgs );

            if ( PerfMon.Enabled )
            {
                calculator = new PerfMonSeriesCalculatorDecorator( calculator );
            }

            return calculator;
        }

        private ISeriesCalculator CreateSeriesCalculator( string name, params object[] ctorArgs )
        {
            if ( name.EqualsI( "sma" ) )
            {
                return CreateCalculator<SimpleMovingAverageCalculator>( ctorArgs );
            }

            throw new ArgumentException( "Cannot create calculator for name: " + name );
        }

        private T CreateCalculator<T>( params object[] ctorArgs )
        {
            return (T)Activator.CreateInstance( typeof( T ), ctorArgs );
        }
    }
}
