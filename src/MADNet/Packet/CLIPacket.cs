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

        public CLIPacket(NetworkStream stream, AES aes)
            : base(stream, aes, 4) { }

        public override void SendPacketSpec(StreamIO streamIO)
        {
            SendBytes(Encoding.Unicode.GetBytes(cliInput));
            SendBytes(Ser(consoleWidth));
            SendBytes(Encoding.Unicode.GetBytes(serverAnswer));
        }

        public override void ReceivePacketSpec(StreamIO streamIO)
        {
            cliInput = Encoding.Unicode.GetString(ReceiveBytes());
            consoleWidth = (int)DeSer(ReceiveBytes(), typeof(Int32));
            serverAnswer = Encoding.Unicode.GetString(ReceiveBytes());
        }
    }
}
