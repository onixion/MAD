using System;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Security.Cryptography;


namespace MAD.CLI.Server
{
    public class CLIPackage
    {
        /*
        public string version;
        public int packageID;
        public string data;
        */

        public byte[] version = Encoding.ASCII.GetBytes("1.0A");

        private static int packageCount = 0;
        public byte[] packageID = new byte[8];

        public byte[] data;

        public CLIPackage()
        {
            packageID = BitConverter.GetBytes(packageCount);
            packageCount++;

            data = new byte[0];
        }

        public CLIPackage(string data)
        {
            this.data = Encoding.ASCII.GetBytes(data);
        }

        public void Send(NetworkStream stream, bool flush)
        {
            stream.Write(version, 0, version.Length);
            stream.Write(packageID, 0, packageID.Length);
            stream.Write(data, 0, data.Length);

            if (flush)
            {
                stream.Flush();
            }
        }

        public string Receive(NetworkStream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                return reader.ReadString();
            }
        }
    }
}
