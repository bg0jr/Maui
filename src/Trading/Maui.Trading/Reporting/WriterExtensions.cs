using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Maui.Trading.Model;

namespace Maui.Trading.Reporting
{
    public static class WriterExtensions
    {
        public static void Dump<TTime, TValue>( this TextWriter writer, TimedValue<TTime, TValue> value )
            where TTime : IComparable<TTime>, IEquatable<TTime>
        {
            writer.WriteLine( "{0}:{1}", value.Time, value.Value );
        }
    }
}
