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

            // init CLI
            //_InitCLI(0);

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

        private void Start()
        {
            NetworkStream stream = client.GetStream();

            // first time send cursor
            NetCommunication.SendString(stream, cursor, true);

            string cliInput;
            string response;

            while (true)
            {
                cliInput = NetCommunication.ReceiveString(stream);

                response = AnalyseInput(cliInput, ref command);

                if (response == "VALID_PARAMETER")
                {
                    NetCommunication.SendString(stream, command.Execute() + "\n<color><gray>" + cursor, true);
                }
                else
                {
                    NetCommunication.SendString(stream, response + "\n<color><gray>" + cursor, true);
                }
            }
        }

        #endregion
    }
}
