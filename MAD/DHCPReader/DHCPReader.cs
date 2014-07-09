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
        //private uint threadCnt = 0;
        private uint hostCnt = 0;
        private byte[] data;
        //private uint maxThreads = 100;

        public ModelHost[] dummy;
        
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
            if (helper.IsDhcp(data) && helper.IsDhcpRequest(data))
            {
                byte[] hexNumber = new byte[6];
                string hexString = "";
                
                for (uint i = 28; i < (28 + 6); i++)
                {
                    hexNumber[i - 28] = data[i];
                    hexString += String.Format("{0:X02}", hexNumber[i - 28]);
                    if (i < (28 + 5))
                        hexString += ":";
                }
                
                for (uint i = NetworkHelper._magicCookiePosition; i < data.Length; i++)
                {
                    bool name = false;
                    switch (Convert.ToUInt16(data[i]))
                    {
                        case 50:
                            byte[] ipBytes = new byte[4];
                            for (int ii = 0; ii < 4; ii++)
                            {
                                ipBytes[ii] = data[i + 2 + ii];
                            }
                            bool address = true;
                            IPAddress ip = new IPAddress(ipBytes);
                            continue;
                        case 12:
                            string hostName = "";
                            byte nameLength = data[i + 1];
                            if (!name)
                            {
                                for (int iii = 1; iii <= nameLength; iii++)
                                {
                                    hostName += (char)data[i + 1 + iii];
                                }
                            }
                            name = true; 
                            continue;
                        default: 
                            break;
                    }
                    //TODO Check if host is realy there or false requestet ip 
                    //TODO CleanCoding the shit out of it
                }
            }
        }
    }
}