using System;
using System.Reflection;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

using MAD.CLICore;
using MAD.JobSystemCore;
using MAD.NetIO;

namespace MAD.CLIServerCore
{
    public class CLISession : CLIFramework
    {
        #region members

        private static int _sessionsCount = 0;
        private int _sessionID;
        public int sessionID { get { return _sessionID; } }

        private object _sessionInitLock = new object();

        private TcpClient _client;
        private IPEndPoint _clientEndPoint;
        private CLIUser _user;

        private string _cursor = "=> ";

        #endregion

        public CLISession(TcpClient client, CLIUser user)
            : base()
        {
            lock (_sessionInitLock)
            {
                _sessionID = _sessionsCount;
                _sessionsCount++;
            }

            _client = client;
            _clientEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
            _user = user;
        }

        #region methodes

        public void Start()
        {
            NetworkStream _stream = _client.GetStream();
            Command _command = null;

            NetCom.SendStringUnicode(_stream, _cursor, true);

            while (true)
            {
                string _cliInput = NetCom.ReceiveStringUnicode(_stream);
                string _response = AnalyseInput(_cliInput, ref _command);

                if (_response == "VALID_PARAMETER")
                {
                    NetCom.SendStringUnicode(_stream, _command.Execute() + "\n<color><gray>" + _cursor, true);
                }
                else
                {
                    NetCom.SendStringUnicode(_stream, _response + "\n<color><gray>" + _cursor, true);
                }
            }
        }

        #endregion
    }
}
