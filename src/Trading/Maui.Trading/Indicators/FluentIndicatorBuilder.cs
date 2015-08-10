using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Indicators
{
    public static class FluentIndicatorBuilder
    {
        public static CombinedIndicator Group()
        {
            return new CombinedIndicator();
        }

        public static CombinedIndicator Group( ICombinedSignalCreator combinedSignalCreator )
        {
            return new CombinedIndicator( combinedSignalCreator );
        }

        public static CombinedIndicator Group( this CombinedIndicator combinedIndicator )
        {
            var newGroup = new CombinedIndicator();
            combinedIndicator.AddIndicator( newGroup );
            return newGroup;
        }

        public static CombinedIndicator Group( this CombinedIndicator combinedIndicator, ICombinedSignalCreator combinedSignalCreator )
        {
            var newGroup = new CombinedIndicator( combinedSignalCreator );
            combinedIndicator.AddIndicator( newGroup );
            return newGroup;
        }

        public static CombinedIndicator Add( this IIndicator lhs, IIndicator rhs )
        {
            return new CombinedIndicator()
                .Add( lhs )
                .Add( rhs );
        }

        public static CombinedIndicator Add( this CombinedIndicator combinedIndicator, IIndicator indicator )
        {
            combinedIndicator.AddIndicator( indicator );
            return combinedIndicator;
        }

        public static WeightedIndicator Weight( this IIndicator indicator, double factor )
        {
            return new WeightedIndicator( indicator, factor );
        }
    }
}
