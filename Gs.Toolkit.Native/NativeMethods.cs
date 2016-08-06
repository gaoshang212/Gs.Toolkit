using System;
using System.Runtime.InteropServices;

namespace Gs.Toolkit.Native
{
    public class NativeMethods
    {
        [DllImport("Kernel32")]
        public static extern IntPtr GetProcAddress(IntPtr handle, string funcname);

        [DllImport("Kernel32")]
        public static extern IntPtr LoadLibrary(string funcname);

        [DllImport("Kernel32")]
        public static extern int FreeLibrary(IntPtr handle);
    }
}