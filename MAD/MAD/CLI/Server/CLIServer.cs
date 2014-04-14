using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace MAD
{
    public class CLIServer : TcpServer
    {
        // cli server vars
        private List<CLIUser> users = new List<CLIUser>();
        private byte[] securePass = new byte[2048];

        public CLIServer(int port)
        {
            Init(port);
        }

        /// <summary>
        /// Init all users for the CLI.
        /// </summary>
        public override void Init(int port)
        {
            base.Init(port);

            securePass = Encoding.ASCII.GetBytes("123456");
            users.Add(new CLIUser("admin", "yolobestpasseva"));
        }

        public override void HandleClient(TcpClient client)
        {
            CLIClient cliClient = new CLIClient(client);

            if (Login(cliClient))
            { 
                // START CLI
            
            }
        }

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

        private bool Login(CLIClient cliClient)
        {
            if (CheckSecurePass(cliClient))
            {
                Console.WriteLine("AUTH");
            }

            return false;
        }

        private bool CheckSecurePass(CLIClient cliClient)
        {
            byte[] temp = new byte[2048];
            temp = cliClient.BeginReadData(2048);

            if (temp == securePass)
                return true;
            else
                return false;
        }

        private bool CheckUsernameAndPassword(CLIClient cliClient)
        {
            byte[] temp = new byte[2048];
            temp = cliClient.BeginReadData(2048);
            string temp2 = Encoding.ASCII.GetString(temp);

            CLIUser client = GetUser(temp2);

            if (client != null)
            {
                
            }

            return false;
        }
    }
}
