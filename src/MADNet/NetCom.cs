using System;
using System.IO;
using System.Text;
using System.Net.Sockets;

using CryptoNet;

namespace MadNet
{
    public static class NetCom
    {
        #region sending / receiving WITHOUT encryption (byte array)

        public static void SendByte(NetworkStream stream, byte[] data, bool flush)
        {
            using (StreamIO _stream = new StreamIO(stream))
            {
                _stream.Write(data, true);
                if (flush)
                    _stream.Flush();
            }
        }

        public static byte[] ReceiveByte(NetworkStream stream)
        {
            using (StreamIO _stream = new StreamIO(stream))
            {
                uint _length = _stream.ReadUInt();
                return _stream.ReadBytes(_length);
            }
        }

        #endregion

        #region sending / receiving WITHOUT encryption (encoding UNICODE)

        public static void SendStringUnicode(NetworkStream stream, string data, bool flush)
        {
            using (StreamIO _stream = new StreamIO(stream))
            {
                _stream.Write(Encoding.Unicode.GetBytes(data), true);

                if (flush)
                    _stream.Flush();
            }
        }

        public static string ReceiveStringUnicode(NetworkStream stream)
        {
            using (StreamIO _stream = new StreamIO(stream))
            {
                uint _length = _stream.ReadUInt();
                return Encoding.Unicode.GetString(_stream.ReadBytes(_length));
            }
        }

        public static void SendInt(NetworkStream stream, int data, bool flush)
        {
            using (StreamIO _stream = new StreamIO(stream))
            {
                _stream.Write(BitConverter.GetBytes(data), true);

                if (flush)
                    _stream.Flush();
            }
        }

        public static int ReceiveInt(NetworkStream stream)
        {
            using (StreamIO _stream = new StreamIO(stream))
            {
                return _stream.ReadInt();
            }
        }

        #endregion

        #region sending / receiving WITH AES encryption (symmetric) (encoding UNICODE)
        /*
        public static void SendStringAESUnicode(NetworkStream stream, string data, string pass, bool flush)
        {
            using (StreamIO _stream = new StreamIO(stream))
            {
                byte[] _data = Encoding.Unicode.GetBytes(data);
                _stream.Write(AESCryptography.AESEncryption(_data, pass), true);

                if (flush)
                    _stream.Flush(); 
            }
        }

        public static string ReceiveStringAESUnicode(NetworkStream stream, string pass)
        {
            using (StreamIO _stream = new StreamIO(stream))
            {
                uint _length = _stream.ReadUInt();
                return Encoding.Unicode.GetString(AESCryptography.AESDecryption(_stream.ReadBytes(_length),pass));
            }
        }
        */
        #endregion
    }
}
