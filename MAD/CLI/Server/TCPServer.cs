using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
//using Amib.Threading; GEHT NICHT?!

namespace MAD
{
    public abstract class TcpServer
    {
        public string version = "0.0.0.1";

        private Thread listenerThread;

        private TcpListener tcpListener;
        private TcpClient tcpClient;

        public void Init(int port)
        {
            tcpListener = new TcpListener(IPAddress.Loopback, port);
        }

        /// <summary>
        /// Start server.
        /// </summary>
        public void Start()
        {
            if (listenerThread == null)
            {
                listenerThread = new Thread(WaitForClients);
                listenerThread.Start();
            }
        }

        /// <summary>
        /// Stop server.
        /// </summary>
        public void Stop()
        {
            if (listenerThread.IsAlive)
            {
                listenerThread.Abort();
                listenerThread = null;
            }
        }

        /// <summary>
        /// Wait for clients to connect.
        /// </summary>
        private void WaitForClients()
        {
            while (true)
            {
                tcpClient = tcpListener.AcceptTcpClient();

                // THREADPOOL
                ThreadPool.QueueUserWorkItem(new WaitCallback(BuildConnection), tcpClient);
            }
        }

        private void BuildConnection(object client)
        {
            // CHECK FOR SECURE KEY

            HandleClient((TcpClient)client);
        }

        public abstract void HandleClient(TcpClient client);
    }
}
