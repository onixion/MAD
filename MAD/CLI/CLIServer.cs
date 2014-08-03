using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.IO;

using MAD.JobSystemCore;
using MAD.CLICore;

using MadNet;

namespace MAD.CLIServerCore
{
    public class CLIServer : CLIServerInternal
    {
        #region members

        private int[] _acceptedRSAModulusLength = new int[] { 2048, 4096, 8192 };
        private int _aesGeneratedPassLength = 20;

        private string _user = "root";
        private string _pass = MD5Hashing.GetHash("rofl");
        private bool _userOnline = false;

        private TcpListener _serverListener;

        private JobSystem _js;

        #endregion

        #region constructor

        public CLIServer(int port, JobSystem js)
        {
            serverPort = port;
            _js = js;

            InitSessionCommands();
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

        public void ChangePort(int newPort)
        {
            if (!IsListening)
                serverPort = newPort;
            else
                throw new Exception("Server running!");
        }

        protected override object GetClient()
        {
            return _serverListener.AcceptTcpClient();
        }

        protected override object HandleClient(object clientObject)
        {
            TcpClient _client = (TcpClient)clientObject;
            NetworkStream _stream;
            IPEndPoint _clientEndPoint;

            try
            {
                _stream = _client.GetStream();
                _clientEndPoint = (IPEndPoint)_client.Client.RemoteEndPoint;

                /*
                DataPacket _receivePacket = new DataPacket(_stream, new AES("GAY"));
                _receivePacket.ReceivePacket();

                string text = Encoding.Unicode.GetString(_receivePacket.data);

                Console.WriteLine("");
                */
                /*
                // LOG

                // -----------------------------------------------
                // KEY-EXCHANGE

                // 1.) Receive RSA-packet.
                RSAPacket _rsaP = new RSAPacket(_stream, null);
                _rsaP.ReceivePacket();

                // 2.) Check if modulus is valid for connection.
                if (!ValidRSAModulus(_rsaP.modulusLength))
                    throw new Exception("RSA-MODULUS-LENGTH NOT SUPPORTED!");

                // 3.) Create RSA-encryptor.
                RSAx _rsa = new RSAx(new RSAxParameters(_rsaP.modulus, _rsaP.publicKey, _rsaP.modulusLength));
                // RSA-packet not needed any more.
                _rsaP.Dispose();

                // 4.) Generate random password.
                string _aesPass = GenRandomPassUnicode(_aesGeneratedPassLength);

                // 5.) Encrypt password with RSA.
                byte[] _aesPassCrypted = _rsa.Encrypt(Encoding.Unicode.GetBytes(_aesPass), true);

                // 6.) Create DataPacket and send it to client.
                DataPacket _dataP = new DataPacket(_stream, null, _aesPassCrypted);
                _dataP.SendPacket();
                _dataP.Dispose();

                // 7.) And from here -> AES.

                // -----------------------------------------------
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
            }
            catch (Exception)
            {
                // LOG
            }

            // Client disconnected.

            // LOG

            _userOnline = false;
            _client.Close();

            return null;
        }

        private bool ValidRSAModulus(int modulus)
        {
            for (int i = 0; i < _acceptedRSAModulusLength.Length; i++)
                if (_acceptedRSAModulusLength[i] == modulus)
                    return true;
            return false;
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

        private void InitSessionCommands()
        {
            /*if (_session != null)
            {
                List<CommandOptions> commands = _session.commands;

                // general purpose
                commands.Add(new CommandOptions("exit", typeof(ExitCommand), null));
                commands.Add(new CommandOptions("help", typeof(HelpCommand), new object[] { commands }));
                commands.Add(new CommandOptions("colortest", typeof(ColorTestCommand), null));
                commands.Add(new CommandOptions("info", typeof(InfoCommand), null));

                // MAC AND IP READER
                commands.Add(new CommandOptions("mac finder start", typeof(CatchBasicInfoStartCommand), new object[] { macFeeder }));
                commands.Add(new CommandOptions("mac finder stop", typeof(CatchBasicInfoStopCommand), new object[] { macFeeder }));
                commands.Add(new CommandOptions("mac finder set time", typeof(CatchBasicInfoSetTimeIntervallCommand), new object[] { macFeeder }));
                commands.Add(new CommandOptions("mac finder print list", typeof(CatchBasicInfoPrintHostsCommand), new object[] { macFeeder }));
                // JOBSYSTEM
                commands.Add(new CommandOptions("js", typeof(JobSystemStatusCommand), new object[] { _js }));
                commands.Add(new CommandOptions("js nodes", typeof(JobSystemStatusNodesCommand), new object[] { _js }));
                commands.Add(new CommandOptions("js jobs", typeof(JobSystemStatusJobsCommand), new object[] { _js }));

                // SCEDULE
                commands.Add(new CommandOptions("scedule start", typeof(JobSceduleStartCommand), new object[] { _js }));
                commands.Add(new CommandOptions("scedule stop", typeof(JobSceduleStopCommand), new object[] { _js }));

                // NODES
                commands.Add(new CommandOptions("node add", typeof(JobSystemAddNodeCommand), new object[] { _js }));
                commands.Add(new CommandOptions("node remove", typeof(JobSystemRemoveNodeCommand), new object[] { _js }));
                commands.Add(new CommandOptions("node start", typeof(JobSystemStartNodeCommand), new object[] { _js }));
                commands.Add(new CommandOptions("node stop", typeof(JobSystemStartNodeCommand), new object[] { _js }));
                commands.Add(new CommandOptions("node save", typeof(JobSystemSaveNodeCommand), new object[] { _js }));
                commands.Add(new CommandOptions("node load", typeof(JobSystemLoadNodeCommand), new object[] { _js }));

                // JOBS
                commands.Add(new CommandOptions("job status", typeof(JobStatusCommand), new object[] { _js }));
                commands.Add(new CommandOptions("job remove", typeof(JobSystemRemoveJobCommand), new object[] { _js }));
                commands.Add(new CommandOptions("job start", typeof(JobSystemStartJobCommand), new object[] { _js }));
                commands.Add(new CommandOptions("job stop", typeof(JobSystemStopJobCommand), new object[] { _js }));

                commands.Add(new CommandOptions("add ping", typeof(JobSystemAddPingCommand), new object[] { _js }));
                commands.Add(new CommandOptions("add http", typeof(JobSystemAddHttpCommand), new object[] { _js }));
                commands.Add(new CommandOptions("add port", typeof(JobSystemAddPortCommand), new object[] { _js }));
                commands.Add(new CommandOptions("add detect", typeof(JobSystemAddHostDetectCommand), new object[] { _js }));
                commands.Add(new CommandOptions("add serviceCheck", typeof(JobSystemAddServiceCheckCommand), new object[] { _js }));

                // CLIServer (these commands cannot be used by cli!)
               
                commands.Add(new CommandOptions("cliserver", typeof(CLIServerInfo), new object[] { cliServer }));
                commands.Add(new CommandOptions("cliserver start", typeof(CLIServerStart), new object[] { cliServer }));
                commands.Add(new CommandOptions("cliserver stop", typeof(CLIServerStop), new object[] { cliServer }));
                commands.Add(new CommandOptions("cliserver changeport", typeof(CLIChangePort), new object[] { cliServer }));
            }
        */
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

        private byte[] GenerateRandomPass(int length)
        {
            byte[] _buffer = new byte[length];
            Random _rand = new Random();
            _rand.NextBytes(_buffer);
            return _buffer;
        }

        #endregion
    }
}
