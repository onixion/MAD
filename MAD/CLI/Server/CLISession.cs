using System;
using System.Reflection;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MAD.CLI.Server
{
    public class CLISession : CLIFramework
    {
        #region members

        private static int _sessionsCount = 0;
        public int sessionID;
        private object _sessionInitLock = new object();

        private TcpClient _client;
        private IPEndPoint _clientEndPoint;

        private CLIUser _user;

        private string _cursor = "=> ";

        #endregion

        public CLISession(TcpClient client, CLIUser user)
        {
            lock (_sessionInitLock)
            {
                sessionID = _sessionsCount;
                _sessionsCount++;
            }

            _client = client;
            _clientEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
            _user = user;

            InitCLI();

            // start session
            this.Start();
        }

        #region methodes

        private void InitCLI()
        {
            // GENERAL
            commands.Add(new CommandOptions("help", typeof(HelpCommand), new object[] { commands }));
            commands.Add(new CommandOptions("colortest", typeof(ColorTestCommand), null));
            commands.Add(new CommandOptions("info", typeof(InfoCommand), null));
            //commands.Add(new CommandOptions("test", typeof(TestCommand), null));

            // JOBSYSTEM
            commands.Add(new CommandOptions("js", typeof(JobSystemStatusCommand), null));
            commands.Add(new CommandOptions("js status", typeof(JobStatusCommand), null));
            commands.Add(new CommandOptions("js add ping", typeof(JobSystemAddPingCommand), null));
            commands.Add(new CommandOptions("js add http", typeof(JobSystemAddHttpCommand), null));
            commands.Add(new CommandOptions("js add port", typeof(JobSystemAddPortCommand), null));
            commands.Add(new CommandOptions("js destroy", typeof(JobSystemRemoveCommand), null));
            commands.Add(new CommandOptions("js start", typeof(JobSystemStartCommand), null));
            commands.Add(new CommandOptions("js stop", typeof(JobSystemStopCommand), null));
        }

        private void Start()
        {
            NetworkStream _stream = _client.GetStream();

            Command _command = null;

            NetCommunication.SendString(_stream, _cursor, true);

            while (true)
            {
                string _cliInput = NetCommunication.ReceiveString(_stream);
                string _response = AnalyseInput(_cliInput, ref _command);

                if (_response == "VALID_PARAMETER")
                {
                    NetCommunication.SendString(_stream, _command.Execute() + "\n<color><gray>" + _cursor, true);
                }
                else
                {
                    NetCommunication.SendString(_stream, _response + "\n<color><gray>" + _cursor, true);
                }
            }
        }

        #endregion
    }
}
