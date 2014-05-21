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
        private string _securePass;
        private string _username;
        private string _passwordMD5;

        public CLIClient(IPEndPoint serverEndPoint, string securePass, string username, string passwordMD5)
        {
            _serverEndPoint = serverEndPoint;
            _securePass = securePass;
            _username = username;
            _passwordMD5 = passwordMD5;
        }

        public void Start()
        {
            TcpClient _client = new TcpClient();
            NetworkStream _stream = null;

            try
            {
                _client.Connect(_serverEndPoint);
                _stream = _client.GetStream();

                Send(_stream, "TESTTESTTEST");

                Console.WriteLine();

                _stream.Close();
            }
            catch (Exception e)
            {
                if (_stream != null)
                    _stream.Close();

                Console.WriteLine("Es konnte keine Verbindung zum Server hergestellt werden.");
            }

            _client.Close();
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
