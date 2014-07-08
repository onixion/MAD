using System;

using System.Net;
using System.Net.Sockets;
using System.Threading;

using MAD.Helper;

namespace MAD.DHCPReader
{
    public class DHCPReader
    {
        private static NetworkHelper helper = new NetworkHelper();
        private IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 67);
        private uint threadCnt = 0;
        private byte[] data;
        private uint maxThreads = 100;

        public void Execute()
        {
            
        }

        private void CatchDHCP()
        {
            UdpClient listener = new UdpClient(67);
            data = listener.Receive(ref groupEP);
            listener.Close();
        }

        private void StartThread()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessData));
        }

        private void ProcessData(Object stateInfo)
        {

        }
    }
}
