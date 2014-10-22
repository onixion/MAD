using System;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Xml;
using System.Xml.Serialization;

namespace MadNet
{
    public abstract class Packet : IDisposable
    {
        private StreamIO _streamIO;
        private AES _aes;

        private uint _packetType = 0;

        /* TYPE               UINT

         * NullPacket       | 0
         * DataPacket       | 1
         * LoginPacket      | 2
         * SSLPacket        | 3    
         * CLIPacket        | 4
         * ServerInfoPacket | 5

         */

        public Packet(Stream stream)
        {
            _streamIO = new StreamIO(stream);
        }

        public Packet(Stream stream, AES aes)
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

        protected  byte[] Ser(object toSerialize)
        {
            try
            {
                XmlSerializer _serializer = new XmlSerializer(toSerialize.GetType());
                using (MemoryStream _ms = new MemoryStream())
                {
                    _serializer.Serialize(_ms, toSerialize);
                    _ms.Position = 0;
                    return _ms.ToArray();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected object DeSer(byte[] toDeserialize, Type type)
        {
            try
            {
                XmlSerializer _serializer = new XmlSerializer(type);
                using (MemoryStream _ms = new MemoryStream(toDeserialize))
                {
                    _ms.Position = 0;
                    return _serializer.Deserialize(_ms);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void Dispose()
        {
            _streamIO.Dispose();
        }
    }
}
