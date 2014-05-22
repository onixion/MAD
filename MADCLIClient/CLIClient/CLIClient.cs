using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

using System.Security.Cryptography;

namespace CLIClient
{
    public class CLIClient
    {
        private IPEndPoint _serverEndPoint;
        private string _username;
        private string _passwordMD5;

        public CLIClient(IPEndPoint serverEndPoint, string username, string passwordMD5)
        {
            _serverEndPoint = serverEndPoint;
            _username = username;
            _passwordMD5 = passwordMD5;
        }

        public void Start()
        {
            TcpClient _client = new TcpClient();
            NetworkStream _stream = null;

            try
            {
                Console.WriteLine("Connecting to server ...");

                _client.Connect(_serverEndPoint);

                Console.WriteLine("Connected to server.");

                CLIConnection(_client.GetStream());

                _stream.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("Could not connect to server!");
            }

            _client.Close();
        }

        private void CLIConnection(NetworkStream _stream)
        { 
            /* From here the connection is astablished */

        }

        #region Send/Recieve

        private void Send(NetworkStream _stream, string _data)
        {

        }

        private string Receive(NetworkStream _stream)
        {

        }

        #endregion
    }
}
