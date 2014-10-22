using System;
using System.Net.Sockets;
using System.Net.Security;

namespace MadNet
{
    public class SSLPacket : Packet
    {
        public byte[] data;

        public SSLPacket(SslStream stream)
            : base(stream) { }

        public SSLPacket(SslStream stream, byte[] data)
            : base(stream)
        {
            this.data = data;
        }

        public override void SendPacketSpec(StreamIO streamIO)
        {
            SendBytes(data);
        }

        public override void ReceivePacketSpec(StreamIO streamIO)
        {
            data = ReceiveBytes();
        }
    }
}
