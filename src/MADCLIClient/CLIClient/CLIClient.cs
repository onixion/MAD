using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Xml;

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
                    using (ServerInfoPacket _serverInfoP = new ServerInfoPacket(_stream, null))
                    {
                        _serverInfoP.ReceivePacket();

                        Console.WriteLine("".PadLeft(Console.BufferWidth, '_'));
                        Console.WriteLine("SERVER-HEADER:      " + Encoding.Unicode.GetString(_serverInfoP.serverHeader));
                        Console.WriteLine("SERVER-VERSION:     " + Encoding.Unicode.GetString(_serverInfoP.serverVersion));
                        Console.WriteLine("SERVER-FINGERPRING: " + Encoding.ASCII.GetString(_serverInfoP.fingerprint));
                        Console.WriteLine("".PadLeft(Console.BufferWidth, '_'));
                    }

                    ConsoleKeyInfo _key;
                    while (true)
                    {
                        Console.WriteLine("Are you sure that you want to connect to this server? Y(es) / N(o)");

                        _key = Console.ReadKey(true);
                        if (_key.Key == ConsoleKey.Y)
                            break;
                        else if (_key.Key == ConsoleKey.N)
                            throw new Exception();
                    }

                    string _xml;
                    using (DataPacket _rsaP = new DataPacket(_stream, null))
                    {
                        _rsaP.ReceivePacket();
                        _xml = Encoding.UTF8.GetString(_rsaP.data);
                    }

                    RSAEncryption _r = new RSAEncryption();
                    _r.LoadPublicFromXml(_xml);

                    string _randomPASS = "TESTLOLWAS";

                    using (DataPacket _sData = new DataPacket(_stream, null))
                    {
                        _sData.data = _r.PublicEncryption(Encoding.UTF8.GetBytes(_randomPASS));
                        _sData.SendPacket();
                    }

                    AES _aes = new AES(_randomPASS);

                    byte[] _username = Encoding.Unicode.GetBytes(username);
                    byte[] _passwordMD5 = Encoding.Unicode.GetBytes(passwordMD5);

                    using (LoginPacket _loginP = new LoginPacket(_stream, _aes, _username, _passwordMD5))
                        _loginP.SendPacket();

                    string _serverAnswer;
                    using(DataPacket _dataP2 = new DataPacket(_stream, _aes))
                    {
                        _dataP2.ReceivePacket();
                        _serverAnswer = Encoding.Unicode.GetString(_dataP2.data);
                    }
  
                    Console.WriteLine("SERVER-REPLY: " + _serverAnswer);

                    if (_serverAnswer == "LOGIN_SUCCESS")
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nServer accepted login-data.");
                        Console.ForegroundColor = ConsoleColor.White;

                        StartRemoteConsole(_stream, _aes);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nServer refused login-data.");
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

        private void StartRemoteConsole(NetworkStream stream, AES aes)
        {
            string _cliInput;
            string _serverResponse;
            DataPacket _dataP = new DataPacket(stream, aes);
            CLIPacket _cliP = new CLIPacket(stream, aes);
            string _cursor = "MAD-CLIENT> ";

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(_cursor);
                Console.ForegroundColor = ConsoleColor.White;

                _cliInput = CLIInput.ReadInput(_cursor.Length);

                _cliP.consoleWidth = Console.BufferWidth;
                _cliP.cliInput = Encoding.Unicode.GetBytes(_cliInput);
                _cliP.SendPacket();

                _dataP.ReceivePacket();
                _serverResponse = Encoding.Unicode.GetString(_dataP.data);

                if (_serverResponse == "CLI_EXIT")
                    break;

                CLIOutput.WriteToConsole(_serverResponse);
            }

            _dataP.Dispose();
            _cliP.Dispose();
        }

        #endregion
    }
}
