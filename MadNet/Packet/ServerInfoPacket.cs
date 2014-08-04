using System;
using System.Text;
using System.Net.Sockets;

namespace MadNet
{
    public class ServerInfoPacket : Packet
    {

        public byte[] serverHeader { get; set; }
        public byte[] serverVersion { get; set; }

        public ServerInfoPacket(NetworkStream stream, AES aes)
            : base(stream, aes, 5) { }

        public ServerInfoPacket(NetworkStream stream, AES aes, byte[] serverHeader, byte[] serverVersion)
            : base(stream, aes, 5)
        {
            this.serverHeader = serverHeader;
            this.serverVersion = serverVersion;
        }

        public override void SendPacketSpec(StreamIO streamIO)
        {
            SendBytes(serverHeader);
            SendBytes(serverVersion);
        }

        public override void ReceivePacketSpec(StreamIO streamIO)
        {
            serverHeader = ReceiveBytes();
            serverVersion = ReceiveBytes();
        }
    }
}
