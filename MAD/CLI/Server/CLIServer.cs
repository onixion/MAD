using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using nc;

namespace MAD.CLI.Server
{
    public class CLIServer : CLIServerInternal
    {
        #region members

        private Version _version = new Version(1,0);
        public string version { get { return _version.ToString(); } }

        private readonly string _dataPath;
        private const string _logFilename = "log.txt";

        private TcpListener _serverListener;

        private List<CLIUser> _users = new List<CLIUser>();
        private List<CLISession> _sessions = new List<CLISession>();

        #endregion

        public CLIServer(string dataPath, int port)
        {
            _dataPath = dataPath;

            _serverListener = new TcpListener(new IPEndPoint(IPAddress.Loopback, port));
            _users.Add(new CLIUser("root", nc.NetCom.GetHash("123"), CLIUser.Group.root));
        }

        #region methodes

        protected override bool StartListener()
        {
            try
            {
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
            TcpClient _client = (TcpClient)clientObject;
            IPEndPoint _clientEndpoint = (IPEndPoint)_client.Client.RemoteEndPoint;
            NetworkStream _clientStream = _client.GetStream();

            Log("Client (" + _clientEndpoint.Address + ":" + _clientEndpoint.Port + ") connected.");

            try
            {
                // send server info
                nc.NetCom.SendString(_clientStream, "Mad CLI-Server <" + version + ">", true);

                // receive login data
                string loginData = nc.NetCom.ReceiveString(_clientStream);

                // TODO: USER-MANAGMENT

                // check login data
                if (Login(loginData))
                {
                    nc.NetCom.SendString(_clientStream, "ACCESS GRANTED", true);
                    _sessions.Add(new CLISession(_client, null));
                }
                else
                {
                    nc.NetCom.SendString(_clientStream, "ACCESS DENIED", true);
                }
            }
            catch (Exception)
            {
                // client lost connection
                Log("Client (" + _clientEndpoint.Address + ":" + _clientEndpoint.Port + ") lost connection to server.");
            }

            // client disconnected
            Log("Client (" + _clientEndpoint.Address + ":" + _clientEndpoint.Port + ") disconnected.");

            _client.Close();

            return null;
        }

        private bool Login(string loginData)
        {
            string[] buffer = loginData.Split(new string[] { "<seperator>" },StringSplitOptions.None);

            if (buffer.Length == 2)
            {
                if (CheckUsernameAndPassword(buffer[0], buffer[1]))
                {
                    CLIUser user = GetCLIUser(buffer[0]);

                    if (!user.online)
                    {
                        user.online = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool CheckUsernameAndPassword(string username, string passwordMD5)
        {
            CLIUser user = GetCLIUser(username);

            if (user != null)
            {
                if (user.passwordMD5 == passwordMD5)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private CLIUser GetCLIUser(string username)
        {
            foreach (CLIUser temp in _users)
            {
                if (temp.username == username)
                {
                    return temp;
                }
            }

            return null;
        }

        #endregion

        #region Logger

        private void CreateLogDir()
        {
            if (!Directory.Exists(_dataPath))
            {
                Directory.CreateDirectory(_dataPath);
            }
       }

        private void Log(string data)
        {
            CreateLogDir();

            using(FileStream stream = new FileStream(Path.Combine(_dataPath, _logFilename), FileMode.Append, FileAccess.Write, FileShare.Read))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine("[ " + nc.NetCom.DateStamp() + " | " + nc.NetCom.TimeStamp() + " ] " + data);
            }
        }

        #endregion
    }
}
