using System;
using System.Text;
using System.Security.Cryptography;

namespace MadNet
{
    public static class MadNetHelper
    {
        private static SHA1 _SHA1 = new SHA1CryptoServiceProvider();
        private static MD5 _MD5 = new MD5CryptoServiceProvider();
        private static Random _rand;

        #region SHA1 hashing

        public static byte[] ToSHA1(byte[] data)
        {
            return _SHA1.ComputeHash(data);
        }

        public static byte[] ToSHA1(string unicodeString)
        {
            return ToMD5(Encoding.Unicode.GetBytes(unicodeString));
        }

        #endregion

        #region MD5 hashing

        public static byte[] ToMD5(byte[] data)
        {
            return _MD5.ComputeHash(data);
        }

        public static string ToMD5(string unicodeString)
        {
            return Encoding.Unicode.GetString(ToMD5(Encoding.Unicode.GetBytes(unicodeString)));
        }

        #endregion

        public static string GetUnicodeRandom(int length)
        {
            _rand = new Random((int)DateTime.Now.Ticks);

            byte[] str = new byte[length * 2];
            for (int i = 0; i < length * 2; i += 2)
            {
                int chr = _rand.Next(0xD7FF);
                str[i + 1] = (byte)((chr & 0xFF00) >> 8);
                str[i] = (byte)(chr & 0xFF);
            }

            return Encoding.Unicode.GetString(str);
        }

        public static string GetTimeStamp()
        {
            return DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss");
        }
    }
}
