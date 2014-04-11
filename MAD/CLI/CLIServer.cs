using System;
using System.Net.Sockets;
using System.Threading;

namespace MAD
{
    public abstract class CLIServer()
    {
        public string version = "0.0.0.1";

        private TcpListener tcpListener;
        private TcpClient tcpClient;
        private Thread listenerThread;

        public void Init()
        {
            listenerThread = new Thread(WaitForClients);
        }

        public void Start()
        {
            if (listenerThread.ThreadState == ThreadState.Unstarted || listenerThread.ThreadState == ThreadState.Stopped)
            {
                listenerThread.Start();
            }
        }

        public void Stop()
        {
            if (listenerThread.ThreadState == ThreadState.Running)
            {
                listenerThread.Abort();
            }
        }

        private void WaitForClients()
        {
            while (true)
            {
                tcpClient = tcpListener.AcceptTcpClient();
            }
        }
    }
}
