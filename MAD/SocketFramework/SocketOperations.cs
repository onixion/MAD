using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Security.Cryptography;

namespace SocketFramework
{
    public abstract class SocketOperations
    {
        public ManualResetEvent sendDone = new ManualResetEvent(false);
        public ManualResetEvent receDone = new ManualResetEvent(false);

        #region Send methods

        public void Send(Socket socket, string text)
        {
            try
            {
                byte[] data = Encoding.ASCII.GetBytes(text + "<EOF>");
                socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
                sendDone.WaitOne();
                sendDone.Reset();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void SendCallback(IAsyncResult result)
        {
            try
            {
                Socket temp = (Socket)result.AsyncState;
                temp.EndSend(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            receDone.Reset();

            return null;
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
                    {
                        recieveObject.recievedDataString = recieveObject.recievedDataString.Replace("<EOF>", "");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            receDone.Set();
        }

        #endregion

        #region Other methods

        public byte[] GetMD5Hash(byte[] data)
        {
            MD5 alg = new MD5CryptoServiceProvider();
            return alg.ComputeHash(data, 0, data.Length);
        }

        #endregion
    }

    public class RecieveObject
    {
        public Socket recieveSocket;

        public byte[] readBytes = new byte[1048];
        public string recievedDataString = "";
    }
}
