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
        #region member

        private IPEndPoint _serverEndPoint;
        private string _username;
        private string _passwordMD5;

        private string cliInput;

        #endregion

        public CLIClient(IPEndPoint serverEndPoint, string username, string passwordMD5)
        {
            _serverEndPoint = serverEndPoint;
            _username = username;
            _passwordMD5 = passwordMD5;
        }

        #region methodes

        public void Start()
        {
            TcpClient _client = new TcpClient();
            NetworkStream _stream;

            try
            {
                Console.WriteLine("Connecting ...");
                _client.Connect(_serverEndPoint);

                _stream = _client.GetStream();

                CLIConnection(_stream);
                _stream.Close();

                Console.WriteLine("Disconnected.");
            }
            catch (Exception)
            {
                Console.WriteLine("Lost connection to server.");
            }

            _client.Close();
        }

        private void CLIConnection(NetworkStream _stream)
        {
            Console.WriteLine("SERVER-INFO: " + NetCommunication.ReceiveString(_stream));
            NetCommunication.SendString(_stream, _username + "<seperator>" + _passwordMD5, true);

            switch (NetCommunication.ReceiveString(_stream))
            { 
                case "ACCESS GRANTED":
                    RemoteConsole(_stream);
                    break;
                case "ACCESS DENIED":
                    Console.WriteLine("Server denied access!");
                    break;
                default:
                    Console.WriteLine("Server-Response-Error!");
                    break;
            }
        }

        private void RemoteConsole(NetworkStream stream)
        {
            Console.Write(NetCommunication.ReceiveString(stream));

            while (true)
            {
                cliInput = Console.ReadLine();

                NetCommunication.SendString(stream, cliInput, true);

                ConsoleWriter.WriteToConsole(NetCommunication.ReceiveString(stream));
            }
        }

        #endregion
    }
}
