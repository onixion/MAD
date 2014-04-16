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
        public ManualResetEvent clieConn = new ManualResetEvent(false);

        public void InitClient(IPEndPoint serverEndPoint)
        {
            this.serverEndPoint = serverEndPoint;
        }

        #region Connect methodes

        public void Connect()
        {
            try
            {
                socket.BeginConnect(serverEndPoint, new AsyncCallback(ConnectCallback), socket);
                clieConn.WaitOne();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            clieConn.Reset();
        }

        private void ConnectCallback(IAsyncResult result)
        {
            Socket temp = (Socket)result.AsyncState;

            try
            {
                temp.EndConnect(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            clieConn.Set();
        }

        #endregion
    }
}
