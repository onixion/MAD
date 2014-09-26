using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

using SnmpSharpNet;

namespace MAD.Helper
{
    public class NetworkHelper                                                      //This class is for Networkingstuff and needed in HostDetect.cs
    {
        #region Member
        public const uint DHCP_SERVER_PORT = 67;                                    //Port on which a DHCP Server listens; for catching DHCP Requests you will need this 
        public const uint DHCP_CLIENT_PORT = 68;                                    //Port on which a DHCP Client listens
        public const uint DHCP_COOKIE_POSITION = 236;                               //Position of magic dhcp cookie in the udp datagramm
        public const uint DHCP_COOKIE_VALUE = 1669485411;                           //Value of the four byte magic dhcp cookie
        
        public const byte DHCP_COOKIE_BYTE0_VALUE = 99;                             //Value of first byte of the four byte magic dhcp cookie
        public const byte DHCP_COOKIE_BYTE1_VALUE = 130;                            //Value of second -"-
        public const byte DHCP_COOKIE_BYTE2_VALUE = 83;                             //Value of third -"-
        public const byte DHCP_COOKIE_BYTE3_VALUE = 99;                             //Value of fourth -"-

        public const string SNMP_COMMUNITY_STRING = "public";                       //community String for snmp requests in MAD
        public const string SNMP_AUTH_SECRET = "MADMADMAD";                         //authentification Secret -"-
        public const string SNMP_PRIV_SECRET = "MADMADMAD";                         //privacy Secret -"-

        public static List<ModelHost> _dummyList = new List<ModelHost>();

        public enum snmpProtocols                                                   //an enumeration with the kinds of supported Protocols
        {
            MD5,
            SHA,
            AES,
            DES
        }

        public enum securityLvl                                                     //a enumeration with the levels of security provided by snmp
        {
            noAuthNoPriv,
            authNoPriv,
            authPriv
        }

        public struct securityModel                                                 //a struct which helps to contain all the security information needed for snmp
        {
            public NetworkHelper.securityLvl securityLevel;
            public NetworkHelper.snmpProtocols privacyProtocol;
            public NetworkHelper.snmpProtocols authentificationProtocol;
        }

        #endregion

        #region Methods

        public static uint GetHosts(uint subnet)
        {
            uint hosts = (uint) Math.Pow(2, 32 - subnet);

            return hosts; 
        }

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
            if (data[DHCP_COOKIE_POSITION] == DHCP_COOKIE_BYTE0_VALUE 
                && data[DHCP_COOKIE_POSITION + 1] == DHCP_COOKIE_BYTE1_VALUE 
                && data[DHCP_COOKIE_POSITION + 2] == DHCP_COOKIE_BYTE2_VALUE 
                && data[DHCP_COOKIE_POSITION + 3] == DHCP_COOKIE_BYTE3_VALUE)
            {
                return true;
            }
            else return false;
        }

        public bool IsDhcpRequest(byte[] data)                                      //Checks if udp datagramm (which should already be checked if dhcp) is a request
        {
            for (uint i = DHCP_COOKIE_POSITION; i < data.Length; i++)
            {
                if (data[i] == 53)
                {
                    if (data[i + 2] == 3)
                        return true;
                }
            }
            return false;
        }

        public static string getMacStringFromDhcp(byte[] data)                                     //Filters the MAC Address out of a dhcp packet (therefor it should be only used if already checkt wether it is a dhcp packet or not) 
        {
            byte[] macBytes = new byte[6];
            string macAddress = "";

            for (uint i = 28; i < (28 + 6); i++)
            {
                macBytes[i - 28] = data[i];
                macAddress += String.Format("{0:X02}", macBytes[i - 28]);
                if (i < (28 + 5))
                    macAddress += ":";
            }
            return macAddress;
        }

        public static string getMacStringFromArp(byte[] data)                                     //Filters the MAC Address out of a arp packet (therefor it should be only used if already checkt wether it is a arp packet or not) 
        {
            byte[] macBytes = new byte[6];
            string macAddress = "";

            for (uint i = 0; i < 6; i++)
            {
                macBytes[i] = data[i];
                macAddress += String.Format("{0:X02}", macBytes[i]);
                if (i < 5)
                    macAddress += ":";
            }
            return macAddress;
        }

        public string getPhysicalAddressString(byte[] data)                         // same as ^ but without doubledots, so PhysicalAddress Class can parse it
        {
            byte[] macBytes = new byte[6];
            string macAddress = "";

            for (uint i = 28; i < (28 + 6); i++)
            {
                macBytes[i - 28] = data[i];
                macAddress += String.Format("{0:X02}", macBytes[i - 28]);
                macAddress.ToUpperInvariant();
            }
            return macAddress;
        }

        public static AgentParameters GetSNMPV2Param(string communityString)
        {
            OctetString _community = new OctetString(communityString);
            AgentParameters _param = new AgentParameters(_community);

            _param.Version = SnmpVersion.Ver2;

            return _param; 
        }

        public static SecureAgentParameters GetSNMPV3Param(UdpTarget target, string communityString, securityModel secModel)
        {
            SecureAgentParameters _param = new SecureAgentParameters();

            if (!target.Discovery(_param))
            {
                return null;
            }

            switch (secModel.securityLevel)
            {
                case NetworkHelper.securityLvl.noAuthNoPriv:
                    _param.noAuthNoPriv(communityString);

                    break;
                case NetworkHelper.securityLvl.authNoPriv:
                    if (secModel.authentificationProtocol == NetworkHelper.snmpProtocols.MD5)
                        _param.authNoPriv(communityString, AuthenticationDigests.MD5, SNMP_AUTH_SECRET);
                    else if (secModel.authentificationProtocol == NetworkHelper.snmpProtocols.SHA)
                        _param.authNoPriv(communityString, AuthenticationDigests.SHA1, SNMP_AUTH_SECRET);

                    break;
                case NetworkHelper.securityLvl.authPriv:
                    if (secModel.authentificationProtocol == NetworkHelper.snmpProtocols.MD5 && secModel.privacyProtocol == NetworkHelper.snmpProtocols.AES)
                        _param.authPriv(SNMP_COMMUNITY_STRING, AuthenticationDigests.MD5, SNMP_AUTH_SECRET, PrivacyProtocols.AES128, SNMP_PRIV_SECRET);
                    else if (secModel.authentificationProtocol == NetworkHelper.snmpProtocols.MD5 && secModel.privacyProtocol == NetworkHelper.snmpProtocols.DES)
                        _param.authPriv(SNMP_COMMUNITY_STRING, AuthenticationDigests.MD5, SNMP_AUTH_SECRET, PrivacyProtocols.DES, SNMP_PRIV_SECRET);
                    else if (secModel.authentificationProtocol == NetworkHelper.snmpProtocols.SHA && secModel.privacyProtocol == NetworkHelper.snmpProtocols.AES)
                        _param.authPriv(SNMP_COMMUNITY_STRING, AuthenticationDigests.SHA1, SNMP_AUTH_SECRET, PrivacyProtocols.AES128, SNMP_PRIV_SECRET);
                    else if (secModel.authentificationProtocol == NetworkHelper.snmpProtocols.SHA && secModel.privacyProtocol == NetworkHelper.snmpProtocols.DES)
                        _param.authPriv(SNMP_COMMUNITY_STRING, AuthenticationDigests.SHA1, SNMP_AUTH_SECRET, PrivacyProtocols.DES, SNMP_PRIV_SECRET);

                    break;
            }

            return _param; 
        }
        #endregion
    }

    public class ModelHost                                                         //A class which provides all importand information for a host - feel free to put more in it!
    {
        public uint ID;

        public IPAddress hostIP;
        public string hostName;
        public string hostMac;

        public bool nameGiven, macGiven, ipGiven;

        private static uint _count = 0;

        public ModelHost()
        {
            macGiven = ipGiven = nameGiven = false;

            ID = _count;

            hostMac = null;
            hostName = null;
            hostIP = null;

        }

        public ModelHost(string MAC)
        {
            macGiven = true;
            ipGiven = nameGiven = false;

            ID = _count;

            hostMac = MAC;
            hostName = null;
            hostIP = null;

            _count++;
        }

        public ModelHost(string MAC, IPAddress address)
        {
            macGiven = ipGiven = true;
            nameGiven = false;

            ID = _count;

            hostMac = MAC;
            hostIP = address; 
            hostName = null;

            _count++;
        }

        public ModelHost(string MAC, string name)
        {
            nameGiven = macGiven = true;
            ipGiven = false;

            ID = _count;

            hostMac = MAC;
            hostName = name;
            hostIP = null;

            _count++;
        }

        public ModelHost(string MAC, IPAddress address, string name)
        {
            nameGiven = ipGiven = macGiven = true;

            ID = _count;

            hostMac = MAC;
            hostIP = address;
            hostName = name;

            _count++;
        }

        public void ManuallyIncreaseCount()
        {
            _count++;
        }

        public void ManuallyDecreaseCount()
        {
            _count--;
        }

        ~ModelHost()
        {
            _count--;
        }
    }
}
