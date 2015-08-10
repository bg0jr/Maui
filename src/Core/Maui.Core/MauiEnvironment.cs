using System.IO;
using Blade;

namespace Maui
{
    /// <summary>
    /// Utilities for the Maui environment.
    /// </summary>
    public sealed class MauiEnvironment
    {
        public static string Root { get; private set; }

        public static string Plugins { get; private set; }

        public static string Config { get; private set; }

        public static string Starter { get; private set; }

        static MauiEnvironment()
        {
            InsideTest = false;

            SetFileSystemStructure();
        }

        private static void SetFileSystemStructure()
        {
            Root = Path.GetFullPath( typeof( MauiEnvironment ).GetAssemblyPath() );
            Plugins = Root;
            Config = Path.Combine( Root, "config" );
            Starter = Path.Combine( Config, "starter" );
        }

        private MauiEnvironment()
        {
        }

        /// <summary>
        /// Used by IntegrationTests to signal that special actions
        /// should not happen during tests.
        /// (e.g. no default logging initialization).
        /// </summary>
        public static bool InsideTest { get; set; }
    }
}
