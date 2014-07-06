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
            _users.Add(new CLIUser("root", MD5Hashing.ToMD5("123"), CLIUser.Group.root));
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

            // LOG

            try
            {
                // First send server informations to client.
                NetCom.SendStringUnicode(_clientStream, "Mad CLI-Server <" + version + ">", true);

                // Receive the login-data.
                string loginData = NetCom.ReceiveStringUnicode(_clientStream);

                // Check the login-data.
                CLIUser _user = Login(loginData);

                if (_user != null)
                {
                    NetCom.SendStringUnicode(_clientStream, "ACCESS GRANTED", true);

                    // Init CLISession for client.
                    CLISession _session = new CLISession(_client, _user);

                    // Add session to list.
                    _sessions.Add(_session);

                    // Start session in own thread from SmartThreadPool
                    _session.Start();
                }
                else
                {
                    NetCom.SendStringUnicode(_clientStream, "ACCESS DENIED", true);
                }
            }
            catch (Exception)
            {
                // Client lost connection or disconnected.
                // LOG
            }

            // Client disconnected.
            // LOG

            _client.Close();

            return null;
        }

        private void InitSessionCommands(CLISession session, CLIUser user)
        {
            List<CommandOptions> commands = session.commands;

            // For now, all user get the same commands.

            // !! INIT COMMANDS !!

            // general purpose
            commands.Add(new CommandOptions("exit", typeof(ExitCommand), null));
            commands.Add(new CommandOptions("help", typeof(HelpCommand), new object[] { commands }));
            commands.Add(new CommandOptions("colortest", typeof(ColorTestCommand), null));
            commands.Add(new CommandOptions("info", typeof(InfoCommand), null));

            // JobSystem
            commands.Add(new CommandOptions("js nodes", typeof(JobSystemStatusNodesCommand), new object[] { _js }));
            commands.Add(new CommandOptions("js jobs", typeof(JobSystemStatusJobsCommand), new object[] { _js }));
            commands.Add(new CommandOptions("scedule start", typeof(JobSceduleStartCommand), new object[] { _js }));
            commands.Add(new CommandOptions("scedule stop", typeof(JobSceduleStopCommand), new object[] { _js }));
            commands.Add(new CommandOptions("js status", typeof(JobStatusCommand), new object[] { _js }));
            commands.Add(new CommandOptions("js add ping", typeof(JobSystemAddPingCommand), new object[] { _js }));
            commands.Add(new CommandOptions("js add http", typeof(JobSystemAddHttpCommand), new object[] { _js }));
            commands.Add(new CommandOptions("js add port", typeof(JobSystemAddPortCommand), new object[] { _js }));
            commands.Add(new CommandOptions("js add detect", typeof(JobSystemAddHostDetectCommand), new object[] { _js }));
            commands.Add(new CommandOptions("js add serviceCheck", typeof(JobSystemAddServiceCheckCommand), new object[] { _js }));
            commands.Add(new CommandOptions("js destroy", typeof(JobSystemRemoveCommand), new object[] { _js }));
            commands.Add(new CommandOptions("js start", typeof(JobSystemStartCommand), new object[] { _js }));
            commands.Add(new CommandOptions("js stop", typeof(JobSystemStopCommand), new object[] { _js }));
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
    }
}
