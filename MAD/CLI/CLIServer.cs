using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Text;
using System.IO;

using MAD.JobSystemCore;
using MAD.CLICore;

using MadNet;

namespace MAD.CLIServerCore
{
    public class CLIServer : Server
    {
        #region members

        public const string HEADER = "MAD CLI-Server";
        public const string VERSION = "v2.4";
        public readonly bool DEBUG_MODE;
        public readonly bool LOG_MODE;
        public readonly int ACCEPTED_RSA_MODULUS_LENGTH = 2048;
        public readonly int AES_PASS_LENGTH = 20;

        private string _user = "root";
        private string _pass = MD5Hashing.GetHash("rofl123");
        private bool _userOnline = false;

        private TcpListener _serverListener;

        private JobSystem _js;

        #endregion

        #region constructor

        public CLIServer(int port, bool debug, bool log, JobSystem js)
        {
            serverPort = port;

            DEBUG_MODE = debug;
            LOG_MODE = log;

            _js = js;
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

                if (DEBUG_MODE)
                    Console.WriteLine(GetTimeStamp() + "Client (" + _clientEndPoint.Address + ") connected.");
                if (LOG_MODE)
                    Log("Client (" + _clientEndPoint.Address + ") connected.");

                using (ServerInfoPacket _serverInfoP = new ServerInfoPacket(_stream, null))
                {
                    _serverInfoP.serverHeader = Encoding.Unicode.GetBytes(HEADER);
                    _serverInfoP.serverVersion = Encoding.Unicode.GetBytes(VERSION);
                    _serverInfoP.SendPacket();
                }

                if (_userOnline)
                    throw new Exception("User already online!");

                /*
                // AES-KEY exchange
                RSAPacket _rsaP = new RSAPacket(_stream, null);
                _rsaP.ReceivePacket();
                if (_rsaP.modulusLength != ACCEPTED_RSA_MODULUS_LENGTH)
                    throw new Exception("RSA-MODULUS-LENGTH NOT SUPPORTED!");
                RSAx _rsa = new RSAx(new RSAxParameters(_rsaP.modulus, _rsaP.publicKey, _rsaP.modulusLength));
                _rsaP.Dispose();
                string _aesPass = GenRandomPassUnicode(AES_PASS_LENGTH);
                byte[] _aesPassCrypted = _rsa.Encrypt(Encoding.Unicode.GetBytes(_aesPass), true);
                using (DataPacket _dataP = new DataPacket(_stream, null, _aesPassCrypted))
                    _dataP.SendPacket();
                AES _aes = new AES(_aesPass);    
                */

                bool _loginSuccess;
                using (LoginPacket _loginP = new LoginPacket(_stream, null))
                {
                    _loginP.ReceivePacket();
                    _loginSuccess = Login(_loginP);
                }

                using (DataPacket _dataP = new DataPacket(_stream, null))
                {
                    if (_loginSuccess)
                        _dataP.data = Encoding.Unicode.GetBytes("LOGIN_SUCCESS");
                    else
                        _dataP.data = Encoding.Unicode.GetBytes("LOGIN_DENIED");
                    _dataP.SendPacket();
                }

                if (_loginSuccess)
                {
                    CLISession _session = new CLISession(_stream, null, _js);
                    _session.InitCommands();
                    _session.Start();
                }

                _client.Close();
            }
            catch (Exception e)
            {
                if (DEBUG_MODE)
                    Console.WriteLine("EX: " + e.Message);
                if (LOG_MODE)
                    Log("EX: " + e.Message);
            }

            if (DEBUG_MODE)
                Console.WriteLine("Client (" + _clientEndPoint.Address + ") disconnected.");
            if (LOG_MODE)
                Log("Client (" + _clientEndPoint.Address + ") disconnected.");

            return null;
        }

        private string GenRandomPassUnicode(int stringLength)
        {
            Random _rand = new Random();
            byte[] _str = new byte[stringLength * 2];

            for (int i = 0; i < stringLength * 2; i += 2)
            {
                int _chr = _rand.Next(0xD7FF);
                _str[i + 1] = (byte)((_chr & 0xFF00) >> 8);
                _str[i] = (byte)(_chr & 0xFF);
            }

            return Encoding.Unicode.GetString(_str);
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

        public void ChangePort(int newPort)
        {
            if (!IsListening)
                serverPort = newPort;
            else
                throw new Exception("Server running!");
        }

        private void Log(string data)
        {
            FileStream _stream;

            if (File.Exists("server.log"))
                _stream = new FileStream("server.log", FileMode.Append, FileAccess.Write , FileShare.Read);
            else
                _stream = new FileStream("server.log", FileMode.Create, FileAccess.Write, FileShare.Read);

            using (StreamWriter _writer = new StreamWriter(_stream))
                _writer.WriteLine(GetTimeStamp() + data);

            _stream.Dispose();
        }

        private string GetTimeStamp()
        {
            return DateTime.Now.ToString("[dd.mm.yyyy|hh:MM.ss]");
        }

        #endregion
    }
}
