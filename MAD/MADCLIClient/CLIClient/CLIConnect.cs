using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace CLIClient
{
    public class CLIConnect
    {
        private TcpClient client = new TcpClient();
        private NetworkStream clientStream;

        public IPAddress serverAddress;
        public int serverPort;
        public IPEndPoint serverEndPoint;

        private string username;
        private byte[] password;

        private byte[] securePass = new byte[2048];

        public CLIConnect(IPAddress serverAddress, int serverPort, string username, string password, string securePass)
        {
            this.serverAddress = serverAddress;
            this.serverPort = serverPort;
            this.serverEndPoint = new IPEndPoint(serverAddress, serverPort);

            this.username = username;
            this.password = GetMD5Hash(Encoding.ASCII.GetBytes(password));
            this.securePass = Encoding.ASCII.GetBytes(securePass);
        }

        public void Connect()
        {
            try
            {
                client.Connect(serverEndPoint);
                clientStream = client.GetStream();

                Authentication();
            }
            catch (Exception)
            { 
                Console.WriteLine("Could not connect to server '" + serverEndPoint.ToString() + "'.");
            }
        }

        /// <summary>
        /// Get MD5 hash.
        /// </summary>
        private byte[] GetMD5Hash(byte[] data)
        {
            MD5 alg = new MD5CryptoServiceProvider();
            return alg.ComputeHash(data, 0, data.Length);
        }

        /// <summary>
        /// Setup the connection.
        /// </summary>
        private void Authentication()
        {
            // send securePass
            BeginWriteData(securePass);
        
        }

        private byte[] ReadData(int length)
        {
            byte[] temp = new byte[length];

            clientStream.Read(temp, 0, length);

            return temp;
        }

        private void BeginWriteData(byte[] data)
        {
            //clientStream.Write(data, 0, data.Length);
            clientStream.BeginWrite(data, 0, data.Length, null, null);
        }
    }
}
