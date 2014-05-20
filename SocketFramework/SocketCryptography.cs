using System;
using System.IO;
using System.Security.Cryptography;

namespace SocketFramework
{
    public abstract class SocketCryptography
    {
        #region hashing

        public byte[] GetMD5Hash(byte[] data)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5CryptoServiceProvider.Create();
            return md5.ComputeHash(data);
        }

        #endregion


        public byte[] EncryptAES(byte[] data, byte[] key, byte[] iv)
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
