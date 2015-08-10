using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blade.IO;

namespace Maui.Trading.Utils
{
    public static class Shell
    {
        /// <summary>
        /// Returns a relative path from this directory back to the given root directory.
        /// </summary>
        public static  string GetPathBackToRoot( this IDirectory self, IDirectory root )
        {
            var numDirectoriesOfRoot = root.Path.Split( new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries ).Length;
            var numDirectoriesOfSelf = self.Path.Split( new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries ).Length;

            var backsteps = string.Empty;
            for ( int i = numDirectoriesOfRoot; i < numDirectoriesOfSelf; ++i )
            {
                backsteps = "../" + backsteps;
            }

            return backsteps;
        }
    }
}
