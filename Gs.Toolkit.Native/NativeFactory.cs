using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gs.Toolkit.Encrypt;

namespace Gs.Toolkit.Native
{
    public class NativeFactory
    {
        private static Dictionary<string, INative> _dic = new Dictionary<string, INative>();

        public static INative Create(string p_fileName)
        {
            var fileName = p_fileName;
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            fileName = Path.GetFullPath(fileName);

            var md5 = Md5.Create(fileName);

            lock (typeof(NativeFactory))
            {
                INative native;
                _dic.TryGetValue(md5, out native);

                if (native != null && native.IsDisposed)
                {
                    native = null;
                    _dic.Remove(md5);
                }

                if (native == null)
                {
                    native = new Gs.Toolkit.Native.Native(p_fileName);
                    _dic.Add(md5, native);
                }

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

            if (!native.IsDisposed)
            {
                native.Dispose();
            }

            var kv = _dic.FirstOrDefault(i => i.Value == native);

            lock (typeof(NativeFactory))
            {
                if (!string.IsNullOrEmpty(kv.Key))
                {
                    _dic.Remove(kv.Key);
                }
            }
        }
    }
}