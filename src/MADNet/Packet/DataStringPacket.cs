using System;
using System.Text;
using System.Net.Sockets;

namespace MadNet
{
    public class DataStringPacket : Packet
    {
        public string data { get; set; }

        public DataStringPacket(NetworkStream stream)
            : base(stream, 1) { }

        public DataStringPacket(NetworkStream stream, string data)
            : base(stream, 1)
        {
            this.data = data;
        }

        public override void SendPacketSpec(StreamIO streamIO, AES aes)
        {
            SendBytes(Encoding.Unicode.GetBytes(data), aes);
        }

        public override void ReceivePacketSpec(StreamIO streamIO, AES aes)
        {
            data = Encoding.Unicode.GetString(ReceiveBytes(aes));
        }
    }
}
