﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

namespace MAD.CLI
{
    public class CLIServer : CLIServerInternal
    {
        public Version version = new Version(0, 0, 1000);

        private TcpListener serverListener;
        private TcpClient client;

        private readonly string dataPath;

        private List<CLIUser> users;
        private List<CLISession> sessions;

        private string secKey = "123";

        public CLIServer(string dataPath, int port)
        {
            this.dataPath = dataPath;

            // init server
            serverListener = new TcpListener(new IPEndPoint(IPAddress.Loopback, port));

            // init server vars
            users = new List<CLIUser>(){new CLIUser("root", "123")};
            sessions = new List<CLISession>();
        }

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
            NetworkStream _stream = _client.GetStream();

            try
            {
                // HERE HERE HERE
            }
            catch (Exception)
            {
                
            }             

            _stream.Close();
            _client.Close();

            return null;
        }

        #region Send/Recieve

        private void Send(NetworkStream _stream, string _data)
        {
            using (BinaryWriter _writer = new BinaryWriter(_stream))
            {
                _writer.Write(_data);
            }
        }

        private string Receive(NetworkStream _stream)
        {
            using (BinaryReader _reader = new BinaryReader(_stream))
            {
                return _reader.ReadString();
            }
        }

        #endregion

        #region Usermanagment / Security

        private bool CheckUsernameAndPassword(string username, string passwordMD5)
        {
            CLIUser user = GetCLIUser(username);

            if (user != null)
            {
                if (user.passwordMD5 == passwordMD5)
                {
                    return false;
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
    }
}
