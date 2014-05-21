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
                _stream = _client.GetStream();

                Console.WriteLine("Connected to server.");

                CLIConnection(_stream);

                _stream.Close();
            }
            catch (Exception)
            {
                if (_stream != null)
                    _stream.Close();

                Console.WriteLine("Could not connect to server!");
            }

            _client.Close();
        }

        private void CLIConnection(NetworkStream _stream)
        { 
            /* From here the connection is astablished */
        
        
        
        
        }

        private void Send(NetworkStream _stream, string _data)
        {
            using (BinaryWriter _writer = new BinaryWriter(_stream))
            {
                _writer.Write(_data);
            }
        }

        private string Receive(NetworkStream _stream)
        {
            using (BinaryReader _reader = new BinaryReader(_stream))
            {
                return _reader.ReadString();
            }
        }
    }
}
