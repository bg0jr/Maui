using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using Blade.Shell;

namespace Maui.Shell
{
    public class MauiScope : IDisposable
    {
        public MauiScope()
        {
            Engine.Boot( null );

            Engine.Run();

            // print some environment info
            Console.WriteLine( "Maui Version " + GetType().Assembly.GetName().Version.ToString() );

            var bannerInfos = Engine.Initializers
                .OfType<IBannerInfoProvider>()
                .Select( provider => provider.GetBannerInformation() )
                .OrderBy( info => info.Key )
                .ToList();

            foreach ( var info in bannerInfos )
            {
                Console.WriteLine( "  {0,-15} : {1}", info.Key, info.Value );
            }

            Console.WriteLine();
        }

        public void Dispose()
        {
            Engine.Shutdown();
        }

        public static Func<IDisposable> Creator
        {
            get
            {
                return () => new MauiScope();
            }
        }
    }
}
