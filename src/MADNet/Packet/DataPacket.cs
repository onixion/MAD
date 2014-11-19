using System;
using System.Net.Sockets;

namespace MadNet
{
    public class DataPacket : Packet
    {
        public byte[] data { get; set; }

        public DataPacket(NetworkStream stream)
            : base(stream, 1) { }

        public DataPacket(NetworkStream stream, byte[] data)
            : base(stream, 1)
        {
            this.data = data;
        }

        public override void SendPacketSpec(StreamIO streamIO, AES aes)
        {
            SendBytes(data, aes);
        }

        public override void ReceivePacketSpec(StreamIO streamIO, AES aes)
        {
            data = ReceiveBytes(aes);
        }
    }
}
