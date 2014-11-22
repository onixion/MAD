using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

using MAD.JobSystemCore;
using MAD.CLICore;
using MAD.Logging;
using MadNet;

namespace MAD.CLIServerCore
{
    public class CLIServer : Server, IDisposable
    {
        #region members

        private bool _debugMode;
        private bool _logMode;
        private string _serverHeader;
        private string _serverVer = "v2.0";

        private TcpListener _serverListener;
        private AES _aes;
        
        private JobSystem _js;

        #endregion

        #region constructor

        public CLIServer(JobSystem js)
        {
            _aes = new AES(MadConf.conf.AES_PASS);

            lock (MadConf.confLock)
            {
                _logMode = MadConf.conf.LOG_MODE;
                _debugMode = MadConf.conf.DEBUG_MODE;
                _serverHeader = MadConf.conf.SERVER_HEADER;
                serverPort = MadConf.conf.SERVER_PORT;
            }

            _js = js;
        }

        #endregion

        #region methods

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

        protected override object GetClient()
        {
            return _serverListener.AcceptTcpClient();
        }

        protected override object HandleClient(object clientObject)
        {
            IPEndPoint _clientEndPoint = null;

            try
            {
                TcpClient _client = (TcpClient)clientObject;
                NetworkStream _stream = _client.GetStream();
                _clientEndPoint = (IPEndPoint)_client.Client.RemoteEndPoint;

                if (_debugMode)
                    Console.WriteLine("[" + MadNetHelper.GetTimeStamp() + "] Client (" + _clientEndPoint.Address + ") connected.");
                if (_logMode)
                    Logger.Log("Client (" + _clientEndPoint.Address + ") connected.", Logger.MessageType.INFORM);

                using (ServerInfoPacket _serverInfoP = new ServerInfoPacket(_stream))
                {
                    _serverInfoP.serverHeader = Encoding.Unicode.GetBytes(_serverHeader);
                    _serverInfoP.serverVersion = Encoding.Unicode.GetBytes(_serverVer);
                    _serverInfoP.SendPacket(_aes);
                }

                CLISession _session = new CLISession(_stream, _aes, _js);
                _session.InitCommands();
                _session.Start();
            }
            catch (Exception e)
            {
                if (_debugMode)
                    Console.WriteLine("[" + MadNetHelper.GetTimeStamp() + "] Execption: " + e.Message);
                if (_logMode)
                    Logger.Log(" Execption: " + e.Message, Logger.MessageType.ERROR);
            }
            finally
            {
                if (_debugMode)
                    Console.WriteLine("[" + MadNetHelper.GetTimeStamp() + "] Client (" + _clientEndPoint.Address + ") disconnected.");
                if (_logMode)
                    Logger.Log("Client (" + _clientEndPoint.Address + ") disconnected.", Logger.MessageType.INFORM);
            }

            return null;
        }

        public void Dispose()
        {
            _aes.Dispose();
        }

        #endregion
    }
}
