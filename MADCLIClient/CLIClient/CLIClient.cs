using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

using MadNet;

namespace CLIClient
{
    public class CLIClient
    {
        #region member

        private IPEndPoint _serverEndPoint;
        private string _username;
        private string _passwordMD5;

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
            DataPacket pak = new DataPacket(_stream, new AES("LOL"));
            pak.SendPacket();










            /*RSAxParameters _par = RSAxUtils.GetRSAxParameters("RANDOM_KEY", 1048);
            NetCom.SendByte(_stream, _par.E , true); // public-key
            NetCom.SendByte(_stream, _par.N, true); // modulus

            NetCom.SendStringAESUnicode(_stream, _username + "<>" + _passwordMD5, "RANDOM_KEY", true);
            */
            switch (NetCom.ReceiveStringUnicode(_stream))
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
            string cliInput;
            string _serverResponse;

            // Receive first cursor.
            Console.Write(NetCom.ReceiveStringUnicode(stream));

            while (true)
            {
                // TODO: own CLI-Read method
                cliInput = Console.ReadLine();

                NetCom.SendStringUnicode(stream, Convert.ToString(Console.BufferWidth), true);
                NetCom.SendStringUnicode(stream, cliInput, true);

                _serverResponse = NetCom.ReceiveStringUnicode(stream);

                if (_serverResponse.Contains("EXIT_CLI"))
                {
                    break;
                }

                ConsoleIO.WriteToConsole(_serverResponse);
            }
        }

        #endregion
    }
}
