using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using SharpPcap;
using PacketDotNet;
using MAD;
using MAD.Logging;
using MAD.Helper;
using MAD.JobSystemCore;
namespace MAD.MacFinders
{
    static class ARPReader
    {
        private static CaptureDeviceList _list;
        private static ICaptureDevice _dev;
        private static ICaptureDevice _listenDev;
        private static Object lockObj = new Object();
        private static Thread _steady;
        public static uint networkInterface = MadConf.conf.arpInterface - 1;
        public static uint subnetMask;
        public static IPAddress netAddress;
        public static IPAddress srcAddress;
        public static void CheckStart()
        {
            Logger.Log("Start Checking Devices", Logger.MessageType.INFORM);
            InitInterfaces();
            Thread _listen = new Thread(ListenCheckResponses);
            _listen.Start();
            EthernetPacket _ethpac = NetworkHelper.CreateArpBasePacket(_dev.MacAddress);
            foreach (ModelHost _dummy in ModelHost.hostList)
            {
                _dummy.status = false;
                ExecuteRequests(_dummy.hostIP);
            }
            Thread.Sleep(10000);
            DeInitInterfaces();
        }
        public static void FloodStart()
        {
            Logger.Log("Start ArpReader", Logger.MessageType.INFORM);
            InitInterfaces();
            Thread _listen = new Thread(ListenFloodResponses);
            _listen.Start();
            EthernetPacket _ethpac = NetworkHelper.CreateArpBasePacket(_dev.MacAddress);
            SendRequests();
            Thread.Sleep(10000);
            _listen.Join();
            DeInitInterfaces();
        }
        private static void InitInterfaces()
        {
            _list = CaptureDeviceList.Instance;
            _dev = _list[(int)networkInterface];
            _dev.Open();
            _listenDev = _list[(int)networkInterface];
        }
        private static void ListenCheckResponses()
        {
            _listenDev.OnPacketArrival += new PacketArrivalEventHandler(ProcessCheck);
            _listenDev.Open();
            _listenDev.StartCapture();
        }
        private static void ListenFloodResponses()
        {
            _listenDev.OnPacketArrival += new PacketArrivalEventHandler(ProcessFlood);
            _listenDev.Open();
            _listenDev.StartCapture();
        }
        private static void DeInitInterfaces()
        {
            _dev.Close();
            _listenDev.StopCapture();
            _listenDev.Close();
            _dev = null;
            _listenDev = null;
        }
        public static string ListInterfaces()
        {
            Logger.Log("Listed Interfaces for ArpReader", Logger.MessageType.INFORM);
            _list = CaptureDeviceList.Instance;
            uint _count = 1;
            string _buffer = "";
            if (_list.Count < 1)
            {
                _buffer += "No Devices";
                return _buffer;
            }
            _buffer += "The following devices are available on this machine:";
            _buffer += "\n";
            _buffer += "-----------------------------------------------------";
            _buffer += "\n";
            foreach (ICaptureDevice dev in _list)
            {
                _buffer += "Nr " + _count.ToString() + "\n";
                _buffer += dev.ToString() + "\n";
                _count++;
            }
            return _buffer;
        }
        public static void SteadyStart(object jsArg)
        {
            _steady = new Thread(SteadyStartsFunktion);
            _steady.Start(jsArg);
        }
        public static void SteadyStop()
        {
            _steady.Abort();
        }
        private static void SteadyStartsFunktion(object jsArg)
        {
            while (true)
            {
                JobSystem _js = (JobSystem)jsArg;
                FloodStart();
                _js.SyncNodes(ModelHost.hostList);
                Thread.Sleep(300000);
            }
        }
        private static void SendRequests()
        {
            Logger.Log("Sending ArpRequest flood..", Logger.MessageType.INFORM);
            uint _hosts = NetworkHelper.GetHosts(subnetMask);
            byte[] _netPartBytes = netAddress.GetAddressBytes();
            if (BitConverter.IsLittleEndian)
                Array.Reverse(_netPartBytes);
            uint _netPartInt = BitConverter.ToUInt32(_netPartBytes, 0);
            EthernetPacket _ethpac = NetworkHelper.CreateArpBasePacket(_dev.MacAddress);
            byte[] _targetBytes = new byte[4];
            for (int i = 1; i < _hosts - 1; i++)
            {
                _targetBytes = BitConverter.GetBytes(_netPartInt + i);
                Array.Resize(ref _targetBytes, 4);
                Array.Reverse(_targetBytes);
                IPAddress _target = new IPAddress(_targetBytes);
                ExecuteRequests(_ethpac, _target);
            }
        }
        public static void ExecuteRequests(IPAddress destIP)
        {
            EthernetPacket _ethpac = NetworkHelper.CreateArpBasePacket(_dev.MacAddress);
            ARPPacket _arppac = new ARPPacket(ARPOperation.Request,
            System.Net.NetworkInformation.PhysicalAddress.Parse("00-00-00-00-00-00"),
            destIP,
            _ethpac.SourceHwAddress,
            srcAddress);
            _ethpac.PayloadPacket = _arppac;
            try
            {
                _dev.SendPacket(_ethpac);
            }
            catch (Exception ex)
            {
                Logger.Log("Problems with sending ArpRequest flood: " + ex.ToString(), Logger.MessageType.ERROR);
            }
        }
        private static void ExecuteRequests(EthernetPacket pacBase, IPAddress destIP)
        {
            ARPPacket _arppac = new ARPPacket(ARPOperation.Request,
            System.Net.NetworkInformation.PhysicalAddress.Parse("00-00-00-00-00-00"),
            destIP,
            pacBase.SourceHwAddress,
            srcAddress);
            pacBase.PayloadPacket = _arppac;
            try
            {
                _dev.SendPacket(pacBase);
            }
            catch (Exception ex)
            {
                Logger.Log("Problems with sending ArpRequest flood: " + ex.ToString(), Logger.MessageType.ERROR);
            }
        }
        private static void ProcessFlood(object sender, CaptureEventArgs packet)
        {
            if (packet.Packet.Data.Length >= 42)
            {
                byte[] data = packet.Packet.Data;
                byte[] arpheader = new byte[4];
                arpheader[0] = data[12];
                arpheader[1] = data[13];
                arpheader[2] = data[20];
                arpheader[3] = data[21];
                if (arpheader[0] == 8 && arpheader[1] == 6 && arpheader[2] == 0 && arpheader[3] == 2)
                    SyncModelHosts(ReadAddresses(data));
            }
        }
        private static void ProcessCheck(object sender, CaptureEventArgs packet)
        {
            if (packet.Packet.Data.Length >= 42)
            {
                byte[] data = packet.Packet.Data;
                byte[] arpheader = new byte[4];
                ModelHost _dummy = new ModelHost();
                arpheader[0] = data[12];
                arpheader[1] = data[13];
                arpheader[2] = data[20];
                arpheader[3] = data[21];
                if (arpheader[0] == 8 && arpheader[1] == 6 && arpheader[2] == 0 && arpheader[3] == 2)
                    _dummy = ReadAddresses(data);
                if (ModelHost.Exists(_dummy))
                {
                    _dummy.status = true;
                    ModelHost.UpdateHost(_dummy, _dummy);
                }
            }
        }
        private static ModelHost ReadAddresses(byte[] data)
        {
            byte[] hwAddress = new byte[6];
            byte[] ipAddress = new byte[4];
            uint hwOffset = 22;
            uint ipOffset = 28;
            for (int i = 0; i < 6; i++)
            {
                hwAddress[i] = data[hwOffset + i];
            }
            for (int i = 0; i < 4; i++)
            {
                ipAddress[i] = data[ipOffset + i];
            }
            string hwAddr = NetworkHelper.getPhysicalAddressStringFromArp(hwAddress);
            IPAddress ipAddr = new IPAddress(ipAddress);
            string _name = TryGetName(ipAddr);
            ModelHost _tmp;
            if (String.IsNullOrEmpty(_name))
                _tmp = new ModelHost(hwAddr, ipAddr);
            else
                _tmp = new ModelHost(hwAddr, ipAddr, _name);
            return _tmp;
        }
        private static void SyncModelHosts(ModelHost _dummy)
        {
            if (ModelHost.Exists(_dummy))
                ModelHost.UpdateHost(_dummy, _dummy);
            else
                ModelHost.AddToList(_dummy);
        }
        private static string TryGetName(IPAddress ipAddr)
        {
            string hostName = "";
            IPHostEntry ipEntry;
            try
            {
                ipEntry = Dns.GetHostEntry(ipAddr);
                hostName = ipEntry.HostName;
            }
            catch (Exception)
            {
                hostName = "N.A.";
            }
            return hostName;
        }
    }
}