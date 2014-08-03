﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Numerics;
using System.Xml;

using System.Security.Cryptography;
using System.Security.Cryptography.Xml;

using MadNet;

namespace CLIClient
{
    public class CLIClient
    {
        #region member
        
        private IPEndPoint _serverEndPoint;
        
        private TcpClient _client;
        private NetworkStream _stream;

        private int _RSAModulusLength = 2048;
        private string _aesPassFromServer { get; set; }

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

        public void LoginToRemoteCLI(string username, string passwordMD5)
        {
            if (_client.Connected)
            {
                try
                {
                    /*
                    DataPacket _sendPacket = new DataPacket(_stream, null, Encoding.Unicode.GetBytes("GAYLORD"));
                    _sendPacket.SendPacket();
                    Console.WriteLine("");
                    */
                    /*
                    // -----------------------------------------------
                    // KEY-EXCHANGE

                    // 1.) Generate public-key and private-key
                    RSAxParameters _par = RSAxUtils.GetRSAxParameters("GAYGAYGAY", _RSAModulusLength);

                    // 2.) Send RSA-packet to server.
                    RSAPacket _rsaP = new RSAPacket(_stream, null, _par.E.ToByteArray(), _par.N.ToByteArray(), _RSAModulusLength);
                    _rsaP.SendPacket();
                    _rsaP.Dispose();

                    // 3.) Create RSA-decryptor.
                    RSAx _rsa = new RSAx(_par);

                    // 3.) Receive encrypted AES-pass, which was generate by the server.
                    DataPacket _dataP = new DataPacket(_stream, null);
                    _dataP.ReceivePacket();

                    // 4.) Encrypt AES-pass.
                    _aesPassFromServer = Encoding.Unicode.GetString(_rsa.Decrypt(_dataP.data, true));

                    // 5.) From here -> AES.

                    // -----------------------------------------------

                    LoginPacket _loginP = new LoginPacket(_stream, null);
                    _loginP.user = Encoding.Unicode.GetBytes(username);
                    _loginP.passMD5 = Encoding.Unicode.GetBytes(passwordMD5);
                    _loginP.SendPacket();
                    _loginP.Dispose();

                    */
                    
                    byte[] _username = Encoding.Unicode.GetBytes(username);
                    byte[] _passwordMD5 = Encoding.Unicode.GetBytes(passwordMD5);

                    // Send login-data.
                    using (LoginPacket _loginP = new LoginPacket(_stream, null, _username, _passwordMD5))
                        _loginP.SendPacket();

                    // Receive answer from server.
                    DataPacket _dataP = new DataPacket(_stream, null);
                    _dataP.ReceivePacket();

                    string _serverAnswer = Encoding.Unicode.GetString(_dataP.data);
                    Console.WriteLine("Server: " + _serverAnswer);

                    if (_serverAnswer == "LOGIN_SUCCESS")
                    {
                    

                    }
                    else
                    {
                        Console.WriteLine("Server refused login-data.");
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Lost connection to server.", e);
                }
            }
            else
            {
                throw new Exception("Not connected to server!");
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
