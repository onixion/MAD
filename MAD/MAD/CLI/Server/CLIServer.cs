﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SocketFramework;

namespace MAD
{
    public class CLIServer : SocketServer
    {
        private string secureKey = "123456";
        private List<CLIUser> users;

        public CLIServer(int port)
        {
            InitSocketServer(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp), new IPEndPoint(IPAddress.Loopback, 999));
            InitCLIUsers();
        }

        public override void HandleClient(Socket socket)
        {
            // get ip from client
            IPEndPoint client = (IPEndPoint)socket.RemoteEndPoint;

            // log request to console
            Console.WriteLine(GetTimeStamp() + " Client (" + client.Address + ") connected ...");

            // reveive sercurePass
            string receivedSecurePass = Receive(socket);
            Send(socket, "OK");

            // reveive username
            string username = Receive(socket);
            Send(socket, "OK");

            // reveive passwordMD5
            string passwordMD5 = Receive(socket);

            if (CheckSecureKey(secureKey))
            {
                if (CheckUsernameAndPassword(username, passwordMD5))
                {
                    // client accepted
                    Send(socket, "ACCEPTED");
                    Console.WriteLine(GetTimeStamp() + " Client (" + client.Address + ") login as '" + username + "'.");

                    /* After the client login to the cli server,
                     * he need to send the mode he want to enter.
                     * For the default cli, he needs to send GET_CLI.
                     * */

                    string mode = Receive(socket);

                    switch (mode)
                    {
                        case "GET_CLI":
                            CLI cli = new CLI(socket);
                            cli.Start();
                            break;

                        default:
                            Send(socket, "MODE_UNKNOWN");
                            break;
                    }
                }
                else
                {
                    Send(socket, "DENIED");
                    Console.WriteLine(GetTimeStamp() + " Client (" + client.Address + ") failed to login. Username or password wrong.");
                }
            }
            else
            {
                Send(socket, "DENIED");
                Console.WriteLine(GetTimeStamp() + " Client (" + client.Address + ") failed to login. SecurePass wrong.");
            }
        }

        public void InitCLIUsers()
        {
            users = new List<CLIUser>()
            {
                new CLIUser("admin", Encoding.ASCII.GetString(GetMD5Hash(Encoding.ASCII.GetBytes("yolo"))))
            };
        }

        #region Usermanagment

        public bool UserExist(string username)
        {
            foreach (CLIUser temp in users)
                if (temp.username == username)
                    return true;

            return false;
        }

        public CLIUser GetUser(string username)
        {
            foreach (CLIUser temp in users)
                if (temp.username == username)
                    return temp;

            return null;
        }

        #endregion

        #region Security

        private bool CheckSecureKey(string secureKey)
        {
            if (this.secureKey == secureKey)
                return true;
            else
                return false;
        }

        private bool CheckUsernameAndPassword(string username, string passwordMD5)
        {
            // get user (if user does not exist: client = null)
            CLIUser client = GetUser(username);

            // check if user exist
            if (client != null)
            {
                // check password
                if (client.passwordMD5 == passwordMD5)
                    return true;
            }
            return false;
        }

        private string GetTimeStamp()
        {
            return DateTime.Now.ToString(" dd-MM-yyyy HH-mm-ss ");
        }

        private byte[] GetMD5Hash(byte[] data)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5CryptoServiceProvider.Create();
            return md5.ComputeHash(data);
        }

        #endregion 
    }
}
