using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Amib.Threading;

namespace MAD
{
    public abstract class TcpServer
    {
        public string version = "0.0.0.4";

        private Thread listenerThread;
        public SmartThreadPool clientPool;

        private TcpListener tcpListener;
        private TcpClient tcpClient;

        /// <summary>
        /// Initial server.
        /// </summary>
        public virtual void Init(int port)
        {
            tcpListener = new TcpListener(IPAddress.Loopback, port);
            clientPool = new SmartThreadPool();
            clientPool.MaxThreads = 3;
        }

        /// <summary>
        /// Start server.
        /// </summary>
        public void Start()
        {
            if (listenerThread == null)
            {
                try
                {
                    tcpListener.Start();

                    listenerThread = new Thread(WaitForClients);
                    listenerThread.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// Stop server.
        /// </summary>
        public void Stop()
        {
            if (listenerThread.IsAlive)
            {
                // stop TCP-listener
                tcpListener.Stop();

                // dispose listenerThread
                listenerThread.Abort();
                listenerThread = null;

                // cancel all threads in pool
                clientPool.Cancel();
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
                clientPool.QueueWorkItem(BuildConnection, tcpClient);
            }
        }

        /// <summary>
        /// Build everything up to ensure that the connection is astablished.
        /// </summary>
        private void BuildConnection(object client)
        {
            //

            HandleClient((TcpClient)client);
        }

        public abstract void HandleClient(TcpClient client);
    }
}
