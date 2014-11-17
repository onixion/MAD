using System;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.IO;

using MadNet;
using CLIIO;

namespace CLIClient
{
    public class CLIClient
    {
        #region member

        private IPEndPoint _serverEndPoint;
        private TcpClient _client;

        private NetworkStream _stream;
        private AES _aes;

        public string serverHeader = "";
        public string serverVersion = "";

        public string cursor = "MAD-CLIENT> ";
        private string _cliInput;

        #endregion

        #region constructor

        public CLIClient(IPEndPoint serverEndPoint, string aesPass)
        {
            _serverEndPoint = serverEndPoint;
            _aes = new AES(aesPass);
        }

        #endregion

        #region methodes

        public void Connect()
        {
            try
            {
                _client = new TcpClient();
                _client.Connect(_serverEndPoint);
                _stream = _client.GetStream();
            }
            catch (Exception e)
            {
                throw new Exception("Could not connect to server. E:" + e.Message);
            }
        }

        public void GetServerInfo()
        {
            if (_client.Connected)
            {
                using (ServerInfoPacket _serverInfoP = new ServerInfoPacket(_stream, _aes))
                {
                    _serverInfoP.ReceivePacket();

                    serverHeader = Encoding.Unicode.GetString(_serverInfoP.serverHeader);
                    serverVersion = Encoding.Unicode.GetString(_serverInfoP.serverVersion);
                }
            }
            else
                throw new Exception("Not connected!");
        }

        public void StartRemoteCLI()
        {
            if (_client.Connected)
            {
                using (CLIPacket _cliP = new CLIPacket(_stream, _aes))
                {
                    _cliP.serverAnswer = "";

                    while (true)
                    {
                        // write cursor
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(cursor);
                        Console.ForegroundColor = ConsoleColor.White;

                        // read input
                        _cliInput = CLIInput.ReadInput(cursor.Length);

                        if (_cliInput != "")
                        {
                            _cliP.consoleWidth = Console.BufferWidth;
                            _cliP.cliInput = _cliInput;
                            _cliP.SendPacket();

                            _cliP.ReceivePacket();

                            if (_cliP.serverAnswer == "EXIT_CLI")
                                break;

                            CLIOutput.WriteToConsole(_cliP.serverAnswer + "\n");
                        }
                    }
                }
            }
            else
                throw new Exception("Not connected!");
        }

        #endregion
    }
}
