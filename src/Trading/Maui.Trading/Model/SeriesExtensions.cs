using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Indicators;

namespace Maui.Trading.Model
{
    public static class SeriesExtensions
    {
        public static ISignalSeries Derive( this ISignalSeries self, ObjectDescriptor type, Func<TimedValue<DateTime, Signal>, TimedValue<DateTime, Signal>> converter )
        {
            var convertedSignals = self.Select( s => converter( s ) );
            var identifier = self.Identifier.Derive( type );
            var series = SignalSeries.FromSortedSet( self.Reference, identifier, convertedSignals );

            return series;
        }
    }
}
