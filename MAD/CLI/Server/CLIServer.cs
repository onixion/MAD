using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

using MAD.NetIO;
using MAD.JobSystemCore;
using MAD.CLICore;

namespace MAD.CLIServerCore
{
    public class CLIServer : CLIServerInternal
    {
        #region members

        private static bool _userOnline = false;

        private TcpListener _serverListener;
        private CLISession _session;

        private JobSystem _js;

        #endregion

        #region constructor

        public CLIServer(int port, JobSystem js)
        {
            serverPort = port;
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

            try
            {
                IPEndPoint _clientEndpoint = (IPEndPoint)_client.Client.RemoteEndPoint;
                NetworkStream _clientStream = _client.GetStream();

                // LOG

                if (_userOnline)
                {
                    return null;
                }

                // First make a diffi-hellman key exchange (working on this)

                // Receive the login-data.
                string loginData = NetCom.ReceiveStringUnicode(_clientStream);

                // Check the login-data.
                if (!Login(loginData))
                {
                    NetCom.SendStringUnicode(_clientStream, "ACCESS DENIED", true);
                    return null;
                }
                else
                {
                    NetCom.SendStringUnicode(_clientStream, "ACCESS GRANTED", true);
                    _userOnline = true;
                }

                // Init CLISession for client and start it.
                _session = new CLISession(_client);
                _session.Start();
            }
            catch (Exception)
            {
                // Client lost connection or disconnected.

                // LOG
            }

            // Client disconnected.

            // LOG

            _userOnline = false;
            _client.Close();

            return null;
        }

        private void InitSessionCommands()
        {
            if (_session != null)
            {
                List<CommandOptions> commands = _session.commands;

                // general purpose
                commands.Add(new CommandOptions("exit", typeof(ExitCommand), null));
                commands.Add(new CommandOptions("help", typeof(HelpCommand), new object[] { commands }));
                commands.Add(new CommandOptions("colortest", typeof(ColorTestCommand), null));
                commands.Add(new CommandOptions("info", typeof(InfoCommand), null));

                // MAC AND IP READER
                /*
                commands.Add(new CommandOptions("mac finder start", typeof(CatchBasicInfoStartCommand), new object[] { macFeeder }));
                commands.Add(new CommandOptions("mac finder stop", typeof(CatchBasicInfoStopCommand), new object[] { macFeeder }));
                commands.Add(new CommandOptions("mac finder set time", typeof(CatchBasicInfoSetTimeIntervallCommand), new object[] { macFeeder }));
                commands.Add(new CommandOptions("mac finder print list", typeof(CatchBasicInfoPrintHostsCommand), new object[] { macFeeder }));
                */
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
                /*
                commands.Add(new CommandOptions("cliserver", typeof(CLIServerInfo), new object[] { cliServer }));
                commands.Add(new CommandOptions("cliserver start", typeof(CLIServerStart), new object[] { cliServer }));
                commands.Add(new CommandOptions("cliserver stop", typeof(CLIServerStop), new object[] { cliServer }));
                commands.Add(new CommandOptions("cliserver changeport", typeof(CLIChangePort), new object[] { cliServer }));*/
            }
        }

        private bool Login(string loginData)
        {
            string[] buffer = loginData.Split(new string[] { "<seperator>" },StringSplitOptions.None);

            return true;
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
