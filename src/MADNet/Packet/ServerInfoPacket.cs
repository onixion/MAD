using System;
using System.Text;
using System.Net.Sockets;

namespace MadNet
{
    public class ServerInfoPacket : Packet
    {
        public byte[] serverHeader { get; set; }
        public byte[] serverVersion { get; set; }

        public ServerInfoPacket(NetworkStream stream)
            : base(stream, 5) { }

        public ServerInfoPacket(NetworkStream stream, byte[] serverHeader, byte[] serverVersion)
            : base(stream, 5)
        {
            this.serverHeader = serverHeader;
            this.serverVersion = serverVersion;
        }

        public override void SendPacketSpec(StreamIO streamIO, AES aes)
        {
            SendBytes(serverHeader, aes);
            SendBytes(serverVersion, aes);
        }

        public override void ReceivePacketSpec(StreamIO streamIO, AES aes)
        {
            serverHeader = ReceiveBytes(aes);
            serverVersion = ReceiveBytes(aes);
        }
    }
}
