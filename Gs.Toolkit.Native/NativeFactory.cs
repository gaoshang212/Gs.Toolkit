using System;

namespace Gs.Toolkit.Native
{
    public class NativeFactory
    {
        public static INative Create(string p_fileName)
        {
            lock (typeof(NativeFactory))
            {
                var native = new Native(p_fileName);

                return native;
            }
        }

        public static INative Create(string p_fileName, IDisposable p_disposable)
        {
            lock (typeof(NativeFactory))
            {
                var native = new Native(p_fileName, p_disposable);

                return native;
            }
        }

        public static void Free(INative p_native)
        {
            var native = p_native;
            if (native == null)
            {
                return;
            }

            native.Dispose();
        }
    }
}