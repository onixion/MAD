using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Security.Cryptography;
using System.Timers;

namespace SocketFramework
{
    public abstract class SocketOperations
    {
        public Version socketOPversion = new Version(1, 7);

        private ManualResetEvent clieConn = new ManualResetEvent(false);
        private ManualResetEvent clieDisc = new ManualResetEvent(false);
        private ManualResetEvent sendDone = new ManualResetEvent(false);
        private ManualResetEvent receDone = new ManualResetEvent(false);

        #region Connect methodes

        public bool Connect(Socket socket, IPEndPoint serverEndPoint)
        {
            try
            {
                socket.BeginConnect(serverEndPoint, new AsyncCallback(ConnectCallback), socket);
                clieConn.WaitOne();

                clieConn.Reset();
                return true;
            }
            catch (Exception)
            {
                clieConn.Reset();
                return false;
            }
        }

        private void ConnectCallback(IAsyncResult result)
        {
            Socket temp = (Socket)result.AsyncState;

            try
            {
                temp.EndConnect(result);
            }
            catch (Exception)
            {
            }

            clieConn.Set();
        }

        public bool Disconnect(Socket socket, IPEndPoint serverEndPoint)
        {
            try
            {
                socket.BeginDisconnect(true, new AsyncCallback(DisconnectCallback), socket);
                clieDisc.WaitOne();

                clieDisc.Reset();
                return true;
            }
            catch (Exception)
            {
                clieDisc.Reset();
                return false;
            }
        }

        private void DisconnectCallback(IAsyncResult result)
        {
            Socket temp = (Socket)result.AsyncState;

            try
            {
                temp.EndDisconnect(result);
            }
            catch (Exception)
            {
            }

            clieDisc.Set();
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
                receDone.WaitOne();

                receDone.Reset();
                return recieveObject.recievedDataString;
            }
            catch (Exception)
            {
                receDone.Reset();
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
                    recieveObject.recievedDataString += Encoding.ASCII.GetString(recieveObject.readBytes,0,receiveLength);

                    if (!recieveObject.recievedDataString.Contains("<EOF>"))
                    {
                        recieveObject.recieveSocket.BeginReceive(recieveObject.readBytes, 0, recieveObject.readBytes.Length, 0, new AsyncCallback(ReceiveCallback), recieveObject);
                        receDone.WaitOne();
                    }
                    else
                        recieveObject.recievedDataString = recieveObject.recievedDataString.Replace("<EOF>", "");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("RECEIVE-CALLBACK-ERROR!");
            }

            receDone.Set();
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

        #region Other methods

        public string GetMD5Hash(string data)
        {
            byte[] temp = Encoding.ASCII.GetBytes(data);
            MD5 alg = new MD5CryptoServiceProvider();
            return Encoding.ASCII.GetString(alg.ComputeHash(temp, 0, temp.Length));
        }

        public string GetTimeStamp()
        {
            return DateTime.Now.ToString("[dd.MM.yyyy]<HH:mm:ss>");
        }

        #endregion
    }


}
