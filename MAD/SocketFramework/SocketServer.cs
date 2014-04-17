using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace SocketFramework
{
    public abstract class SocketServer : SocketOperations
    {
        public Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public IPEndPoint serverEndPoint;
        public ManualResetEvent clieConn = new ManualResetEvent(false);

        public Thread listenThread;

        public void InitSocketServer(IPEndPoint serverEndPoint)
        {
            this.serverEndPoint = serverEndPoint;
        }

        #region Server handling methodes

        public void Start()
        {
            if (listenThread == null)
            {
                socket.Bind(serverEndPoint);
                socket.Listen(5);

                listenThread = new Thread(ListenOnSocket);
                listenThread.Start();
            }
        }

        public void Stop()
        {
            if (listenThread != null)
            {
                socket.Shutdown(SocketShutdown.Both);
                listenThread = null;
            }
        }

        private void ListenOnSocket()
        {
            while (true)
            {
                socket.BeginAccept(new AsyncCallback(HandleClientInternal), socket);
                clieConn.WaitOne();
                clieConn.Reset();
            }
        }

        private void HandleClientInternal(IAsyncResult result)
        {
            clieConn.Set();
            Socket temp = (Socket)result.AsyncState;
            Socket workSocket = temp.EndAccept(result);

            HandleClient(workSocket);
        }

        public abstract void HandleClient(Socket socket);

        #endregion
    }
}