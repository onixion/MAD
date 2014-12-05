using System;
using System.Text;
using System.Net.NetworkInformation;
using System.Security.Cryptography;

namespace MadNet
{
    public static class MadNetHelper
    {
        private static SHA1 _SHA1 = new SHA1CryptoServiceProvider();
        private static SHA256 _SHA256 = new SHA256CryptoServiceProvider();
        private static SHA512 _SHA512 = new SHA512CryptoServiceProvider();

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

        public static string ParseMacAddress(string mac)
        {
            if (mac.Length < 12)
                mac = mac.PadRight(12 - mac.Length, '0');
            else if (mac.Length > 12)
                throw new Exception("MAC-Address too long!");

            return PhysicalAddress.Parse(mac).ToString();
        }

        public static string GetTimeStamp()
        {
            return DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss");
        }
    }
}
