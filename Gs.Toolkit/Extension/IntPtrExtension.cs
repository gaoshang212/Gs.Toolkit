using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Gs.Toolkit.Extension
{
    public static class IntPtrExtension
    {
        public static IntPtr ToIntPtr(this byte[] p_bytes)
        {
            var bytes = p_bytes;
            if (bytes == null || bytes.Length == 0)
            {
                return IntPtr.Zero;
            }

            IntPtr p = Marshal.AllocHGlobal(bytes.Length);
            Marshal.Copy(bytes, 0, p, bytes.Length);

            return p;
        }

        public static IntPtr ToIntPtr(this byte[] p_bytes, int p_len)
        {
            var bytes = p_bytes;
            if (bytes == null)
            {
                return IntPtr.Zero;
            }

            if (bytes.Length > p_len)
            {
                throw new ArgumentException();
            }

            var cbytes = Enumerable.Repeat<byte>(0x00, p_len).ToArray();
            bytes.CopyTo(cbytes, 0);

            IntPtr p = Marshal.AllocHGlobal(cbytes.Length);
            Marshal.Copy(cbytes, 0, p, cbytes.Length);

            return p;
        }

        public static IntPtr ToIntPtr(this string p_data, Encoding p_encoding)
        {
            if (p_data == null)
            {
                throw new ArgumentNullException("p_data");
            }

            var bytes = p_encoding.GetBytes(p_data);

            return bytes.ToIntPtr(bytes.Length + 1);
        }

        public static void Free(this IntPtr p_prt)
        {
            if (p_prt == IntPtr.Zero)
            {
                return;
            }

            Marshal.FreeHGlobal(p_prt);
        }

        public static void Free(this IntPtr[] p_ptrs)
        {
            var ptrs = p_ptrs;
            if (ptrs == null)
            {
                return;
            }

            foreach (var ptr in ptrs)
            {
                ptr.Free();
            }
        }
    }
}