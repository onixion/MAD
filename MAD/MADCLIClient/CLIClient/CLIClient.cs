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

            this.username = username;
            this.passwordMD5 = Encoding.ASCII.GetString(GetMD5Hash(Encoding.ASCII.GetBytes(password)));
            this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private byte[] GetMD5Hash(byte[] data)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5CryptoServiceProvider.Create();
            return md5.ComputeHash(data);
        } 
    }
}
