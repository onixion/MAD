using System;
using System.Net.Sockets;

namespace MadNet
{
    public class DataPacket : Packet
    {
        public byte[] data { get; set; }

        public DataPacket(NetworkStream stream, AES aes)
            : base(stream, aes, 1) { }

        public DataPacket(NetworkStream stream, AES aes, byte[] data)
            : base(stream, aes, 1)
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
