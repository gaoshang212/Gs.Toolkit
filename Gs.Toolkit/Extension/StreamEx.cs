using System.IO;
using System.Text;

namespace Gs.Toolkit.Extension
{
    public static class StreamEx
    {
        /// <summary>
        /// 默认Utf8
        /// </summary>
        /// <param name="p_stream"></param>
        /// <param name="p_data"></param>
        public static void Add(this Stream p_stream, string p_data)
        {
            p_stream.Add(p_data, Encoding.UTF8);
        }

        public static void Add(this Stream p_stream, string p_data, Encoding p_encoding)
        {
            var buffer = p_encoding.GetBytes(p_data);
            p_stream.Add(buffer);
        }

        public static void Add(this Stream p_stream, Stream p_src)
        {
            using (BinaryReader br = new BinaryReader(p_src))
            {
                var buffer = br.ReadBytes((int)p_src.Length);
                p_stream.Add(buffer);
            }
        }

        public static void Add(this Stream p_stream, byte[] p_data)
        {
            p_stream.Write(p_data, 0, p_data.Length);
        }

        public static byte[] ReadAllBytes(this Stream p_stream)
        {
            p_stream.Position = 0;
            using (BinaryReader br = new BinaryReader(p_stream))
            {
                return br.ReadBytes((int)p_stream.Length);
            }
        }
    }
}