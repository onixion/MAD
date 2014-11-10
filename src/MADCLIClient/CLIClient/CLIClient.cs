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

        public bool connected = false;
        public bool login = false;

        private IPEndPoint _serverEndPoint;
        private TcpClient _client;

        private NetworkStream _stream;
        private AES _aes = null;

        public string serverHeader = "";
        public string serverVersion = "";

        public X509Certificate2 cert = null;

        private string _username;
        private string _passwordMD5;

        public string cursor = "MAD-CLIENT>";
        private string _cliInput;

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

        public void Connect()
        {
            try
            {
                _client = new TcpClient();
                _client.Connect(_serverEndPoint);
                _stream = _client.GetStream();

                connected = true;
            }
            catch (Exception e)
            {
                throw new Exception("Could not connect to server. E:" + e.Message);
            }
        }

        public void GetServerInfo()
        {
            if (connected)
            {
                using (DataPacket _dataP = new DataPacket(_stream, null, new byte[] { 0 }))
                    _dataP.SendPacket();

                using (ServerInfoPacket _serverInfoP = new ServerInfoPacket(_stream, null))
                {
                    _serverInfoP.ReceivePacket();

                    serverHeader = Encoding.Unicode.GetString(_serverInfoP.serverHeader);
                    serverVersion = Encoding.Unicode.GetString(_serverInfoP.serverVersion);
                }
            }
            else
                throw new Exception("Not connected!");
            
        }

        public AES MakeHandshake()
        {
            if (connected)
            {
                using (DataPacket _dataP = new DataPacket(_stream, null, new byte[] { 1 }))
                    _dataP.SendPacket();

                using (SslStream _sStream = new SslStream(_stream, false, new RemoteCertificateValidationCallback(CheckSSLCertification), null))
                {
                    _sStream.AuthenticateAsClient("MAD");
                    cert = (X509Certificate2)_sStream.RemoteCertificate;

                    _aes = new AES("RANDOM");
                    using (StreamWriter _writer = new StreamWriter(_sStream))
                        _writer.WriteLine("RANDOM");
                }

                return _aes;
            }
            else
                throw new Exception("Not connected!");
        }

        private bool CheckSSLCertification(object o, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }

        public void Login()
        {
            if (connected)
            {
                if(_aes == null)
                    throw new Exception("No AES!");

                using (DataPacket _dataP = new DataPacket(_stream, null, new byte[] { 2 }))
                    _dataP.SendPacket();

                using (LoginPacket _loginP = new LoginPacket(_stream, _aes))
                {
                    _loginP.user = Encoding.Unicode.GetBytes(_username);
                    _loginP.passMD5 = Encoding.Unicode.GetBytes(_passwordMD5);
                    _loginP.SendPacket();
                }

                using (DataPacket _dataP = new DataPacket(_stream, _aes))
                {
                    _dataP.ReceivePacket();
                    if (_dataP.data[0] == 0)
                        login = true;
                    else
                        throw new Exception("Login rejected!");
                }
            }
            else
                throw new Exception("Not connected!");
        }

        public void StartRemoteCLI()
        {
            if (connected)
            {
                if (_aes != null)
                {
                    if (login)
                    {
                        using (DataPacket _dataP = new DataPacket(_stream, null, new byte[] { 3 }))
                            _dataP.SendPacket();

                        using (CLIPacket _cliP = new CLIPacket(_stream, _aes))
                        {
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

                                    CLIOutput.WriteToConsole(_cliP.serverAnswer);
                                }
                            }
                        }
                    }
                    else
                        throw new Exception("Not logged in!");
                }
                else
                    throw new Exception("No AES!");
            }
            else
                throw new Exception("Not connected!");
        }

        #endregion
    }
}
