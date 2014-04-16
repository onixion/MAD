using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SocketFramework;

namespace MAD
{
    public class CLIServer : SocketServer
    {
        private List<CLIUser> users = new List<CLIUser>();
        private string securePass = "123456";

        public CLIServer(int port)
        {
            InitSocketServer(new IPEndPoint(IPAddress.Loopback, port));
            InitCLIServer();
        }

        public void InitCLIServer()
        {
            users.Add(new CLIUser("admin", GetMD5Hash("yolo")));
        }

        public override void HandleClient(Socket socket)
        {
            Console.WriteLine("Client connected ...");

            string receivedSecurePass = Receive(socket);
            Send(socket, "OK");
            string username = Receive(socket);
            Send(socket, "OK");
            string passwordMD5 = GetMD5Hash(Receive(socket));

            if (CheckSecurePass(securePass))
            {
                if (CheckUsernameAndPassword(username, passwordMD5))
                {
                    Send(socket, "ACCEPTED");

                    /* After the client login to the cli server,
                     * he need to send the mode he want to enter.
                     * For the default cli, he need to send GET_CLI.
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
                    Send(socket, "DENIED");
            }
            else
                Send(socket, "DENIED");
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

        private bool CheckSecurePass(string securePass)
        {
            if (this.securePass == securePass)
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

        #endregion 
    }
}
