using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

using CryptoNet;

namespace MadNet
{
    public class AES : IDisposable
    {
        //RijndaelOpen _aes = new RijndaelOpen();
        Rijndael _aes = Rijndael.Create();

        private string _pass { get; set; }
        private static byte[] _salt = new byte[] { 0x11, 0x33, 0x3A, 0x4D, 0x32, 0xFF, 0xE2, 0x95 };

        public AES(string pass)
        {
            _pass = pass;
        }

        public AES(string pass, byte[] salt)
        {
            _pass = pass;
            _salt = salt;
        }

        public byte[] Encrypt(byte[] data)
        {
            if (data != null)
            {
                if (data.Length != 0)
                {
                    PasswordDeriveBytes _pdb = new PasswordDeriveBytes(Encoding.Unicode.GetBytes(_pass), _salt);
                    return AESEncryption(data, _pdb.GetBytes(32), _pdb.GetBytes(16));
                }
                else
                {
                    return data;
                }
            }
            else
            {
                return new byte[0];
            }
        }

        public byte[] Decrypt(byte[] data)
        {
            if (data != null)
            {
                if (data.Length != 0)
                {
                    PasswordDeriveBytes _pdb = new PasswordDeriveBytes(Encoding.Unicode.GetBytes(_pass), _salt);
                    return AESDecryption(data, _pdb.GetBytes(32), _pdb.GetBytes(16));
                }
                else
                {
                    return data;
                }
            }
            else
            {
                return new byte[0];
            }
        }

        private byte[] AESEncryption(byte[] data, byte[] key, byte[] iv)
        {
            RijndaelOpen _aes = new RijndaelOpen();

            _aes.Key = key;
            _aes.IV = iv;

            ICryptoTransform _transform = _aes.CreateEncryptor(key, iv);

            using (MemoryStream _stream = new MemoryStream())
            {
                using (CryptoStream _cryptoStream = new CryptoStream(_stream, _transform, CryptoStreamMode.Write))
                {
                    _cryptoStream.Write(data, 0, data.Length);
                    _cryptoStream.FlushFinalBlock();
                }

                data = _stream.ToArray();
            }

            return data;
        }

        private byte[] AESDecryption(byte[] data, byte[] key, byte[] iv)
        {
            _aes.Key = key;
            _aes.IV = iv;

            ICryptoTransform _transform = _aes.CreateDecryptor(key, iv);

            using (MemoryStream _stream = new MemoryStream())
            {
                using (CryptoStream _cryptoStream = new CryptoStream(_stream, _transform, CryptoStreamMode.Write))
                {
                    _cryptoStream.Write(data, 0, data.Length);
                    _cryptoStream.FlushFinalBlock();
                }

                data = _stream.ToArray();
            }

            return data;
        }

        public void Dispose()
        {
            _aes.Dispose();
        }
    }
}
