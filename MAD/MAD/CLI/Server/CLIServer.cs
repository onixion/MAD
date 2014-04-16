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
        // cli server vars
        private List<CLIUser> users = new List<CLIUser>();
        private string securePass = "123456";

        public CLIServer(int port)
        {
            InitServer(new IPEndPoint(IPAddress.Loopback, port));

            InitCLIServer();
        }

        public void InitCLIServer()
        {
            users.Add(new CLIUser("admin", "yolo"));
        }

        public override void HandleClient(Socket socket)
        {
            Console.WriteLine("Client connected: " + socket.RemoteEndPoint.AddressFamily.ToString());

            string receivedSecurePass = Receive(socket);
            Send(socket, "OK");
            string username = Receive(socket);
            Send(socket, "OK");
            byte[] password = GetMD5Hash(Encoding.ASCII.GetBytes(Receive(socket)));
            Send(socket, "OK");

            Console.WriteLine("SecurePass: " + receivedSecurePass);
            Console.WriteLine("Username: " + username);
            
            if(CheckSecurePass(receivedSecurePass))
            {
                if (CheckUsernameAndPassword(username, password))
                {
                    Console.WriteLine("YES");
                }
            }
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

        private bool CheckUsernameAndPassword(string username, byte[] password)
        {
            // get user (if user does not exist: client = null)
            CLIUser client = GetUser(username);

            // check if user exist
            if (client != null)
            {
                // check password
                if (client.CheckPassword(password))
                    return true;
            }
            return false;
        }

        #endregion 

    }
}
