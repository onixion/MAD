using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Numerics;

using System.Security.Cryptography;
using System.Security.Cryptography.Xml;

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
                    RSAxParameters _par = RSAxUtils.GetRSAxParameters("GAYGAYGAY", _RSAModulusLength);
                    RSAPacket _rsaP = new RSAPacket(_stream, null, _par.E.ToByteArray(), _par.N.ToByteArray(), _RSAModulusLength);
                    _rsaP.SendPacket();
                    _rsaP.Dispose();
                    RSAx _rsa = new RSAx(_par);
                    DataPacket _dataP = new DataPacket(_stream, null);
                    _dataP.ReceivePacket();
                    _aesPassFromServer = Encoding.Unicode.GetString(_rsa.Decrypt(_dataP.data, true));
                    AES _aes = new AES(_aesPassFromServer);
                    */

                    byte[] _username = Encoding.Unicode.GetBytes(username);
                    byte[] _passwordMD5 = Encoding.Unicode.GetBytes(passwordMD5);

                    using (LoginPacket _loginP = new LoginPacket(_stream, null, _username, _passwordMD5))
                        _loginP.SendPacket();

                    string _serverAnswer;
                    using(DataPacket _dataP = new DataPacket(_stream, null))
                    {
                        _dataP.ReceivePacket();
                        _serverAnswer = Encoding.Unicode.GetString(_dataP.data);
                    }
                    Console.WriteLine("Server: " + _serverAnswer);

                    if (_serverAnswer == "LOGIN_SUCCESS")
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Server accepted login-data.");
                        Console.ForegroundColor = ConsoleColor.White;

                        StartVirtualConsole(_stream, null);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Server refused login-data.");
                        Console.ForegroundColor = ConsoleColor.White;
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

        private void StartVirtualConsole(NetworkStream stream, AES aes)
        {
            string _cliInput;
            string _serverResponse;
            DataPacket _dataP = new DataPacket(stream, aes);
            CLIPacket _cliP = new CLIPacket(stream, aes);

            while (true)
            {
                CLIInput.WriteCursor();
                _cliInput = CLIInput.ReadInput();

                _cliP.consoleWidth = Int32.Parse(Console.BufferWidth.ToString());
                _cliP.cliInput = Encoding.Unicode.GetBytes(_cliInput);
                _cliP.SendPacket();

                _dataP.ReceivePacket();
                _serverResponse = Encoding.Unicode.GetString(_dataP.data);

                if (_serverResponse == "EXIT_CLI")
                    break;

                CLIOutput.WriteToConsole(_serverResponse);
            }

            _dataP.Dispose();
            _cliP.Dispose();
        }

        #endregion
    }
}
