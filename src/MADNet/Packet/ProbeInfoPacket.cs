using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MadNet
{
    public class ProbeInfoPacket : Packet
    {  
        public string name { get; set; }
        public DateTime time { get; set; }
        public string cpuusage { get; set; }
        public string ramusage { get; set; }

        public ProbeInfoPacket(NetworkStream stream)
            : base(stream, 1) { }

        public override void SendPacketSpec(StreamIO streamIO, AES aes)
        {
            SendBytes(Encoding.Unicode.GetBytes(name), aes);
            SendBytes(Encoding.Unicode.GetBytes(time.ToString()), aes);
            SendBytes(Encoding.Unicode.GetBytes(cpuusage), aes);
            SendBytes(Encoding.Unicode.GetBytes(ramusage), aes);
        }

        public override void ReceivePacketSpec(StreamIO streamIO, AES aes)
        {
            name = Encoding.Unicode.GetString(ReceiveBytes(aes));
            time = DateTime.Parse(Encoding.Unicode.GetString(ReceiveBytes(aes)));
            cpuusage = Encoding.Unicode.GetString(ReceiveBytes(aes));
            ramusage = Encoding.Unicode.GetString(ReceiveBytes(aes));
        }
    }
}
