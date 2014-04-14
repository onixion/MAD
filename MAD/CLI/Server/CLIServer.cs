using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace MAD
{
    public class CLIServer : TcpServer
    {
        // cli server vars
        public List<CLIUser> users = new List<CLIUser>();

        // login vars
        public int loginTry = 3;

        public CLIServer(int port)
        {
            Init(port);
            InitUsers();
        }

        /// <summary>
        /// Init all available users for the CLI.
        /// </summary>
        private void InitUsers()
        { 
            users.Add(new CLIUser("admin", "yolobestpasseva"));
        }

        public override void HandleClient(TcpClient client)
        {
            Login();


            // CLI
        }

        public bool UsernameExist(string username)
        {
            foreach (CLIUser temp in users)
                if (temp.username == username)
                    return true;

            return false;
        }

        public void Login()
        {
            
        }
    }
}
