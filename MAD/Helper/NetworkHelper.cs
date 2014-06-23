using System;
using System.Net;

namespace MAD.HostDetect
{
    public class NetworkHelper                                     //This class is for Networkingstuff and needed in HostDetect.cs
    {
        public Byte[] GetHosts(IPAddress subnet)                    //returns the maximal number of hosts for a given subnetmask INCLUSIVE Netaddress and Broadcastaddress!!!
        {
            byte[] subnetBytes = subnet.GetAddressBytes();

            for (int i = 0; i < subnetBytes.Length; i++)            //just inverting every single bit -> subnetmask = !host
            {
                subnetBytes[i] = (byte)~subnetBytes[i];
            }

            return (subnetBytes);
        }

        public Byte[] GetNet(IPAddress subnet)                      //Not really a complex function, just for completeness
        {
            byte[] subnetBytes = subnet.GetAddressBytes();

            return (subnetBytes);
        }
    }
}
