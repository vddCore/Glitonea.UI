namespace Glitonea.UI.Platform;

using System.Runtime.InteropServices;

internal static partial class Win32
{
    public static class DwmApi
    {
        private const string LibraryName = "dwmapi";
        
        public enum DWMWINDOWATTRIBUTE {
            DWMWA_NCRENDERING_ENABLED,
            DWMWA_NCRENDERING_POLICY,
            DWMWA_TRANSITIONS_FORCEDISABLED,
            DWMWA_ALLOW_NCPAINT,
            DWMWA_CAPTION_BUTTON_BOUNDS,
            DWMWA_NONCLIENT_RTL_LAYOUT,
            DWMWA_FORCE_ICONIC_REPRESENTATION,
            DWMWA_FLIP3D_POLICY,
            DWMWA_EXTENDED_FRAME_BOUNDS,
            DWMWA_HAS_ICONIC_BITMAP,
            DWMWA_DISALLOW_PEEK,
            DWMWA_EXCLUDED_FROM_PEEK,
            DWMWA_CLOAK,
            DWMWA_CLOAKED,
            DWMWA_FREEZE_REPRESENTATION,
            DWMWA_PASSIVE_UPDATE_MODE,
            DWMWA_USE_HOSTBACKDROPBRUSH,
            DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
            DWMWA_WINDOW_CORNER_PREFERENCE = 33,
            DWMWA_BORDER_COLOR,
            DWMWA_CAPTION_COLOR,
            DWMWA_TEXT_COLOR,
            DWMWA_VISIBLE_FRAME_BORDER_THICKNESS,
            DWMWA_SYSTEMBACKDROP_TYPE,
            DWMWA_LAST
        };

        public const uint DWMWA_COLOR_NONE = 0xFFFFFFFE;
        public const uint DWMWA_COLOR_DEFAULT = 0xFFFFFFFF;
        
        [DllImport("dwmapi.dll", CharSet = CharSet.Unicode)]
        private static extern unsafe IntPtr DwmGetWindowAttribute(
            IntPtr hwnd,
            DWMWINDOWATTRIBUTE dwAttribute,
            void* pvAttribute,
            int cbAttribute
        );
        
        [DllImport("dwmapi.dll", CharSet = CharSet.Unicode)]
        private static extern unsafe IntPtr DwmSetWindowAttribute(
            IntPtr hwnd,
            DWMWINDOWATTRIBUTE dwAttribute,
            void* pvAttribute,
            int cbAttribute
        );
        
        [DllImport("dwmapi.dll", CharSet = CharSet.Unicode)]
        private static extern unsafe IntPtr DwmIsCompositionEnabled(
            bool *pfEnabled
        );

        public static bool DwmIsCompositionEnabled()
        {
            var outBool = false;
            
            unsafe
            {
                DwmIsCompositionEnabled(&outBool);
            }

            return outBool;
        }

        public static IntPtr DwmGetWindowAttribute(
            IntPtr hwnd,
            DWMWINDOWATTRIBUTE dwAttribute,
            out uint attribute
        )
        {
            IntPtr result = IntPtr.Zero;
            uint vAttribute = 0;
            
            unsafe
            {
                 result = DwmGetWindowAttribute(
                    hwnd, 
                    dwAttribute, 
                    &vAttribute,
                    Marshal.SizeOf<uint>()
                );
            }
            
            attribute = vAttribute;
            return result;
        }
        
        public static IntPtr DwmGetWindowAttribute(
            IntPtr hwnd,
            DWMWINDOWATTRIBUTE dwAttribute,
            out ulong attribute
        )
        {
            IntPtr result = IntPtr.Zero;
            ulong vAttribute = 0;
            
            unsafe
            {
                result = DwmGetWindowAttribute(
                    hwnd, 
                    dwAttribute, 
                    &vAttribute,
                    Marshal.SizeOf<ulong>()
                );
            }
            
            attribute = vAttribute;
            return result;
        }
        
        public static IntPtr DwmGetWindowAttribute(
            IntPtr hwnd,
            DWMWINDOWATTRIBUTE dwAttribute,
            out bool attribute
        )
        {
            IntPtr result = IntPtr.Zero;
            bool pvAttribute = false;
            
            unsafe
            {
                result = DwmGetWindowAttribute(
                    hwnd, 
                    dwAttribute, 
                    &pvAttribute,
                    Marshal.SizeOf<bool>()
                );
            }
            
            attribute = pvAttribute;
            return result;
        }
        
        public static IntPtr DwmSetWindowAttribute(
            IntPtr hwnd,
            DWMWINDOWATTRIBUTE dwAttribute,
            uint attribute
        )
        {
            IntPtr result = IntPtr.Zero;
            uint pvAttribute = attribute;
            
            unsafe
            {
                result = DwmSetWindowAttribute(
                    hwnd, 
                    dwAttribute, 
                    &pvAttribute,
                    Marshal.SizeOf<uint>()
                );
            }
            
            attribute = pvAttribute;
            return result;
        }
        
        public static IntPtr DwmSetWindowAttribute(
            IntPtr hwnd,
            DWMWINDOWATTRIBUTE dwAttribute,
            ulong attribute
        )
        {
            IntPtr result = IntPtr.Zero;
            ulong pvAttribute = attribute;
            
            unsafe
            {
                result = DwmSetWindowAttribute(
                    hwnd, 
                    dwAttribute, 
                    &pvAttribute,
                    Marshal.SizeOf<ulong>()
                );
            }
            
            attribute = pvAttribute;
            return result;
        }
        
        public static IntPtr DwmSetWindowAttribute(
            IntPtr hwnd,
            DWMWINDOWATTRIBUTE dwAttribute,
            bool attribute
        )
        {
            IntPtr result = IntPtr.Zero;
            bool pvAttribute = attribute;
            
            unsafe
            {
                result = DwmSetWindowAttribute(
                    hwnd, 
                    dwAttribute, 
                    &pvAttribute,
                    Marshal.SizeOf<bool>()
                );
            }
            
            attribute = pvAttribute;
            return result;
        }
    }
}