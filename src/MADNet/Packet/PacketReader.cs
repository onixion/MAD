using System;
using System.Net.Sockets;

namespace MadNet
{
    public static class PacketReader
    {
        // NOT TESTED
        public static Packet ReadPacket(NetworkStream stream, AES aes)
        {
            StreamIO _streamIO = new StreamIO(stream);
            uint _packetType = _streamIO.ReadUInt();

            switch (_packetType)
            {
                case 1: // DataPacket
                    DataPacket _dataPacket = new DataPacket(stream, aes);
                    _dataPacket.ReceivePacketSpec(_streamIO);
                    return _dataPacket;

                case 2: // LoginPacket
                    LoginPacket _loginPacket = new LoginPacket(stream, aes);
                    _loginPacket.ReceivePacketSpec(_streamIO);
                    return _loginPacket;

                default:
                    throw new PacketException("Packet type not known!", null);
            }
        }
    }
}
