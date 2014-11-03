using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Text;
using System.IO;
using System.Xml;
using System.Security.Cryptography.X509Certificates;

using MAD.JobSystemCore;
using MAD.CLICore;
using MAD.Logging;
using MadNet;

namespace MAD.CLIServerCore
{
    public class CLIServer : Server
    {
        #region members

        private bool _debugMode;
        private bool _logMode;
        private string _serverHeader;
        private string _serverVer = "v2.0";

        private TcpListener _serverListener;
        private X509Certificate2 _certificate;
        
        private bool _userOnline = false;
        private string _user;
        private string _pass = MadNetHelper.ToMD5("rofl123");

        private JobSystem _js;

        #endregion

        #region constructor

        public CLIServer(string certfile, JobSystem js)
        {
            LoadConfig();
            LoadCertificate(certfile);

            _js = js;
        }

        private void LoadConfig()
        {
            _logMode = MadConf.conf.LOG_MODE;
            _debugMode = MadConf.conf.DEBUG_MODE;
            _serverHeader = MadConf.conf.SERVER_HEADER;
            serverPort = MadConf.conf.SERVER_PORT;
        }

        private void LoadCertificate(string certfile)
        {
            if (File.Exists(certfile))
            {
                Console.Write("Pass for certification: ");
                _certificate = new X509Certificate2(certfile, Console.ReadLine());
            }
            else
                throw new Exception("Certification-file does not exist!");
        }

        #endregion

        #region methods

        protected override bool StartListener()
        {
            try
            {
                _serverListener = new TcpListener(new IPEndPoint(IPAddress.Loopback, serverPort));
                _serverListener.Start();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected override void StopListener()
        {
            _serverListener.Stop();
        }

        protected override object GetClient()
        {
            return _serverListener.AcceptTcpClient();
        }

        protected override object HandleClient(object clientObject)
        {
            IPEndPoint _clientEndPoint = null;

            try
            {
                TcpClient _client = (TcpClient)clientObject;
                NetworkStream _stream = _client.GetStream();
                AES _aes = null;
                bool _login = false;

                if (_debugMode)
                    Console.WriteLine(GetTimeStamp() + " Client (" + _clientEndPoint.Address + ") connected.");
                if (_logMode)
                    Logger.Log("Client (" + _clientEndPoint.Address + ") connected.", Logger.MessageType.INFORM);

                while (true)
                {
                    using (DataPacket _dataP = new DataPacket(_stream, null))
                    {
                        _dataP.ReceivePacket();

                        switch (_dataP.data[0])
                        {
                            case 0: // info
                                SendServerInfo(_stream);
                                break;
                            case 1: // SSL handshake
                                _aes = MakeHandshake(_stream);
                                break;
                            case 2: // login
                                _login = Login(_stream, _aes);
                                break;
                            case 3: // start remote cli
                                if (_login)
                                {
                                    CLISession _session = new CLISession(_stream, _aes, _js);
                                    _session.InitCommands();
                                    _session.Start();
                                }
                                break;
                            case 4:
                                throw new Exception("Client disconnected!");
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (_debugMode)
                    Console.WriteLine(GetTimeStamp() + " Client (" + _clientEndPoint.Address + ") disconnected.");
                if (_logMode)
                    Logger.Log("Client (" + _clientEndPoint.Address + ") disconnected.", Logger.MessageType.INFORM);
            }

            return null;
        }

        private AES MakeHandshake(NetworkStream stream)
        {
            using (SslStream _sStream = new SslStream(stream))
            {
                _sStream.AuthenticateAsServer(_certificate);

                using (StreamReader _reader = new StreamReader(_sStream))
                    return new AES(_reader.ReadLine());
            }
        }

        private void SendServerInfo(NetworkStream stream)
        {
            using (ServerInfoPacket _serverInfoP = new ServerInfoPacket(_stream, null))
            {
                _serverInfoP.serverHeader = Encoding.Unicode.GetBytes(_serverHeader);
                _serverInfoP.serverVersion = Encoding.Unicode.GetBytes(_serverVer);
                _serverInfoP.SendPacket();
            }
        }

        private bool Login(NetworkStream stream, AES aes)
        {
            using (LoginPacket _loginP = new LoginPacket(stream, aes))
            {
                _loginP.ReceivePacket();

                if (_loginP.user.Length != 0 || _loginP.passMD5.Length != 0)
                {
                    string _userUnicode = Encoding.Unicode.GetString(_loginP.user);
                    string _passUnicode = Encoding.Unicode.GetString(_loginP.passMD5);

                    if (_userUnicode == _user && _passUnicode == _pass)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        private string GetTimeStamp()
        {
            return DateTime.Now.ToString("[dd.mm.yyyy|hh:MM.ss]");
        }

        #endregion
    }
}
