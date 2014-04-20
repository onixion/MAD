using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Amib.Threading;

namespace SocketFramework
{
    public abstract class SocketServer : SocketOperations
    {
        public Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public IPEndPoint serverEndPoint;

        private ManualResetEvent clientConnect = new ManualResetEvent(false);

        public Thread listenerThread;
        public bool stopRequest = false;

        private SmartThreadPool threadPool = new SmartThreadPool();

        public void InitSocketServer(IPEndPoint serverEndPoint)
        {
            this.serverEndPoint = serverEndPoint;

            socket.Bind(serverEndPoint);
            socket.Listen(5);
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
                // give the thread a stop signal
                stopRequest = true;

                clientConnect.Set();
                clientConnect.Reset();

                // wait for thread to close
                listenerThread.Join();
                listenerThread = null;

                // cancel all threads
                //threadPool.Shutdown(); // NOT THE BEST IDEA
            }
        }

        private void ListenOnSocket()
        {
            while (true)
            {
                socket.BeginAccept(new AsyncCallback(HandleClientInternal), socket);
                clientConnect.WaitOne();
                clientConnect.Reset();

                if (stopRequest == true)
                {
                    stopRequest = false;
                    break;
                }
            }
        }

        private void HandleClientInternal(IAsyncResult result)
        {
            clientConnect.Set();
            Socket clientSocket = (Socket)result.AsyncState;
            threadPool.QueueWorkItem(HandleClient, clientSocket.EndAccept(result));
        }

        public abstract void HandleClient(Socket socket);

        #endregion
    }
}