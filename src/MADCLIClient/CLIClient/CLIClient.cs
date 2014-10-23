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

        private RSA _RSA;
        
        private string _username;
        private string _passwordMD5;

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
            _client = new TcpClient();

            try
            {
                _client.Connect(_serverEndPoint);
                _stream = _client.GetStream();

                LoginToServer();
                _client.Close();
            }
            catch (Exception e)
            {
                throw new Exception("Could not connect to server. E:" + e.Message);
            }
        }

        private void LoginToServer()
        {
            try
            {
                using (ServerInfoPacket _serverInfoP = new ServerInfoPacket(_stream, null))
                {
                    _serverInfoP.ReceivePacket();

                    CLIOutput.WriteToConsole("<color><yellow>" + ("".PadLeft(Console.BufferWidth, '_')));
                    CLIOutput.WriteToConsole("<color><yellow>SERVER-HEADER:      <color><white>" + Encoding.Unicode.GetString(_serverInfoP.serverHeader));
                    CLIOutput.WriteToConsole("<color><yellow>SERVER-VERSION:     <color><white>" + Encoding.Unicode.GetString(_serverInfoP.serverVersion) + "\n");
                }

                SslStream _sStream = new SslStream(_stream);
                _sStream.AuthenticateAsClient("test");
                X509Certificate2 _cert = (X509Certificate2) _sStream.RemoteCertificate;

                CLIOutput.WriteToConsole("<color><yellow>-- SERVER-CERTIFICATE BEGIN -- \n");
                CLIOutput.WriteToConsole("<color><yellow>THUMB-PRINT: <color><white>" + _cert.Thumbprint + "\n");
                CLIOutput.WriteToConsole("<color><yellow>PUBLIC-KEY:  <color><white>" + _cert.GetPublicKeyString() + "\n");
                CLIOutput.WriteToConsole("<color><yellow>-- SERVER-CERTIFICATE END -- \n");

                ConsoleKeyInfo _key;
                while (true)
                {
                    CLIOutput.WriteToConsole("<color><red>Are you sure you want to connect to this server? Y(es) / N(o)");
                    _key = Console.ReadKey(true);
                    if (_key.Key == ConsoleKey.Y)
                        break;
                    else if (_key.Key == ConsoleKey.N)
                        throw new Exception();
                }

                string _aesPass = "THIS WILL BE RANDOM";
                using (SSLPacket _sslP = new SSLPacket(_sStream))
                {
                    _sslP.data = Encoding.Unicode.GetBytes(_aesPass);
                    _sslP.SendPacket();
                }

                // set aes
                AES _aes = new AES(_aesPass);

                // send login-packet
                using (LoginPacket _loginP = new LoginPacket(_stream, _aes))
                {
                    _loginP.user = Encoding.Unicode.GetBytes(_username);
                    _loginP.passMD5 = Encoding.Unicode.GetBytes(_passwordMD5);

                    _loginP.SendPacket();
                }

                string _serverAnswer;
                using (DataPacket _dataP2 = new DataPacket(_stream, _aes))
                {
                    _dataP2.ReceivePacket();
                    _serverAnswer = Encoding.Unicode.GetString(_dataP2.data);
                }

                CLIOutput.WriteToConsole("<color><yellow>SERVER-REPLY: <color><white>" + _serverAnswer);

                if (_serverAnswer == "LOGIN_SUCCESS")
                {
                    CLIOutput.WriteToConsole("<color><green>Server accepted login-data.");
                    StartRemoteConsole(_stream, _aes);
                }
                else
                {
                    CLIOutput.WriteToConsole("<color><red>Server refused login-data.");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Lost connection to server.", e);
            }
        }

        private void StartRemoteConsole(NetworkStream stream, AES aes)
        {
            string _cliInput;
            string _serverResponse;
            DataPacket _dataP = new DataPacket(stream, aes);
            CLIPacket _cliP = new CLIPacket(stream, aes);
            string _cursor = "MAD-CLIENT> ";

            _dataP.ReceivePacket();
            CLIOutput.WriteToConsole(Encoding.Unicode.GetString(_dataP.data));
            _dataP.data = null;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(_cursor);
                Console.ForegroundColor = ConsoleColor.White;

                _cliInput = CLIInput.ReadInput(_cursor.Length);

                if (_cliInput != "")
                {
                    _cliP.consoleWidth = Console.BufferWidth;
                    _cliP.cliInput = Encoding.Unicode.GetBytes(_cliInput);
                    _cliP.SendPacket();

                    _dataP.ReceivePacket();
                    _serverResponse = Encoding.Unicode.GetString(_dataP.data);

                    if (_serverResponse == "EXIT_CLI")
                        break;

                    CLIOutput.WriteToConsole(_serverResponse);
                }
            }

            _dataP.Dispose();
            _cliP.Dispose();
        }

        #endregion
    }
}
