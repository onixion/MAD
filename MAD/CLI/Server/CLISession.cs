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

        private TcpClient client;
        private IPEndPoint clientEndPoint;

        private CLIUser user;

        #endregion

        public CLISession(TcpClient client, CLIUser user)
        {
            this.client = client;
            this.clientEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;

            this.user = user;

            // init CLISession
            _InitSession();

            InitCLI(user);

            // start session
            this.Start();
        }

        #region methodes

        private void _InitSession()
        {
            lock (_sessionInitLock)
            {
                sessionID = _sessionsCount;
                _sessionsCount++;
            }
        }

        private void InitCLI(CLIUser user)
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


            if (user.group == CLIUser.Group.root)
            { 
            
            }
        }

        private void Start()
        {
            NetworkStream stream = client.GetStream();

            Command _command = null;

            NetCommunication.SendString(stream, cursor, true);

            while (true)
            {
                string _cliInput = NetCommunication.ReceiveString(stream);
                string _response = AnalyseInput(_cliInput, ref _command);

                if (_response == "VALID_PARAMETER")
                {
                    NetCommunication.SendString(stream, _command.Execute() + "\n<color><gray>" + cursor, true);
                }
                else
                {
                    NetCommunication.SendString(stream, _response + "\n<color><gray>" + cursor, true);
                }
            }
        }

        #endregion
    }
}
