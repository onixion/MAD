using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace SocketFramework
{
    public abstract class SocketFramework : SocketCryptography
    {
        public Version version = new Version(3, 0);

        public AutoResetEvent clientConnect = new AutoResetEvent(false);
        public AutoResetEvent clientDisconnect = new AutoResetEvent(false);
        public AutoResetEvent sendDone = new AutoResetEvent(false);
        public AutoResetEvent receiveDone = new AutoResetEvent(false);

        public int sendTimeout = 5000;
        public int receiveTimeout = 5000;

        #region Connect methodes

        public bool sConnect(Socket socket, IPEndPoint serverEndPoint)
        {
            try
            {
                socket.BeginConnect(serverEndPoint, new AsyncCallback(sConnectCallback), socket);
                clientConnect.WaitOne();

                return true;
            }
            catch (Exception)
            {
                clientConnect.Reset();
                return false;
            }
        }

        private void sConnectCallback(IAsyncResult result)
        {
            Socket temp = (Socket)result.AsyncState;

            try
            {
                temp.EndConnect(result);
            }
            catch (Exception)
            {
            }

            clientConnect.Set();
        }

        public bool sDisconnect(Socket socket, IPEndPoint serverEndPoint)
        {
            try
            {
                socket.BeginDisconnect(true, new AsyncCallback(sDisconnectCallback), socket);
                clientDisconnect.WaitOne();

                return true;
            }
            catch (Exception)
            {
                clientDisconnect.Reset();
                return false;
            }
        }

        private void sDisconnectCallback(IAsyncResult result)
        {
            Socket temp = (Socket)result.AsyncState;

            try
            {
                temp.EndDisconnect(result);
            }
            catch (Exception)
            {
            }
   
            clientDisconnect.Set();
        }

        #endregion

        #region Send methods

        public bool Send(Socket socket, string text)
        {
            try
            {
                byte[] data = Encoding.ASCII.GetBytes(text + "<EOF>");

                socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
                sendDone.WaitOne();

                sendDone.Reset();
                return true;
            }
            catch (Exception)
            {
                sendDone.Reset();
                return false;
            }
        }

        private void SendCallback(IAsyncResult result)
        {
            try
            {
                Socket temp = (Socket)result.AsyncState;
                temp.EndSend(result);
            }
            catch (Exception)
            {
            }
                
            sendDone.Set();
        }

        #endregion

        #region Recieve methods

        public string Receive(Socket socket)
        {
            RecieveObject recieveObject = new RecieveObject();
            recieveObject.recieveSocket = socket;

            try
            {
                socket.BeginReceive(recieveObject.readBytes, 0, recieveObject.readBytes.Length, 0, new AsyncCallback(ReceiveCallback), recieveObject);
                receiveDone.WaitOne();

                receiveDone.Reset();
                return recieveObject.recievedDataString;
            }
            catch (Exception)
            {
                receiveDone.Reset();
                return null;
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            RecieveObject recieveObject = (RecieveObject)result.AsyncState;

            try
            {
                int receiveLength = recieveObject.recieveSocket.EndReceive(result);

                if (receiveLength > 0)
                {
                    recieveObject.recievedDataString += Encoding.ASCII.GetString(recieveObject.readBytes, 0, receiveLength);

                    if (!recieveObject.recievedDataString.Contains("<EOF>"))
                    {
                        recieveObject.recieveSocket.BeginReceive(recieveObject.readBytes, 0, recieveObject.readBytes.Length, 0, new AsyncCallback(ReceiveCallback), recieveObject);
                        receiveDone.WaitOne();
                    }
                    else
                        recieveObject.recievedDataString = recieveObject.recievedDataString.Replace("<EOF>", "");
                }
            }
            catch (Exception)
            {

            }

                receiveDone.Set();
        }

        #region revieveObject

        public class RecieveObject
        {
            public Socket recieveSocket;

            public byte[] readBytes = new byte[1048];
            public string recievedDataString = "";
        }

        #endregion

        #endregion

        #region timestamp method

        public string GetTimeStamp()
        {
            return DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss");
        }

        #endregion


        #region testing

        public bool ssConnect(Socket socket, IPEndPoint serverEndpoint)
        {
            try
            {
                if (!socket.Connected)
                {
                    socket.Connect(serverEndpoint);
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool ssDisconnect(Socket socket)
        {
            if (socket.Connected)
            {
                try
                {
                    socket.Disconnect(true);
                    return true;
                }
                catch (Exception)
                { }
            }

            return false;
        }

        public bool ssSend(Socket socket, string data)
        {
            try
            {
                if (socket.Connected)
                {
                        socket.Send(Encoding.ASCII.GetBytes(data), SocketFlags.None);
                        return true;
                }
            }
            catch (Exception)
            { }

            return false;
        }

        public string ssReceive(Socket socket)
        {
            if (socket.Connected)
            {
                try
                {
                    string buffer = "";
                    while (true)
                    {
                        byte[] buffer2 = new byte[3];
                        int reveivedData = socket.Receive(buffer2);

                        buffer += Encoding.ASCII.GetString(buffer2);

                        if (buffer.Contains("\0"))
                        {
                            break;
                        }
                    }

                    return buffer;
                }
                catch (Exception)
                { }
            }

            return null;
        }

        #endregion
    }

    public class SocketFrameworkNode : SocketFramework { }
}
