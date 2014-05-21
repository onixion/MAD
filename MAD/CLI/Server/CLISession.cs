using System;
using System.Reflection;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MAD.CLI
{
    public class CLISession : CLIFramework
    {
        private static int _sessionsCount = 0;
        public int sessionID;
        private object _sessionInitLock = new object();

        private TcpClient client;
        private IPEndPoint clientEndPoint;

        // --------------------------------------------------------
        //          CLI Session
        // --------------------------------------------------------

        public CLISession(TcpClient client)
        {
            this.client = client;
            this.clientEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;

            // init CLISession
            _InitSession();

            // init CLI
            _InitCLI();

            // start session
            this.Start();
        }

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
           
        }
    }
}
