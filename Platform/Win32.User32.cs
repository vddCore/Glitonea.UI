namespace Glitonea.UI.Platform;

using System.Runtime.InteropServices;

internal static partial class Win32
{
    public static class User32
    {
        private const string LibraryName = "user32";

        public enum GWL_INDEX : int
        {
            GWL_EXSTYLE = -20,
            GWL_HINSTANCE = -6,
            GWL_HWNDPARENT = -8,
            GWL_ID = -12,
            GWL_STYLE = -16,
            GWL_USERDATA = -21,
            GWL_WNDPROC = -4
        }

        public enum GWL_EXSTYLE
        {
            WS_EX_ACCEPTFILES = 0x00000010,
            WS_EX_APPWINDOW = 0x00040000,
            WS_EX_CLIENTEDGE = 0x00000200,
            WS_EX_COMPOSITED = 0x02000000,
            WS_EX_CONTEXTHELP = 0x00000400,
            WS_EX_CONTROLPARENT  = 0x00010000,
            WS_EX_DLGMODALFRAME = 0x00000001,
            WS_EX_LAYERED = 0x00080000,
            WS_EX_LAYOUTRTL = 0x00400000,
            WS_EX_LEFT = 0x00000000,
            WS_EX_LEFTSCROLLBAR = 0x00004000,
            WS_EX_MDICHILD = 0x00000040,
            WS_EX_NOACTIVATE = 0x08000000,
            WS_EX_NOINHERITLAYOUT = 0x00100000,
            WS_EX_NOREDIRECTIONBITMAP = 0x00200000,
            WS_EX_NOPARENTNOTIFY = 0x00000004,
            WS_EX_RTLREADING =  0x00002000,
            WS_EX_STATICEDGE = 0x00020000,
            WS_EX_TOOLWINDOW = 0x00000080,
            WS_EX_TOPMOST =  0x00000008,
            WS_EX_TRANSPARENT = 0x00000020,
            WS_EX_WINDOWEDGE  = 0x00000100,
        }
        
        [DllImport(LibraryName, CharSet = CharSet.Unicode)]
        public static extern IntPtr SetWindowLong(
            IntPtr hwnd,
            GWL_INDEX  nIndex,
            IntPtr dwNewLong
        );
        
        [DllImport(LibraryName, CharSet = CharSet.Unicode)]
        public static extern IntPtr GetWindowLong(
            IntPtr hwnd,
            GWL_INDEX nIndex
        );
    }
}