using System;
using System.Text;
using System.Net.Sockets;

namespace MadNet
{
    public class DataStringPacket : Packet
    {
        public string data { get; set; }

        public DataStringPacket(NetworkStream stream, AES aes)
            : base(stream, aes, 1) { }

        public DataStringPacket(NetworkStream stream, AES aes, string data)
            : base(stream, aes, 1)
        {
            this.data = data;
        }

        public override void SendPacketSpec(StreamIO streamIO)
        {
            SendBytes(Encoding.Unicode.GetBytes(data));
        }

        public override void ReceivePacketSpec(StreamIO streamIO)
        {
            data = Encoding.Unicode.GetString(ReceiveBytes());
        }
    }
}
