using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MAD
{
    public abstract class TcpServer
    {
        public string version = "0.0.0.1";

        private TcpListener tcpListener;
        private TcpClient tcpClient;

        private Thread listenerThread;
        // Threadpool for clients


        public void Init(int port)
        {
            tcpListener = new TcpListener(IPAddress.Loopback, port);
        }

        public void Start()
        {
            if (listenerThread == null)
            {
                listenerThread = new Thread(WaitForClients);
                listenerThread.Start();
            }
        }

        public void Stop()
        {
            if (listenerThread.IsAlive)
            {
                listenerThread.Abort();
                listenerThread = null;
            }
        }

        private void WaitForClients()
        {
            while (true)
            {
                tcpClient = tcpListener.AcceptTcpClient();


                // THREAD POOL
            }
        }

        public abstract void HandleClient(TcpClient client);
    }
}
