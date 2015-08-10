using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Security;

namespace Maui.Tools.Studio
{
    public static class MouseUtilities
    {
        [SecuritySafeCritical]
        public static Point GetMousePosition( Visual relativeTo )
        {
            Win32Point w32Mouse = new Win32Point();
            GetCursorPos( ref w32Mouse );
            return relativeTo.PointFromScreen( new Point( w32Mouse.X, w32Mouse.Y ) );
        }

        [StructLayout( LayoutKind.Sequential )]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };

        [DllImport( "user32.dll" )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool GetCursorPos( ref Win32Point pt );
    }
}
