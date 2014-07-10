﻿using System;
using System.Net;
using System.Net.Sockets;

namespace MAD.Helper
{
    public class NetworkHelper                                                      //This class is for Networkingstuff and needed in HostDetect.cs
    {
        #region Member
        public const uint DHCP_SERVER_PORT = 67;                                    //Port on which a DHCP Server listens; for catching DHCP Requests you will need this 
        public const uint DHCP_CLIENT_PORT = 68;                                    //Port on which a DHCP Client listens

        public const uint _magicCookiePosition = 236;                               //Position of magic dhcp cookie in the udp datagramm

        public const uint MAGIC_COOKIE_VALUE = 1669485411;                          //Value of the four byte magic dhcp cookie
        public const byte COOKIE_BYTE0_VALUE = 99;                                  //Value of first byte of the four byte magic dhcp cookie
        public const byte COOKIE_BYTE1_VALUE = 130;                                 //Value of second -"-
        public const byte COOKIE_BYTE2_VALUE = 83;                                  //Value of third -"-
        public const byte COOKIE_BYTE3_VALUE = 99;                                  //Value of fourth -"-
        #endregion

        #region Methods
        public Byte[] GetHosts(IPAddress subnet)                                    //returns the maximal number of hosts for a given subnetmask INCLUSIVE Netaddress and Broadcastaddress!!!
        {
            byte[] subnetBytes = subnet.GetAddressBytes();

            for (int i = 0; i < subnetBytes.Length; i++)                            //just inverting every single bit -> subnetmask = !host
            {
                subnetBytes[i] = (byte)~subnetBytes[i];
            }

            return (subnetBytes);
        }

        public Byte[] GetNet(IPAddress subnet)                                      //Not really a complex function, just for completeness
        {
            byte[] subnetBytes = subnet.GetAddressBytes();

            return (subnetBytes);
        }

        public bool IsDhcp(byte[] data)                                             //Checks if udp datagramm is dhcp
        {
            if (data[_magicCookiePosition] == COOKIE_BYTE0_VALUE 
                && data[_magicCookiePosition + 1] == COOKIE_BYTE1_VALUE 
                && data[_magicCookiePosition + 2] == COOKIE_BYTE2_VALUE 
                && data[_magicCookiePosition + 3] == COOKIE_BYTE3_VALUE)
            {
                return true;
            }
            else return false;
        }

        public bool IsDhcpRequest(byte[] data)                                      //Checks if udp datagramm (which should already be checked if dhcp) is a request
        {
            for (uint i = _magicCookiePosition; i < data.Length; i++)
            {
                if (data[i] == 53)
                {
                    if (data[i + 2] == 3)
                        return true;
                }
            }
            return false;
        }
        #endregion
    }


    public struct ModelHost                                                                //A struct which provides all importand information for a host - feel free to put more in it!
    {
        public IPAddress hostIP;
        
        public string hostName;

        public string hostMac;

        public ModelHost(string MAC)
        {
            hostMac = MAC;
            hostName = null;
            hostIP = null; 
        }

        public ModelHost(string MAC, IPAddress address)
        {
            hostMac = MAC;
            hostIP = address; 
            hostName = null;
        }

        public ModelHost(string MAC, string name)
        {
            hostMac = MAC;
            hostName = name;
            hostIP = null;
        }

        public ModelHost(string MAC, IPAddress address, string name)
        {
            hostMac = MAC;
            hostIP = address;
            hostName = name; 
        }
    }
}
