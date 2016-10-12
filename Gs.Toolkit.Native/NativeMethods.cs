using System;
using System.Runtime.InteropServices;

namespace Gs.Toolkit.Native
{
    public class NativeMethods
    {
        [DllImport("Kernel32", SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr handle, string funcname);

        [DllImport("Kernel32", SetLastError = true)]
        public static extern IntPtr LoadLibrary(string funcname);

        [DllImport("Kernel32", SetLastError = true)]
        public static extern int FreeLibrary(IntPtr handle);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hReservedNull, LoadLibraryFlags dwFlags);
    }
}