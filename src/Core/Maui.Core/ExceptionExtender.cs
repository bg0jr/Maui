using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Blade;
using System.Reflection;

namespace Maui
{
    public static class ExceptionExtender
    {
        public static string Dump( this Exception exception )
        {
            using ( var writer = new StringWriter() )
            {
                DumpTo( exception, writer, 0 );
                return writer.ToString();
            }
        }

        public static void DumpTo( this Exception exception, TextWriter writer )
        {
            DumpTo( exception, writer, 0 );
        }

        private static void DumpTo( this Exception exception, TextWriter writer, int level )
        {
            if ( exception == null )
            {
                return;
            }

            string padding = " ".PadRight( level );
            writer.WriteLine( padding + exception.GetType() + ": " + exception.Message );
            writer.WriteLine( padding + "Context: " );
            foreach ( var key in exception.Data.Keys )
            {
                writer.WriteLine( string.Format( "{0}  {1}: {2}",
                    padding, key, exception.Data[ key ] ) );
            }
            writer.WriteLine( padding + "StackTrace: " + exception.StackTrace );

            if ( exception.InnerException != null )
            {
                writer.WriteLine( padding + "Inner exception was: " );

                DumpTo( exception.InnerException, writer, level + 2 );
            }
            if ( exception is ReflectionTypeLoadException )
            {
                var typeLoadEx = (ReflectionTypeLoadException)exception;

                foreach ( var loaderEx in typeLoadEx.LoaderExceptions )
                {
                    writer.WriteLine( padding + "Loader exception was: " );
                    DumpTo( loaderEx, writer, level + 2 );
                }
            }
        }

        /// <summary>
        /// Preserves the full stack trace before rethrowing an exception.
        /// <remarks>
        /// According to this post see http://weblogs.asp.net/fmarguerie/archive/2008/01/02/rethrowing-exceptions-and-preserving-the-full-call-stack-trace.aspx
        /// it is required to get the full stack trace in any case.
        /// </remarks>
        /// </summary>
        public static void PreserveStackTrace( this Exception exception )
        {
            MethodInfo preserveStackTrace = typeof( Exception ).GetMethod( "InternalPreserveStackTrace",
              BindingFlags.Instance | BindingFlags.NonPublic );
            preserveStackTrace.Invoke( exception, null );
        }
    }
}
