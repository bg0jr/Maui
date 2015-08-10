using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Media3D;
using System.Windows.Media;

namespace Maui.Tools.Studio
{
    public static class VisualExtensions
    {
        public static T GetItemAtLocation<T>( this Visual self, Point location ) where T : FrameworkElement
        {
            var hitTestResults = VisualTreeHelper.HitTest( self, location );

            var parent = hitTestResults.VisualHit;
            while ( parent != null )
            {
                if ( parent is T )
                {
                    return (T)parent;
                }

                parent = VisualTreeHelper.GetParent( parent );
            }

            return null;
        }

        public static T GetDataContextAtLocation<T>( this Visual self, Point location )
        {
            var item = GetItemAtLocation<FrameworkElement>( self, location );

            return (T)item.DataContext;
        }
    }
}
