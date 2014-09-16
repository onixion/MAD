using System;
using System.Security.Cryptography;

namespace MadNet
{
    public static class SHA
    {
        private static SHA1 sha = new SHA1CryptoServiceProvider();

        public static byte[] ComputeSHA1(byte[] data)
        {
            return sha.ComputeHash(data, 0, data.Length);
        }

        public static string GenFingerPrint(byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", ":");
        }

    }
}
