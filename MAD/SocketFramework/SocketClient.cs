using System;
using System.Net;
using System.Net.Sockets;

namespace SocketFramework
{
    public abstract class SocketClient : SocketFramework
    {
        public Socket serverSocket;
        public IPEndPoint serverEndPoint;

        public void InitSocketClient(Socket serverSocket, IPEndPoint serverEndPoint)
        {
            this.serverSocket = serverSocket;
            this.serverEndPoint = serverEndPoint;
        }
    }
}
