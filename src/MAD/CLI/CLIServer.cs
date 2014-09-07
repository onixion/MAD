﻿using System;
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

                RSAEncryption _rsa = new RSAEncryption();

                using (DataStringPacket _dataP = new DataStringPacket(_stream, null))
                {
                    _dataP.ReceivePacket();
                    _rsa.LoadPublicFromXml(_dataP.data);
                }
                string _pass = GenRandomPassUnicode(AES_PASS_LENGTH);

                byte[] _encrypted = _rsa.PublicEncryption(Encoding.UTF8.GetBytes(_pass));

                using (DataPacket _dataP = new DataPacket(_stream, null, _encrypted))
                        _dataP.SendPacket();
               
                AES _aes = new AES(_pass);

                bool _loginSuccess;
                using (LoginPacket _loginP = new LoginPacket(_stream, _aes))
                {
                    _loginP.ReceivePacket();
                    _loginSuccess = Login(_loginP);
                }

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
                if (DEBUG_MODE)
                    Console.WriteLine(GetTimeStamp() + " EX: " + e.Message);
                if (LOG_MODE)
                    Log(GetTimeStamp() + " EX: " + e.Message);
            }

            if (DEBUG_MODE)
                Console.WriteLine(GetTimeStamp() + " Client (" + _clientEndPoint.Address + ") disconnected.");
            if (LOG_MODE)
                Log(GetTimeStamp() + " Client (" + _clientEndPoint.Address + ") disconnected.");

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
                _writer.WriteLine(data);

            _stream.Dispose();
        }

        private string GetTimeStamp()
        {
            return DateTime.Now.ToString("[dd.mm.yyyy|hh:MM.ss]");
        }

        #endregion
    }
}
