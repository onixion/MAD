using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace MadNet
{
    public class AES : IDisposable
    {
        private Rijndael _aes = Rijndael.Create();
        private RandomNumberGenerator _gen = RNGCryptoServiceProvider.Create();
        private Rfc2898DeriveBytes _pdb = null;
        private byte[] _buffer = null;

        public AES(string pass)
        {
            _pdb = new Rfc2898DeriveBytes(Encoding.Unicode.GetBytes(pass), 
                new byte[]{25,34,56,32,46,32,54,76,12,128,250,234,21,23,63,74,35}, 10);

            _aes.Mode = CipherMode.CBC;
            _aes.BlockSize = 128;
        }

        public byte[] Encrypt(byte[] data)
        {
            if (data != null)
            {
                if (data.Length != 0)
                {
                    _pdb.Reset();
                    return AESEncryption(data, _pdb.GetBytes(16));
                }
                else
                    return data;
            }
            else
                return new byte[0];
        }

        public byte[] Decrypt(byte[] data)
        {
            if (data != null)
            {
                if (data.Length != 0)
                {
                    _pdb.Reset();
                    return AESDecryption(data, _pdb.GetBytes(16));
                }
                else
                    return data;
            }
            else
                return new byte[0];
        }

        private byte[] AESEncryption(byte[] data, byte[] key)
        {
            _aes.Key = key;
            _aes.IV = new byte[16];

            using (ICryptoTransform _transform = _aes.CreateEncryptor(_aes.Key, _aes.IV))
            using (MemoryStream _stream = new MemoryStream())
            {
                using (CryptoStream _cryptoStream = new CryptoStream(_stream, _transform, CryptoStreamMode.Write))
                {
                    _buffer = new byte[16];
                    _gen.GetBytes(_buffer);
                    _cryptoStream.Write(_buffer, 0, 16);
                    _cryptoStream.Write(data, 0, data.Length);

                }
                return _stream.ToArray();
            }
        }

        private byte[] AESDecryption(byte[] data, byte[] key)
        {
            _aes.Key = key;
            _aes.IV = new byte[16];

            using (ICryptoTransform _transform = _aes.CreateDecryptor(_aes.Key, _aes.IV))
            using (MemoryStream _stream = new MemoryStream(data))
            using (MemoryStream _stream2 = new MemoryStream())
            {
                using (CryptoStream _cryptoStream = new CryptoStream(_stream, _transform, CryptoStreamMode.Read))
                {
                    _buffer = new byte[16];
                    _cryptoStream.Read(_buffer, 0, 16);
                    _cryptoStream.CopyTo(_stream2);
                }
                return _stream2.ToArray();
            }
        }

        public void Dispose()
        {
            _aes.Dispose();
            _pdb.Dispose();
            _gen.Dispose();
        }
    }
}
