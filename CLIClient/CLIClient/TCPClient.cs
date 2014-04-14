using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace CLIClient
{
    public abstract class TCPClient
    {
        private TcpClient client;

        public IPAddress serverAddress;
        public int serverPort;
        public IPEndPoint serverEndPoint;

        private string user;
        private string pass;

        public void Connect(string user, string pass)
        {
            client = new TcpClient();

            try
            {
                client.Connect(serverEndPoint);
            }
            catch (Exception)
            { 
                Console.WriteLine("Could not connect to server '" + serverEndPoint.ToString() + "'.");
            }
        }
    }
}
