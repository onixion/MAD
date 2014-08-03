using System;
using System.Net.Sockets;

namespace MadNet
{
    public abstract class Packet : IDisposable
    {
        private StreamIO _streamIO;
        private AES _aes;

        private uint _packetType = 0;

        /* TYPE           UINT

         * NullPacket       | 0
         * DataPacket       | 1
         * LoginPacket      | 2
         * RSAPacket        | 3    
         * CLIPacket        | 4
         * ServerInfoPacket | 5

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

        private void SendPacketType()
        {
            _streamIO.Write(_packetType);
            _streamIO.Flush();
        }

        private uint ReceivePacketType()
        {
            return _streamIO.ReadUInt();
        }

        public void SendPacket()
        {
            SendPacketType();
            SendPacketSpec(_streamIO);
        }

        public void ReceivePacket()
        {
            if (_packetType == ReceivePacketType())
                ReceivePacketSpec(_streamIO);
            else
                throw new PacketException("No packet or wrong packet!", null);
        }

        public abstract void SendPacketSpec(StreamIO streamIO);

        public abstract void ReceivePacketSpec(StreamIO streamIO);

        protected void SendBytes(byte[] data)
        {
            if (data != null)
            {
                if (_aes == null)
                    _streamIO.Write(data, true);
                else
                    _streamIO.Write(_aes.Encrypt(data), true);
            }
            else
            {
                _streamIO.Write(new byte[0], true);
            }
            _streamIO.Flush();
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
