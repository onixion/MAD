using System;
using System.Text;
using System.Net.Sockets;

namespace MadNet
{
    /*
     * This packet is used to send commands to the server.
     * Sometimes the server need the console-width of the client,
     * so he can compute the output and modify it to fit the 
     * screen. */

    public class CLIPacket : Packet
    {
        public string cliInput { get; set; }
        public int consoleWidth { get; set; }
        public string serverAnswer { get; set; }

        public CLIPacket(NetworkStream stream)
            : base(stream, 4) { }

        public override void SendPacketSpec(StreamIO streamIO, AES aes)
        {
            SendBytes(Encoding.Unicode.GetBytes(cliInput), aes);
            SendBytes(Ser(consoleWidth), aes);
            SendBytes(Encoding.Unicode.GetBytes(serverAnswer), aes);
        }

        public override void ReceivePacketSpec(StreamIO streamIO, AES aes)
        {
            cliInput = Encoding.Unicode.GetString(ReceiveBytes(aes));
            consoleWidth = (int)DeSer(ReceiveBytes(aes), typeof(Int32));
            serverAnswer = Encoding.Unicode.GetString(ReceiveBytes(aes));
        }
    }
}
