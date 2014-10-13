﻿using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Collections.Generic;

using SharpPcap;
using PacketDotNet;

using MAD.Logging;

namespace MAD.MacFinders
{
	public class ModelHost                                                         //A class which provides all importand information for a host - feel free to put more in it!
	{
		#region member
		public IPAddress hostIP;
		public string hostName;
		public string hostMac;
        public string status; 
        
        public static List<ModelHost> hostList = new List<ModelHost>();
		
        public bool nameGiven, macGiven, ipGiven;
        public bool reachable; 

		public static uint _count = 0;

        private static Object lockObj = new Object();
		#endregion

		#region constructors
		public ModelHost()
		{
			macGiven = ipGiven = nameGiven = false;

			hostMac = null;
			hostName = null;
			hostIP = null;

		}

		public ModelHost(string MAC)
		{
			macGiven = true;
			ipGiven = nameGiven = false;

			hostMac = MAC;
			hostName = null;
			hostIP = null;
		}

		public ModelHost(string MAC, IPAddress address)
		{
			macGiven = ipGiven = true;
			nameGiven = false;

			hostMac = MAC;
			hostIP = address; 
			hostName = null;
		}

		public ModelHost(string MAC, string name)
		{
			nameGiven = macGiven = true;
			ipGiven = false;

			hostMac = MAC;
			hostName = name;
			hostIP = null;
		}

		public ModelHost(string MAC, IPAddress address, string name)
		{
			nameGiven = ipGiven = macGiven = true;

			hostMac = MAC;
			hostIP = address;
			hostName = name;
		}
		#endregion

		#region privateFunctions

		private static void IncreaseCount()
		{
			_count++;
		}

		private static void DecreaseCount()
		{
			_count--;
		}
		#endregion

		#region publicFunctions

		#region Exists
		public static bool Exists(string macAddr)
		{
			bool _exists; 
			var _foo = hostList.Find(x => x.hostMac.Contains(macAddr));

			if(_foo == null)
				_exists = false;
			else
				_exists = true;

            return _exists;
		}

		public static bool Exists(ModelHost dummy)
		{
			bool _exists; 
			var _foo = hostList.Find(x => x.hostMac.Contains(dummy.hostMac));

			if(_foo == null)
				_exists = false;
			else
				_exists = true;

            return _exists;
		}
		#endregion

		#region AddToList
		public static void AddToList(ModelHost dummy)
		{
            lock (lockObj)
            {
                hostList.Add(dummy);
                if (!Exists(dummy))
                    IncreaseCount();
            }
		}

		public static void AddToList(string macAddr)
		{
            lock (lockObj)
            {
                ModelHost _dummy = new ModelHost(macAddr);
                hostList.Add(_dummy);
                if (!Exists(_dummy))
                    IncreaseCount();
            }		
        }

		public static void AddToList(string macAddr, IPAddress ipAddr)
		{
            lock (lockObj)
            {
                ModelHost _dummy = new ModelHost(macAddr, ipAddr);
                hostList.Add(_dummy);
                if (!Exists(_dummy))
                    IncreaseCount();
            }
		}

		public static void AddToList(string macAddr, string hostName)
		{
            lock (lockObj)
            {
                ModelHost _dummy = new ModelHost(macAddr, hostName);
                hostList.Add(_dummy);
                if (!Exists(_dummy))
                    IncreaseCount();
            }
		}

		public static void AddToList(string macAddr, IPAddress ipAddr, string hostName)
		{
            lock (lockObj)
            {
                ModelHost _dummy = new ModelHost(macAddr, ipAddr, hostName);
                hostList.Add(_dummy);
                if (!Exists(_dummy))
                    IncreaseCount();
            }
		}
		#endregion 

		#region RemoveFromList
		public static void RemoveFromList(ModelHost dummy)
		{
			if(Exists(dummy))
			{
                lock (lockObj)
                {
                    DecreaseCount();
                    hostList.Remove(hostList.Find(x => x.hostMac.Contains(dummy.hostMac)));
                }
			}
		}

		public static void RemoveFromList(string macAddr)
		{
			if(Exists(macAddr))
			{
                lock (lockObj)
                {
                    DecreaseCount();
                    hostList.Remove(hostList.Find(x => x.hostMac.Contains(macAddr)));
                }
			}
		}
		#endregion

		#region UpdateHosts
		public static void UpdateHost(string macAddr, IPAddress ipAddr)
		{
            lock (lockObj)
            {
                if (Exists(macAddr))
                {
                    RemoveFromList(macAddr);
                    AddToList(macAddr, ipAddr);
                }
                else
                {
                    AddToList(macAddr, ipAddr);
                }
            }
		}

		public static void UpdateHost(string macAddr, string hostName)
		{
            lock (lockObj)
            {
                if (Exists(macAddr))
                {
                    RemoveFromList(macAddr);
                    AddToList(macAddr, hostName);
                }
                else
                {
                    AddToList(macAddr, hostName);
                }
            }
		}

		public static void UpdateHost(string macAddr, IPAddress ipAddr, string hostName)
		{
            lock (lockObj)
            {
                if (Exists(macAddr))
                {
                    RemoveFromList(macAddr);
                    AddToList(macAddr, ipAddr, hostName);
                }
                else
                {
                    AddToList(macAddr, ipAddr, hostName);
                }
            }
		}

		public static void UpdateHost(string macAddr, ModelHost replacement)
		{
            lock (lockObj)
            {
                if (Exists(macAddr))
                {
                    RemoveFromList(macAddr);
                    AddToList(replacement);
                }
                else
                {
                    AddToList(replacement);
                }
            }
		}

		public static void UpdateHost(ModelHost dummy, IPAddress ipAddr)
		{
            lock (lockObj)
            {
                if (Exists(dummy))
                {
                    RemoveFromList(dummy);
                    AddToList(dummy.hostMac, ipAddr);
                }
                else
                {
                    AddToList(dummy.hostMac, ipAddr);
                }
            }
		}

		public static void UpdateHost(ModelHost dummy, string hostName)
		{
            lock (lockObj)
            {
                if (Exists(dummy))
                {
                    RemoveFromList(dummy);
                    AddToList(dummy.hostMac, hostName);
                }
                else
                {
                    AddToList(dummy.hostMac, hostName);
                }
            }
		}

		public static void UpdateHost(ModelHost dummy, IPAddress ipAddr, string hostName)
		{
            lock (lockObj)
            {
                if (Exists(dummy))
                {
                    RemoveFromList(dummy);
                    AddToList(dummy.hostMac, ipAddr, hostName);
                }
                else
                {
                    AddToList(dummy.hostMac, ipAddr, hostName);
                }
            }
		}

		public static void UpdateHost(ModelHost dummy, ModelHost replacement)
		{
            lock (lockObj)
            {
                if (Exists(dummy))
                {
                    RemoveFromList(dummy);
                    AddToList(replacement);
                }
                else
                {
                    AddToList(replacement);
                }
            }
		}
		#endregion 

		public static string PrintLists()
		{
            Logger.Log("Printing list of hosts", Logger.MessageType.INFORM); 
			string _output = "";

			if (hostList != null)
			{
                lock (lockObj)
                {
                    foreach (ModelHost _dummy in hostList)
                    {
                        if (_dummy.reachable)
                        {
                            _output += "\n MAC-Address: " + _dummy.hostMac;

                            if (_dummy.hostName != null)
                                _output += "\n Host Name: " + _dummy.hostName;
                            else
                                _output += "\n Host Name: NA..";

                            if (_dummy.hostIP != null)
                                _output += "\n IP-Address: " + _dummy.hostIP.ToString();
                            else
                                _output += "\n IP-Address: NA..";

                            _output += "\n \n";
                        }
                        else
                        {
                            _output += "\n MAC-Address: " + _dummy.hostMac + _dummy.status;

                            if (_dummy.hostName != null)
                                _output += "\n Host Name: " + _dummy.hostName;
                            else
                                _output += "\n Host Name: NA..";

                            if (_dummy.hostIP != null)
                                _output += "\n IP-Address: " + _dummy.hostIP.ToString();
                            else
                                _output += "\n IP-Address: NA..";

                            _output += "\n \n";
                        }
                    }
                }
			}
			return _output;
		}

        public static void PingThroughList()
        {
            Logger.Log("Pinging through host list..", Logger.MessageType.INFORM);
            lock (lockObj)
            {
                if (ModelHost.hostList != null)
                {
                    for (int i = 0; i < ModelHost.hostList.Count; i++)
                    {
                        ModelHost _dummy = ModelHost.hostList[i];
                        if (_dummy.hostIP != null)
                        {
                            Ping _ping = new Ping();

                            try
                            {
                                PingReply _reply = _ping.Send(_dummy.hostIP);

                                if (_reply.Status != IPStatus.Success)
                                {
                                    _dummy.reachable = false;

                                    if (_reply.Status == IPStatus.DestinationHostUnreachable)
                                        _dummy.status = " (Host Unreachable)";
                                    else if (_reply.Status == IPStatus.DestinationNetworkUnreachable)
                                        _dummy.status = " (Network Unreachable)";
                                    else if (_reply.Status == IPStatus.DestinationPortUnreachable)
                                        _dummy.status = " (Port Unreachable)";
                                    else if (_reply.Status == IPStatus.DestinationUnreachable)
                                        _dummy.status = " (Unreachable)";
                                    else if (_reply.Status == IPStatus.TimedOut)
                                        _dummy.status = " (Ping Time Out)";
                                    else if (_reply.Status == IPStatus.Unknown)
                                        _dummy.status = " (Unknown Ping Error)";
                                    else
                                        _dummy.status = " (Unknown Ping Error)";

                                    ModelHost.hostList[i] = _dummy;
                                }
                                else
                                {
                                    _dummy.reachable = true;

                                    ModelHost.hostList[i] = _dummy;
                                }
                            }
                            catch
                            {
                                _dummy.reachable = false;
                                _dummy.status = " (Unknown Ping Error)";
                                ModelHost.hostList[i] = _dummy;
                            }
                        }
                    }
                }
            }
        }

		#endregion
	}
}

