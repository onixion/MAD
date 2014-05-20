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

        /*
        public AutoResetEvent clientConnect = new AutoResetEvent(false);
        public AutoResetEvent clientDisconnect = new AutoResetEvent(false);
        public AutoResetEvent sendDone = new AutoResetEvent(false);
        public AutoResetEvent receiveDone = new AutoResetEvent(false);
        */

        public AutoResetEvent done = new AutoResetEvent(false);

        public Thread connectionCheck;
        public bool checkConnectionStop = false;

        private void CheckConnection(object socket)
        {
            Socket temp = (Socket)socket;

            while (true)
            {
                if (checkConnectionStop == true)
                {
                    checkConnectionStop = false;
                    break;
                }

                if (!temp.Connected)
                {
                    done.Set();
                    break;
                }

                Thread.Sleep(200);
            }
        }

        #region Connect methodes

        public bool sConnect(Socket socket, IPEndPoint serverEndPoint)
        {
            try
            {
                socket.BeginConnect(serverEndPoint, new AsyncCallback(sConnectCallback), socket);
                done.WaitOne();

                return true;
            }
            catch (Exception)
            {
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

            done.Set();
        }

        public bool sDisconnect(Socket socket, IPEndPoint serverEndPoint)
        {
            try
            {
                socket.BeginDisconnect(true, new AsyncCallback(sDisconnectCallback), socket);
                done.WaitOne();

                return true;
            }
            catch (Exception)
            {
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
   
            done.Set();
        }

        #endregion

        #region Send methods

        public bool Send(Socket socket, string text)
        {
            connectionCheck = new Thread(new ParameterizedThreadStart(CheckConnection));
            connectionCheck.Start(socket);

            SendObject sendObject = new SendObject();
            sendObject.sendSocket = socket;

            try
            {
                byte[] data = Encoding.ASCII.GetBytes(text);

                socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), sendObject);

                // wait for Callback or Client-Disconnect
                done.WaitOne();

                // stop checking connection of client
                checkConnectionStop = true;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void SendCallback(IAsyncResult result)
        {
            SendObject sendObject = (SendObject)result.AsyncState;

            try
            {
                int sendLength = sendObject.sendSocket.EndSend(result);

                if (sendLength > 0)
                {
                    sendObject.sendSocket.BeginSend(sendObject.data, 0, sendObject.sendLengthParts, SocketFlags.None, new AsyncCallback(SendCallback), sendObject);
                    done.WaitOne();
                }

                // send <EOF>
                byte[] endSymbol = Encoding.ASCII.GetBytes("<EOF>");

                sendObject.sendSocket.BeginSend(endSymbol, 0, endSymbol.Length, SocketFlags.None, new AsyncCallback(SendCallback), sendObject);
                done.WaitOne();
            }
            catch (Exception)
            { }
                
            done.Set();
        }

        #endregion

        #region Recieve methods

        public string Receive(Socket socket)
        {
            connectionCheck = new Thread(new ParameterizedThreadStart(CheckConnection));
            connectionCheck.Start(socket);

            RecieveObject recieveObject = new RecieveObject();
            recieveObject.recieveSocket = socket;

            try
            {
                socket.BeginReceive(recieveObject.readBytes, 0, recieveObject.readBytes.Length, 0, new AsyncCallback(ReceiveCallback), recieveObject);

                // wait for Callback or Client-Disconnect
                done.WaitOne();

                // stop checking connection of client
                checkConnectionStop = true;

                return recieveObject.recievedDataString;
            }
            catch (Exception)
            {
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
                        done.WaitOne();
                    }
                    else
                        recieveObject.recievedDataString = recieveObject.recievedDataString.Replace("<EOF>", "");
                }
            }
            catch (Exception)
            { }

            done.Set();
        }

        #region sendObject

        public class SendObject
        {
            public Socket sendSocket;

            public int sendLengthParts = 1048;
            public byte[] data;
        }

        #endregion

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
        /*
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
        */
        #endregion
    }

    public class SocketFrameworkNode : SocketFramework { }
}
