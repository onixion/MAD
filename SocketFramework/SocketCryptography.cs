using System;
using System.IO;
using System.Security.Cryptography;

namespace SocketFramework
{
    public static class SocketCryptography
    {
        public static byte[] EncryptAES(byte[] data, byte[] key, byte[] iv)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Rijndael alg = Rijndael.Create();
                alg.Key = key;
                alg.IV = iv;

                using (CryptoStream cStream = new CryptoStream(stream, alg.CreateEncryptor(), CryptoStreamMode.Write))
                    cStream.Write(data, 0, data.Length);

                return stream.ToArray();
            }
        }
    }
}
