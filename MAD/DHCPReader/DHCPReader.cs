using System;
using System.Collections.Generic;

using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
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
        private bool acknowledge; 
        private bool addressGiven;
        private bool nameGiven = false;
        private IPAddress ip;
        private string hostName = "";
        public List<ModelHost> dummyList; 
        
        public void Execute()
        {
            while (true)
            {
                CatchDHCP();
                StartThread();
            }
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
                    
                    switch (Convert.ToUInt16(data[i]))
                    {
                        case 50:
                            byte[] ipBytes = new byte[4];
                            for (uint ii = 0; ii < 4; ii++)
                            {
                                ipBytes[ii] = data[i + 2 + ii];
                            }
                            addressGiven = true;
                            ip = new IPAddress(ipBytes);
                            continue;
                        case 12:
                            
                            byte nameLength = data[i + 1];
                            if (!nameGiven)
                            {
                                try
                                {
                                    for (uint iii = 1; iii <= nameLength; iii++)
                                    {
                                        hostName += (char)data[i + 1 + iii];
                                        nameGiven = true; 
                                    }
                                }
                                catch
                                {
                                    nameGiven = false; 
                                }
                            }
                            continue;
                        default: 
                            break;
                    }
                    if(addressGiven)
                    {
                        Thread.Sleep(3000);
                        try
                        {
                            Ping _ping = new Ping();
                            PingReply _reply = _ping.Send(ip);

                            if (_reply.Status == IPStatus.Success)
                                acknowledge = true;
                            else
                                acknowledge = false;
                        }
                        catch
                        {
                            acknowledge = false;
                        }
                    }

                    if (addressGiven && acknowledge && nameGiven)
                    {
                        dummyList.Remove(dummyList.Find(x => x.hostMac.Contains(hexString)));
                        dummyList.Add(new ModelHost(hexString, ip, hostName));
                    }
                    else if (addressGiven && acknowledge && !nameGiven)
                    {
                        dummyList.Remove(dummyList.Find(x => x.hostMac.Contains(hexString)));
                        dummyList.Add(new ModelHost(hexString, ip));
                    }
                    else if (!addressGiven || !acknowledge && nameGiven)
                    {
                        dummyList.Remove(dummyList.Find(x => x.hostMac.Contains(hexString)));
                        dummyList.Add(new ModelHost(hexString, hostName));
                    }
                    else if (!addressGiven || !acknowledge && !nameGiven)
                    {
                        dummyList.Remove(dummyList.Find(x => x.hostMac.Contains(hexString)));
                        dummyList.Add(new ModelHost(hexString));
                    }
                    //TODO CleanCoding the shit out of it
                }
            }
        }
    }
}