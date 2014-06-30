using System;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace nc
{
    public static class NetCom
    {
        #region sending / receiving WITHOUT encryption

        public static void SendString(NetworkStream stream, string data, bool flush)
        {
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(data);
            
            if (flush)
            {
                writer.Flush();
            }
        }

        public static string ReceiveString(NetworkStream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            return reader.ReadString();
        }
        
        #endregion

        #region sending / receiving WITH AES encryption (symmetric)

        public enum KeySize { Bit128, Bit192, Bit256 }

        public static void SendStringAES(NetworkStream stream, string data, string pass, KeySize keySize, bool flush)
        {
            BinaryWriter _writer = new BinaryWriter(stream);
            _writer.Write(EncryptAES(data, pass, GetKeySize(keySize)));

            if (flush)
            {
                _writer.Flush();
            }
        }

        public static string ReceiveAES(NetworkStream stream, string pass, KeySize keySize)
        {
            BinaryReader _reader = new BinaryReader(stream);
            return DecryptAES(_reader.ReadString(), pass, GetKeySize(keySize));
        }

        public static string EncryptAES(string data, string pass, int keySize)
        {
            byte[] _pass = Encoding.UTF8.GetBytes(pass);
            byte[] _salt = Encoding.ASCII.GetBytes("Kosher");
            
            PasswordDeriveBytes _passDerive = new PasswordDeriveBytes(_pass, _salt, "SHA1", 2);

            byte[] _key = _passDerive.GetBytes(keySize / 8);

            RijndaelManaged _symKey = new RijndaelManaged();
            _symKey.Mode = CipherMode.CBC;

            byte[] _iv = Encoding.ASCII.GetBytes("DrdsfEikrDslFeof");
            byte[] _data = Encoding.ASCII.GetBytes(data);
            byte[] _clipherText = null;

            using (ICryptoTransform _encryptor = _symKey.CreateEncryptor(_key, _iv))
            {
                using (MemoryStream _stream = new MemoryStream())
                {
                    using (CryptoStream _cryptStream = new CryptoStream(_stream, _encryptor, CryptoStreamMode.Write))
                    {
                        _cryptStream.Write(_data, 0, _data.Length);
                        _cryptStream.FlushFinalBlock();
                        _clipherText = _stream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(_clipherText);
        }

        public static string DecryptAES(string clipherText, string pass, int keySize)
        {
            byte[] _pass = Encoding.UTF8.GetBytes(pass);
            byte[] _salt = Encoding.ASCII.GetBytes("Kosher");

            PasswordDeriveBytes _passDerive = new PasswordDeriveBytes(_pass, _salt, "SHA1", 2);

            byte[] _key = _passDerive.GetBytes(keySize / 8);

            RijndaelManaged _symKey = new RijndaelManaged();
            _symKey.Mode = CipherMode.CBC;

            byte[] _iv = Encoding.ASCII.GetBytes("DrdsfEikrDslFeof");

            byte[] _clipherText = Convert.FromBase64String(clipherText);
            byte[] _data = new byte[clipherText.Length];

            using (ICryptoTransform _decryptor = _symKey.CreateDecryptor(_key, _iv))
            {
                using (MemoryStream _stream = new MemoryStream())
                {
                    _stream.Write(_clipherText, 0, _clipherText.Length);
                    _stream.Flush();
                    _stream.Seek(0, SeekOrigin.Begin);

                    using (CryptoStream _cryptStream = new CryptoStream(_stream, _decryptor, CryptoStreamMode.Read))
                    {
                        _cryptStream.Read(_data, 0, _data.Length);
                    }
                }
            }

            return Encoding.ASCII.GetString(_data);
        }

        private static int GetKeySize(KeySize keySize)
        {
            switch (keySize)
            { 
                case KeySize.Bit128:
                    return 128;
                case KeySize.Bit192:
                    return 192;
                case KeySize.Bit256:
                    return 256;
                default:
                    throw new Exception("INVALID-KEYSIZE!");
            }
        }

        #endregion

        #region asymmetric encryption handshake

        public static string AsymmetricHandshakeTransmitter(NetworkStream stream)
        {
            return null;
        }

        public static string AsymmetricHandshakeReceiver(NetworkStream stream)
        {
            return null;
        }

        private static void ComputeAsymmetricHandshake()
        { 
        
        }

        public static long GetBigPrimeNumber()
        {
            return 0;
        }

        public static byte[] GenRandomPassword(int length)
        {
            Random _ran = new Random((int)DateTime.Now.Ticks);
            byte[] _buffer = new byte[length];

            for (int i = 0; i < length; i++)
            {

            }

            return _buffer;
        }

        #endregion

        #region Hashing

        public static string GetHash(string data)
        { 
            MD5 md5 = MD5CryptoServiceProvider.Create();
            return Encoding.ASCII.GetString(md5.ComputeHash(Encoding.ASCII.GetBytes(data)));
        }

        #endregion

        #region TimeStamps

        public static string DateStamp()
        {
            return DateTime.Now.ToString("dd.MM.yyyy");
        }

        public static string TimeStamp()
        {
            return DateTime.Now.ToString("hh:mm:ss");
        }

        #endregion
    }
}
