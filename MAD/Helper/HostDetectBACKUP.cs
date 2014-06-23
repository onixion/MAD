using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

using System.Net;
using System.Net.NetworkInformation;

using MAD.jobSys;

namespace MAD.HostDetect
{
    public class HostDetect
    {
        public List<IPAddress> Detect(string Net, string Subnetmask)
        {
            List<IPAddress> hostAddresses = new List<IPAddress>();

            IPAddress toDetect = IPAddress.Parse(Net);
            IPAddress Mask = IPAddress.Parse(Subnetmask);

            int lifeFor = 50;

            NetworkHelper helper = new NetworkHelper();

            byte[] netBytes = toDetect.GetAddressBytes();
            byte[] hostBytes = helper.GetHosts(Mask);
            byte[] subnetBytes = helper.GetNet(Mask);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(hostBytes);
                Array.Reverse(netBytes);
            }

            uint tempTarget = BitConverter.ToUInt32(netBytes, 0);
            uint hosts = BitConverter.ToUInt32(hostBytes, 0);

            for (uint counter = 1; counter < hosts; counter++)
            {
                Ping ping = new Ping();

                uint target = tempTarget + counter;
                byte[] targetByte = BitConverter.GetBytes(target);
                Array.Reverse(targetByte);
                IPAddress targetIP = new IPAddress(targetByte);

                PingReply reply = ping.Send(targetIP, lifeFor);

                if (reply.Status == IPStatus.Success)
                {
                    hostAddresses.Add(reply.Address);
                }
                Console.WriteLine("{0}", counter);
                ping.Dispose();
            }

            return (hostAddresses);

        }

    }
}
