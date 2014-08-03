using System;
using System.Net;
using System.Net.Sockets;

namespace MadNet
{
    /*
     * This packet is used to send the login data to the server. */

    public class LoginPacket : Packet
    {
        public byte[] user { get; set; }
        public byte[] passMD5 { get; set; }

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
            SendBytes(user);
            SendBytes(passMD5);
        }

        public override void ReceivePacketSpec(StreamIO streamIO)
        {
            user = ReceiveBytes();
            passMD5 = ReceiveBytes();
        }
    }
}
