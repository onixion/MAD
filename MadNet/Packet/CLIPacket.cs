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
        public int consoleWidth { get; set; }
        public byte[] cliInput { get; set; }

        public CLIPacket(NetworkStream stream, AES aes)
            : base(stream, aes, 4) { }

        public CLIPacket(NetworkStream stream, AES aes, int consoleWidth, byte[] cliInput)
            : base(stream, aes, 4)
        {
            this.consoleWidth = consoleWidth;
            this.cliInput = cliInput;
        }

        public override void SendPacketSpec(StreamIO streamIO)
        {
            SendBytes(Encoding.Unicode.GetBytes(consoleWidth.ToString()));
            SendBytes(cliInput);
        }

        public override void ReceivePacketSpec(StreamIO streamIO)
        {
            consoleWidth = Int32.Parse(Encoding.Unicode.GetString(ReceiveBytes()));
            cliInput = ReceiveBytes();
        }
    }
}
