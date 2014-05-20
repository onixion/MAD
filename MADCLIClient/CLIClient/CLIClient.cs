using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Security.Cryptography;
using SocketFramework;

namespace CLIClient
{
    public class CLIClient : SocketClient
    {
        public string securePass;
        public string username;
        public string passwordMD5;

        public CLIClient(IPEndPoint serverEndPoint, string securePass, string username, string password)
        {
            // init socketClient
            InitSocketClient(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp), serverEndPoint);

            this.securePass = securePass;
            this.username = username;
            this.passwordMD5 = Encoding.ASCII.GetString(GetMD5Hash(Encoding.ASCII.GetBytes(password)));
            this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
    }
}
