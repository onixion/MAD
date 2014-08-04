using System;
using System.Text;
using System.Net.Sockets;

namespace MadNet
{
    /*
     * This packet is used to send the server the public-key,
     * modulus and modulus-length. These things are needed
     * for RSA-enryption. */

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
            SendBytes(publicKey);
            SendBytes(modulus);
            SendBytes(Ser(modulusLength));
        }

        public override void ReceivePacketSpec(StreamIO streamIO)
        {
            publicKey = ReceiveBytes();
            modulus = ReceiveBytes();
            modulusLength = (int)DeSer(ReceiveBytes(), modulusLength.GetType());
        }
    }
}
