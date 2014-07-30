using System;
using System.Reflection;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

using MAD.CLICore;
using MAD.JobSystemCore;

using MadNet;

namespace MAD.CLIServerCore
{
    public class CLISession : CLIFramework
    {
        #region members

        private static uint _sessionsCount = 0;
        private uint _sessionID;
        public uint sessionID { get { return _sessionID; } }

        private object _sessionInitLock = new object();

        private TcpClient _client;
        private IPEndPoint _clientEndPoint;

        private string _cursor = "=> ";

        private Command _command;
        private string _response;

        private int _consoleWidth = 0;

        #endregion

        public CLISession(TcpClient client)
            : base()
        {
            lock (_sessionInitLock)
            {
                _sessionID = _sessionsCount;
                _sessionsCount++;
            }

            _client = client;
            _clientEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
        }

        #region methodes

        public void Start()
        {
            NetworkStream _stream = _client.GetStream();

            // Send first cursor.
            NetCom.SendStringUnicode(_stream, _cursor, true);

            while (true)
            {
                // First receive console-width.
                try { _consoleWidth = System.Convert.ToInt32(NetCom.ReceiveStringUnicode(_stream)); }
                catch (Exception)
                {
                    _consoleWidth = 50;
                }

                // Then cli-input of the client.
                _response = NetCom.ReceiveStringUnicode(_stream);
                _response = AnalyseInput(ref _command, _response);

                if (_response == "VALID_par")
                {
                    NetCom.SendStringUnicode(_stream, _command.Execute(_consoleWidth) + "\n<color><gray>" + _cursor, true);
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
