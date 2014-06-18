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

        #region sending / receiving WITH AES encryption

        public static void SendStringAES128(NetworkStream stream, string data, string pass, bool flush)
        {


        }

        public static string ReceiveAES128(NetworkStream stream, string pass)
        {
            return null;
        }

        public static string EncryptAES(string data, string pass)
        {
            byte[] _pass = Encoding.UTF8.GetBytes(pass);
            byte[] _salt = Encoding.ASCII.GetBytes("Kosher");
            
            PasswordDeriveBytes _passDerive = new PasswordDeriveBytes(_pass, _salt, "SHA1", 2);

            byte[] _key = _passDerive.GetBytes(256 / 8);

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

        public static string DecryptAES(string clipherText, string pass)
        {
            byte[] _pass = Encoding.UTF8.GetBytes(pass);
            byte[] _salt = Encoding.ASCII.GetBytes("Kosher");

            PasswordDeriveBytes _passDerive = new PasswordDeriveBytes(_pass, _salt, "SHA1", 2);

            byte[] _key = _passDerive.GetBytes(256 / 8);

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
