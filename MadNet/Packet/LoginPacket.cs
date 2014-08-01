using System;
using System.Net;
using System.Net.Sockets;

namespace MadNet
{
    public class LoginPacket : Packet
    {
        public byte[] user;
        public byte[] passMD5;

        public LoginPacket(NetworkStream stream, AES aes)
            : base(stream, aes, 2) { }

        public LoginPacket(NetworkStream stream, AES aes, byte[] user, byte[] passMD5)
            : base(stream, aes, 2)
        {
            this.user = user;
            this.passMD5 = passMD5;
        }

        public override void SendPacketSpec(StreamIO streamIO)
        {
            streamIO.Write(user, true);
            streamIO.Write(passMD5, true);
        }

        public override void ReceivePacketSpec(StreamIO streamIO)
        {
            user = streamIO.ReadBytes();
            passMD5 = streamIO.ReadBytes();
        }
    }
}
