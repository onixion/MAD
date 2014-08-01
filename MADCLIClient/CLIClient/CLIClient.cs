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

        private TcpClient _client;
        private NetworkStream _stream;

        #endregion

        #region constructor

        public CLIClient(IPEndPoint serverEndPoint)
        {
            _serverEndPoint = serverEndPoint;
        }

        #endregion

        #region methodes

        public void Connect()
        {
            _client = new TcpClient();

            try
            {
                _client.Connect(_serverEndPoint);
                _stream = _client.GetStream();
            }
            catch (Exception)
            {
                throw new Exception("Could not connect to server.");
            }
        }

        public void StartRemoteCLI()
        {
            try
            {
                // Establish connection.
                RSAxParameters _par = RSAxUtils.GetRSAxParameters("RANDOM", 1048);
                RSAx _rsa = new RSAx(_par);

                RSAPacket _rsaPacket = new RSAPacket(_stream, null, Encoding.Unicode.GetBytes(_par.E), _par.N);

            }
            catch (Exception e)
            {
                throw new Exception("Lost connection to server.", e);
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
