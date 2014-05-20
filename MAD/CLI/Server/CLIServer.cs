using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MAD.CLI
{
    public class CLIServer : SocketFramework.SocketServer
    {
        private readonly string dataPath;

        private string cryptoKey;
        private string secureKey = "123456";

        private List<CLIUser> users;
        private List<CLISession> sessions;

        public CLIServer(string dataPath, int port)
        {
            this.dataPath = dataPath;

            // init server
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverEndPoint = new IPEndPoint(IPAddress.Any, port);
            InitSocketServer(serverSocket, serverEndPoint);

            // init server vars
            users = new List<CLIUser>(){new CLIUser("root", GetMD5Hash(Encoding.ASCII.GetBytes("lol")).ToString())};
            sessions = new List<CLISession>();
        }

        public override void HandleClient(Socket socket)
        {
            // get ip-endpoint from client
            IPEndPoint client = (IPEndPoint)socket.RemoteEndPoint;

            // log request to console
            Console.WriteLine("<" + GetTimeStamp() + "> Client (" + client.Address + ") connected.");

            while (true)
            {
                // reveive sercurePass
                string receivedSecurePass = Receive(socket);
                if (receivedSecurePass == null) { break; }
                if (!Send(socket, "OK")) { break; }

                // reveive username
                string username = Receive(socket);
                if (username == null) { break; }
                if (!Send(socket, "OK")) { break; }

                // reveive passwordMD5
                string passwordMD5 = Receive(socket);
                if (passwordMD5 == null) { break; }

                if (this.secureKey == receivedSecurePass)
                {
                    if (CheckUsernameAndPassword(username, passwordMD5))
                    {
                        // client accepted
                        if (!Send(socket, "ACCEPTED")) { break; }
                        Console.WriteLine("<" + GetTimeStamp() + "> Client (" + client.Address + ") has login as '" + username + "'.");

                        /* After the client login to the cli server,
                         * he need to send the mode he want to enter.
                         * For the default cli, he needs to send GET_CLI.
                         */

                        string mode = Receive(socket);
                        if (mode == null) { break; }

                        switch (mode)
                        {
                            case "GET_CLI":
                                sessions.Add(new CLISession(socket));
                                break;
                            // other modes ...

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

            Console.WriteLine("<" + GetTimeStamp() + "> Client (" + client.Address + ") disconnected.");

            sDisconnect(socket, serverEndPoint);
            socket.Close();
        }

        #region Usermanagment

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
