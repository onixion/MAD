using System;
using System.Text;
using System.Security.Cryptography;

namespace MadNet
{
    public static class MadNetHelper
    {
        private static SHA1 _SHA1 = new SHA1CryptoServiceProvider();
        private static SHA256 _SHA256 = new SHA256CryptoServiceProvider();
        private static SHA512 _SHA512 = new SHA512CryptoServiceProvider();

        private static Random _rand;

        #region SHA1 hashing

        public static byte[] GenSHA1(byte[] data)
        {
            return  _SHA1.ComputeHash(data);
        }

        public static string GenSHA1(string unicodeString)
        {
            return Encoding.Unicode.GetString(GenSHA1(Encoding.Unicode.GetBytes(unicodeString)));
        }

        #endregion

        #region SHA256 hashing

        public static byte[] GenSHA256(byte[] data)
        {
            return _SHA256.ComputeHash(data);
        }

        public static string GenSHA256(string unicodeString)
        {
            return Encoding.Unicode.GetString(GenSHA256(Encoding.Unicode.GetBytes(unicodeString)));
        }

        #endregion

        #region SHA512 hashing

        public static byte[] GenSHA512(byte[] data)
        {
            return _SHA512.ComputeHash(data);
        }

        public static string GenSHA512(string unicodeString)
        {
            return Encoding.Unicode.GetString(GenSHA512(Encoding.Unicode.GetBytes(unicodeString)));
        }

        #endregion

        public static byte[] CombineByteArrays(byte[] Array1, byte[] Array2)
        {
            byte[] resultArray = new byte[Array1.Length + Array2.Length];
            Array.Copy(Array1, resultArray, Array1.Length);
            Array.Copy(Array2, 0, resultArray, Array1.Length, Array2.Length);
            return resultArray;
        }

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
