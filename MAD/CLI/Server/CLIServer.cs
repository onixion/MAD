using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

using MAD.NetIO;
using MAD.jobSys;

namespace MAD.cli
{
    public class CLIServer : CLIServerInternal
    {
        #region members

        private Version _version = new Version(1, 5);
        public string version { get { return _version.ToString(); } }

        private string _dataPath;
        private const string _logFilename = "log.txt";

        private TcpListener _serverListener;

        private List<CLIUser> _users = new List<CLIUser>();
        private List<CLISession> _sessions = new List<CLISession>();

        private JobSystem _js;

        #endregion

        #region constructor

        public CLIServer(int port, string dataPath, JobSystem js)
        {
            serverPort = port;
            _dataPath = dataPath;

            _js = js;

            // TODO: Load users out of the database.
            _users.Add(new CLIUser("root", "123", CLIUser.Group.root));
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
                /*
                // First send server informations to client.
                NetCom.SendString(_clientStream, "Mad CLI-Server <" + version + ">", true);

                // Receive the login-data.
                string loginData = NetCom.ReceiveString(_clientStream);

                // Checl the login-data.
                CLIUser _user = Login(loginData);

                if (_user != null)
                {
                    //NetCom.SendString(_clientStream, "ACCESS GRANTED", true);

                    // Init CLISession for client.
                    CLISession _session = new CLISession(_client, _user);

                    // Set objects for commands.
                    _session.SetWorkObjects(_js, null);

                    // Add commands to this session.
                    _session.AddToCommands(CLIFramework.CommandGroup.Gereral, CLIFramework.CommandGroup.JobSystem);

                    // Add session to list.
                    _sessions.Add(_session);

                    // Start session in own thread from SmartThreadPool
                    _session.Start();
                }
                else
                {
                    //NetCom.SendString(_clientStream, "ACCESS DENIED", true);
                }*/
            }
            catch (Exception)
            {
                // Client lost connection or disconnected.
                Log("Client (" + _clientEndpoint.Address + ":" + _clientEndpoint.Port + ") lost connection to server.");
            }

            // Client disconnected.
            Log("Client (" + _clientEndpoint.Address + ":" + _clientEndpoint.Port + ") disconnected.");

            _client.Close();

            return null;
        }

        public void ChangePort(int newPort)
        {
            if (!IsListening)
            {
                serverPort = newPort;
            }
            else
            {
                throw new Exception("Server running!");
            }
        }

        private CLIUser Login(string loginData)
        {
            string[] buffer = loginData.Split(new string[] { "<seperator>" },StringSplitOptions.None);

            if (buffer.Length == 2)
            {
                if (CheckUsernameAndPassword(buffer[0], buffer[1]))
                {
                    CLIUser _user = GetCLIUser(buffer[0]);

                    if (!_user.online)
                    {
                        _user.online = true;
                        return _user;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
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
                //writer.WriteLine("[ " + nc.NetCom.DateStamp() + " | " + nc.NetCom.TimeStamp() + " ] " + data);
            }
        }

        #endregion
    }
}
