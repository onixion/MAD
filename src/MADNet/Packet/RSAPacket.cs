using System;
using System.Net.Sockets;

namespace MadNet
{
    public class RSAPacket : Packet
    {
        public byte[] modulus { get; set; }
        public byte[] exponent { get; set; }

        public RSAPacket(NetworkStream stream, AES aes)
            : base(stream, aes, 1) { }

        public RSAPacket(NetworkStream stream, AES aes, byte[] modulus, byte[] exponent)
            : base(stream, aes, 1)
        {
            this.modulus = modulus;
            this.exponent = exponent;
        }

        public override void SendPacketSpec(StreamIO streamIO)
        {
            SendBytes(modulus);
            SendBytes(exponent);
        }

        public override void ReceivePacketSpec(StreamIO streamIO)
        {
            modulus = ReceiveBytes();
            exponent= ReceiveBytes();
        }
    }
}
