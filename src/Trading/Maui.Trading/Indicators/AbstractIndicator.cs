using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Reporting;
using Maui.Trading.Model;

namespace Maui.Trading.Indicators
{
    public abstract class AbstractIndicator : IIndicator
    {
        protected AbstractIndicator()
        {
            Name = GetType().Name;
        }

        protected AbstractIndicator( string name )
        {
            Name = name;
        }

        public string Name
        {
            get;
            private set;
        }

        public abstract IndicatorResult Calculate( IIndicatorContext context );
    }
}
