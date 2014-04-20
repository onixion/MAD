using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace SocketFramework
{
    public class SocketClient : SocketOperations
    {
        public Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public IPEndPoint serverEndPoint;

        public void InitClient(IPEndPoint serverEndPoint)
        {
            this.serverEndPoint = serverEndPoint;
        }
    }
}
