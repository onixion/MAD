using System;
using System.Net;
using System.Net.Sockets;

namespace MadNet
{
    public class RSAPacket : Packet
    {
        public byte[] publicKey { get; set; }
        public byte[] modulus { get; set; }
        public int modulusLength { get; set; }

        public RSAPacket(NetworkStream stream, AES aes)
            : base(stream, aes, 3) { }

        public RSAPacket(NetworkStream stream, AES aes, byte[] publicKey, byte[] modulus, int modulusLength)
            : base(stream, aes, 3)
        {
            this.publicKey = publicKey;
            this.modulus = modulus;
            this.modulusLength = modulusLength;
        }

        public override void SendPacketSpec(StreamIO streamIO)
        {
            streamIO.Write(publicKey, true);
            streamIO.Write(modulus, true);
            streamIO.Write(modulusLength);
        }

        public override void ReceivePacketSpec(StreamIO streamIO)
        {
            publicKey = streamIO.ReadBytes();
            modulus = streamIO.ReadBytes();
            modulusLength = streamIO.ReadInt();
        }
    }
}
