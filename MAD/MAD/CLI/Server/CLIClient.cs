using System;
using System.Net.Sockets;

namespace MAD
{
    public class CLIClient
    {
        public TcpClient client;
        public NetworkStream clientStream;

        public CLIClient(TcpClient client)
        {
            this.client = client;
            clientStream = client.GetStream();
        }

        public byte[] BeginReadData(int length)
        {
            byte[] temp = new byte[length];

            //clientStream.Read(temp, 0, temp.Length);

            clientStream.BeginRead(temp, 0, length, null, null);

            return temp;
        }

        public void WriteData(byte[] data)
        {
            clientStream.Write(data, 0, data.Length);
        }
    }
}
