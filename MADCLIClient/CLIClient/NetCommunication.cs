using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Net.Sockets;

namespace CLIClient
{
    public static class NetCommunication
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

        #region sending / receiving WITH encryption

        public static void SendStringAES128(NetworkStream stream, string data, string pass, bool flush)
        {

        }

        public static string ReceiveAES128(NetworkStream stream, string pass)
        {
            return null;
        }

        private static string AESEncrypt(byte[] key, byte[] iv, string data)
        {
            if (key.Length != 0 || iv.Length != 0 || data.Length != 0)
            {
                return null;
            }
            else
            {
                return null;
            }
        }

        private static string AESDecrypt(int blocksize, byte[] key, byte[] iv, string data)
        {
            if (key.Length != 0 || iv.Length != 0 || data.Length != 0)
            {
                return null;
            }
            else
            {
                return null;
            }
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
