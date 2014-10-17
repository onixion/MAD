using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Xml;

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
        private RSA _RSA = new RSA();
        
        private bool _userOnline = false;
        private string _user = "root";
        private string _pass = MadNetHelper.ToMD5("rofl123");
        
        private JobSystem _js;

        #endregion

        #region constructor

        public CLIServer(JobSystem js)
        {
            LoadConfig();
            _js = js;
        }

        // This method loads the current config.
        private void LoadConfig()
        {
            lock (MadConf.confLock)
            {
                _logMode = MadConf.conf.LOG_MODE;
                _debugMode = MadConf.conf.DEBUG_MODE;
                _serverHeader = MadConf.conf.SERVER_HEADER;
                serverPort = MadConf.conf.SERVER_PORT;

                try { _RSA.LoadKeysXML(MadConf.conf.SERVER_RSA_KEYS); }
                catch (Exception)
                { }
            }
        }

        #endregion

        #region methodes

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
                _clientEndPoint = (IPEndPoint)_client.Client.RemoteEndPoint;

                if (_debugMode)
                    Console.WriteLine(GetTimeStamp() + " Client (" + _clientEndPoint.Address + ") connected.");
                if (_logMode)
                    Logger.Log("Client (" + _clientEndPoint.Address + ") connected.", Logger.MessageType.INFORM);

                if (_userOnline)
                    throw new Exception("User already online!");

                // send server info
                using (ServerInfoPacket _serverInfoP = new ServerInfoPacket(_stream, null))
                {
                    _serverInfoP.serverHeader = Encoding.Unicode.GetBytes(_serverHeader);
                    _serverInfoP.serverVersion = Encoding.Unicode.GetBytes(_serverVer);
                    _serverInfoP.SendPacket();
                }

                // get public key
                byte[] _modulus = null;
                byte[] _exponent = null;
                _RSA.GetPublicKey(out _modulus, out _exponent);

                // send modulus
                using (DataPacket _p = new DataPacket(_stream, null, _modulus))
                    _p.SendPacket();

                // send exponent
                using (DataPacket _p = new DataPacket(_stream, null, _exponent))
                    _p.SendPacket();

                // receive aes pass from client
                byte[] _aesEncrypted = null;
                using (DataPacket _dataP = new DataPacket(_stream, null))
                {
                    _dataP.ReceivePacket();
                    _aesEncrypted = _dataP.data;
                }

                // encrypt it an set aes
                byte[] _aesDecrypted = _RSA.DecryptPrivate(_aesEncrypted);
                AES _aes = new AES(Encoding.UTF8.GetString(_aesDecrypted));

                // receive login packet
                bool _loginSuccess;
                using (LoginPacket _loginP = new LoginPacket(_stream, _aes))
                {
                    _loginP.ReceivePacket();
                    _loginSuccess = Login(_loginP);
                }

                // send result
                using (DataPacket _dataP = new DataPacket(_stream, _aes))
                {
                    if (_loginSuccess)
                        _dataP.data = Encoding.Unicode.GetBytes("LOGIN_SUCCESS");
                    else
                        _dataP.data = Encoding.Unicode.GetBytes("LOGIN_DENIED");
                    _dataP.SendPacket();
                }

                if (_loginSuccess)
                {
                    CLISession _session = new CLISession(_stream, _aes, _js);
                    _session.InitCommands();
                    _session.Start();
                }

                _client.Close();
            }
            catch (Exception e)
            {
                if (_debugMode)
                    Console.WriteLine(GetTimeStamp() + " CLISERVER: " + e.Message);
                if (_logMode)
                    Logger.Log("CLISERVER: " + e.Message, Logger.MessageType.INFORM);
            }

            if (_debugMode)
                Console.WriteLine(GetTimeStamp() + "CLISERVER: Client (" + _clientEndPoint.Address + ") disconnected.");
            if (_logMode)
                Logger.Log("CLISERVER: Client (" + _clientEndPoint.Address + ") disconnected.", Logger.MessageType.INFORM);

            return null;
        }

        private bool Login(LoginPacket loginP)
        {
            if (loginP.user.Length != 0 || loginP.passMD5.Length != 0)
            {
                string _userUnicode = Encoding.Unicode.GetString(loginP.user);
                string _passUnicode = Encoding.Unicode.GetString(loginP.passMD5);

                if (_userUnicode == _user && _passUnicode == _pass)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        private string GetTimeStamp()
        {
            return DateTime.Now.ToString("[dd.mm.yyyy|hh:MM.ss]");
        }

        #endregion
    }
}
