using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using SocketFramework;

namespace CLIClient
{
    public class CLIClient : SocketClient 
    {
        private byte[] securePass = new byte[2048];

        private string username;
        private byte[] password;

        public CLIClient(IPAddress serverAddress, int serverPort, string username, string password, string securePass)
        {
            this.serverEndPoint = new IPEndPoint(serverAddress, serverPort);

            this.username = username;
            this.password = GetMD5Hash(Encoding.ASCII.GetBytes(password));
            this.securePass = Encoding.ASCII.GetBytes(securePass);
        }
    }
}
