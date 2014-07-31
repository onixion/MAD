﻿using System;
using System.Net.Sockets;

namespace MadNet
{
    public abstract class Packet : IDisposable
    {
        private StreamIO _streamIO;
        private AES _aes;

        private uint _packetType = 0;

        /* TYPE           UINT
         * NullPacket   | 0
         * DataPacket   | 1
         * LoginPacket  | 2
         * RSAPacket    | 3                   
         */

        public Packet(NetworkStream stream, AES aes)
        {
            _streamIO = new StreamIO(stream);
            _aes = aes;
        }

        public Packet(NetworkStream stream, AES aes, uint packetType)
        {
            _streamIO = new StreamIO(stream);
            _aes = aes;
            _packetType = packetType;
        }

        public void SendPacket()
        {
            _streamIO.Write(_packetType);
            SendPacketSpec(_streamIO);
        }

        public void ReceivePacket()
        {
            if (_packetType == _streamIO.ReadUInt())
                ReceivePacketSpec(_streamIO);
            else
                throw new PacketException("Wrong packet type!", null);
        }

        public abstract void SendPacketSpec(StreamIO streamIO);

        public abstract void ReceivePacketSpec(StreamIO streamIO);

        protected void SendBytes(byte[] data)
        {
            if (_aes == null)
            {
                _streamIO.Write(data, true);
                _streamIO.Flush();
            }
            else
            {
                _streamIO.Write(_aes.Encrypt(data), true);
                _streamIO.Flush();
            }
        }

        protected byte[] ReceiveBytes()
        {
            if (_aes == null)
            {
                uint _length = _streamIO.ReadUInt();
                return _streamIO.ReadBytes(_length);
            }
            else
            {
                uint _length = _streamIO.ReadUInt();
                return _aes.Decrypt(_streamIO.ReadBytes(_length));
            }
        }

        public void Dispose()
        {
            _streamIO.Dispose();
        }
    }
}
