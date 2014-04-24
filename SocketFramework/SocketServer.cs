using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Amib.Threading;
using SocketFramework;

namespace SocketFramework
{
    public abstract class SocketServer : SocketFramework
    {
        #region members
        public Socket serverSocket;
        public IPEndPoint serverEndPoint;

        public Thread listenerThread;
        public bool serverStopRequest = false;

        private SmartThreadPool threadPool = new SmartThreadPool();
        #endregion

        public void InitSocketServer(Socket serverSocket, IPEndPoint serverEndPoint)
        {
            this.serverSocket = serverSocket;
            this.serverEndPoint = serverEndPoint;

            this.serverSocket.Bind(serverEndPoint);
            this.serverSocket.Listen(20);
        }
        
        #region Server handling methodes

        public void Start()
        {
            if (listenerThread == null)
            {
                listenerThread = new Thread(ListenOnSocket);
                listenerThread.Start();
            }
        }

        public void Stop()
        {
            if (listenerThread != null)
            {
                serverStopRequest = true;

                clientConnect.Set();
                clientConnect.Reset();

                listenerThread.Join();
                listenerThread = null;

                threadPool.Cancel(); // NOT THE BEST IDEA
            }
        }

        private void ListenOnSocket()
        {
            while (true)
            {
                serverSocket.BeginAccept(new AsyncCallback(HandleClientInternal), serverSocket);

                clientConnect.WaitOne();
                clientConnect.Reset();

                if (serverStopRequest)
                {
                    serverStopRequest = false;
                    break;
                }
            }
        }

        private void HandleClientInternal(IAsyncResult result)
        {
            clientConnect.Set();
            Socket client = (Socket)result.AsyncState;

            // add client to ThreadPool
            threadPool.QueueWorkItem(HandleClient, client.EndAccept(result));
        }

        public abstract void HandleClient(Socket socket);

        #endregion
    }
}