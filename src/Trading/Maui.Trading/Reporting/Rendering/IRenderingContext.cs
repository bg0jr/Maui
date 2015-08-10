using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Maui.Trading.Reporting.Charting;
using Blade.IO;

namespace Maui.Trading.Reporting.Rendering
{
    public interface IRenderingContext : IDisposable
    {
        /// <summary>
        /// Always points to the root/base of the rendering output
        /// </summary>
        string DocumentRoot
        {
            get;
            set;
        }

        /// <summary>
        /// Absolute URL to the currect document to be written.
        /// </summary>
        string DocumentUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Directory of the current DocumentUrl
        /// </summary>
        string DocumentUrlRoot
        {
            get;
        }

        TextWriter Document
        {
            get;
            set;
        }

        void Render( object reportNode );

        IRenderingContext CreateChildContext();
    }

    public static class IRenderingContextExtensions
    {
        public static IRenderingContext CreateUniqChildContext( this IRenderingContext self )
        {
            return self.CreateChildContext( Guid.NewGuid().ToString() + ".html" );
        }

        public static IRenderingContext CreateChildContext( this IRenderingContext self, params string[] relativeDocumentPath )
        {
            var childCtx = self.CreateChildContext();
            var directory = self.DocumentUrlRoot != null ? self.DocumentUrlRoot : self.DocumentRoot;
            childCtx.DocumentUrl = OS.CombinePaths( new[] { directory }, relativeDocumentPath );

            if ( !Directory.Exists( childCtx.DocumentUrlRoot ) )
            {
                Directory.CreateDirectory( childCtx.DocumentUrlRoot );
            }

            childCtx.Document = new StreamWriter( childCtx.DocumentUrl );

            return childCtx;
        }

        /// <summary>
        /// Returns DocumentUrl relative the "outer" context.
        /// </summary>
        public static string RelativeDocumentUrl( this IRenderingContext self, IRenderingContext outerContext )
        {
            var relativePath = self.DocumentUrl.Substring( outerContext.DocumentUrlRoot.Length );

            while ( relativePath.StartsWith( "/" ) || relativePath.StartsWith( "\\" ) )
            {
                relativePath = relativePath.Substring( 1 );
            }

            return relativePath;
        }
    }
}
