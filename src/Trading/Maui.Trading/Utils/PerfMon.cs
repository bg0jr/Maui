using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace Maui.Trading.Utils
{
    public class PerfMon
    {
        private static Dictionary<string, TimeSpan> myStatistics = new Dictionary<string, TimeSpan>();

        public static bool Enabled
        {
            [MethodImpl( MethodImplOptions.Synchronized )]
            get;
            [MethodImpl( MethodImplOptions.Synchronized )]
            set;
        }

        [MethodImpl( MethodImplOptions.Synchronized )]
        public static void Log( string message, params object[] args )
        {
            if ( !Enabled )
            {
                return;
            }

            Console.WriteLine( message, args );
        }

        [MethodImpl( MethodImplOptions.Synchronized )]
        public static IDisposable Profile( string message, params object[] args )
        {
            return new Profiler( string.Format( message, args ) );
        }

        [MethodImpl( MethodImplOptions.Synchronized )]
        private static void OnFinished( string message, TimeSpan duration )
        {
            if ( !myStatistics.ContainsKey( message ) )
            {
                myStatistics[ message ] = new TimeSpan( 0 );
            }

            myStatistics[ message ] += duration;
        }


        private class Profiler : IDisposable
        {
            private DateTime myStart;

            public Profiler( string message )
            {
                Message = message;
                myStart = DateTime.Now;

                PerfMon.Log( "{0}|Start|{1}", myStart.TimeOfDay, Message );
            }

            public string Message
            {
                get;
                private set;
            }

            public void Dispose()
            {
                var stop = DateTime.Now;
                PerfMon.Log( "{0}|Stop |{1}|{2}", stop.TimeOfDay, Message, stop - myStart );
                OnFinished( Message, stop - myStart );
            }
        }

        public static void DumpStatistics()
        {
            foreach ( var entry in myStatistics )
            {
                Log( "{0,20}: {1}", entry.Key, entry.Value );
            }
        }
    }
}
