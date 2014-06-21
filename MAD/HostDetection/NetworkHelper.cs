using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.NetworkInformation;

namespace MAD.HostDetect
{
    public class NetworkHelper
    {
        public Byte[] GetHosts(IPAddress subnet)
        {
            byte[] byteSubnet = subnet.GetAddressBytes();

            for (int i = 0; i < byteSubnet.Length; i++)
            {
                byteSubnet[i] = (byte)~byteSubnet[i];
            }

            return (byteSubnet);
        }

        public Byte[] GetNet(IPAddress subnet)                      //Not really a komplex function, just for completeness
        {
            byte[] byteSubnet = subnet.GetAddressBytes();

            return (byteSubnet);
        }
    }
}
