using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

namespace MAD.CLI.Server
{
    public class CLIServer : CLIServerInternal
    {
        #region members

        public Version version = new Version(0, 8, 1000);

        private readonly string dataPath;
        private const string logDirName = "log";

        private TcpListener serverListener;

        public List<CLIUser> users;
        public List<CLISession> sessions;

        #endregion

        public CLIServer(string dataPath, int port)
        {
            this.dataPath = dataPath;

            // init tcp-server
            serverListener = new TcpListener(new IPEndPoint(IPAddress.Loopback, port));

            // init server vars
            users = new List<CLIUser>(){new CLIUser("root", NetCommunication.GetHash("123"), CLIUser.Group.root)};
            sessions = new List<CLISession>();
        }

        #region methodes

        protected override bool StartListener()
        {
            try
            {
                serverListener.Start();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected override void StopListener()
        {
            serverListener.Stop();
        }

        protected override object GetClient()
        {
            return serverListener.AcceptTcpClient();
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
                NetCommunication.SendString(_clientStream, "Mad CLI-Server v" + version, true);

                // receive login data
                string loginData = NetCommunication.ReceiveString(_clientStream);

                if (Login(loginData))
                {
                    NetCommunication.SendString(_clientStream, "ACCESS GRANTED", true);
                    sessions.Add(new CLISession(_client, null)); // TODO: CLIUser Managment
                }
                else
                {
                    NetCommunication.SendString(_clientStream, "ACCESS DENIED", true);
                }
            }
            catch (Exception)
            {

            }             

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

        private void Logout(string username)
        {
        
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
                    // password wrong.
                    return false;
                }
            }
            else
            {
                // username wrong.
                return false;
            }
        }

        private CLIUser GetCLIUser(string username)
        {
            foreach (CLIUser temp in users)
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
            if (!Directory.Exists(Path.Combine(dataPath, logDirName)))
            {
                Directory.CreateDirectory(Path.Combine(dataPath, logDirName));
            }
        }

        private void Log(string data)
        {
            CreateLogDir();

            using(FileStream stream = new FileStream(Path.Combine(dataPath, logDirName, "logCLISERVER.txt"), FileMode.Append, FileAccess.Write, FileShare.Read))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine("[ " + NetCommunication.DateStamp() + " | " + NetCommunication.TimeStamp() + " ] " + data);
            }
        }

        #endregion
    }
}
