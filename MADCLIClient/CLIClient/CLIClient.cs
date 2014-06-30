using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

using nc;

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

        #region constructor

        public CLIClient(IPEndPoint serverEndPoint, string username, string passwordMD5)
        {
            _serverEndPoint = serverEndPoint;
            _username = username;
            _passwordMD5 = passwordMD5;
        }

        #endregion

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
            Console.WriteLine("SERVER-INFO: " + NetCom.ReceiveString(_stream));
            NetCom.SendString(_stream, _username + "<seperator>" + _passwordMD5, true);

            switch (NetCom.ReceiveString(_stream))
            { 
                case "ACCESS GRANTED":
                    StartVirtualConsole(_stream);
                    break;
                case "ACCESS DENIED":
                    Console.WriteLine("Server denied access!");
                    break;
                default:
                    Console.WriteLine("Server-Response-Error!");
                    break;
            }
        }

        private void StartVirtualConsole(NetworkStream stream)
        {
            string _serverResponse;

            Console.Write(NetCom.ReceiveString(stream));

            while (true)
            {
                // TODO: own CLI-Read method
                cliInput = Console.ReadLine();

                NetCom.SendString(stream, cliInput, true);

                _serverResponse = NetCom.ReceiveString(stream);

                if (_serverResponse == "EXIT_CLI")
                {
                    break;
                }

                ConsoleIO.WriteToConsole(_serverResponse);
            }
        }

        #endregion
    }
}
