using System.Security.Cryptography;
using System.Text;

namespace Gs.Toolkit.Encrypt
{
    public class Md5
    {
        public string Encrypt(string p_input, Encoding p_encoding, string p_key)
        {
            var md5 = MD5.Create();
            var inputBytes = p_encoding.GetBytes(p_input);
            var hash = md5.ComputeHash(inputBytes);

            return ToHexString(hash, "x2");
        }

        public static string Encrypt(string p_input, Encoding p_encoding)
        {
            Md5 md5 = new Md5();
            return md5.Encrypt(p_input, p_encoding, null);
        }

        public static byte[] Encrypt(byte[] p_input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(p_input);
            return hash;
        }

        public static string Create(byte[] p_input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(p_input);
            return ToHexString(hash);
        }

        public static byte[] Encrypt(string p_input)
        {
            var md5 = MD5.Create();
            var input = Encoding.Default.GetBytes(p_input);
            var hash = md5.ComputeHash(input);
            return hash;
        }

        public static string Create(string p_input)
        {
            return Create(Encoding.UTF8.GetBytes(p_input));
        }

        private static string ToHexString(byte[] hash, string p_format = "x2")
        {
            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString(p_format));
            }
            return sb.ToString();
        }
    }
}
