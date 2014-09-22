using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

using SharpPcap;
using PacketDotNet;

using MAD.Logging;
using MAD.Helper;

namespace MAD.MacFinders
{
    class ARPReader
    {
        private bool _running = false;
        private CaptureDeviceList _list; 

        public uint networkInterface;
        public uint subnetMask;
        public IPAddress netAddress; 

        public string Start()
        {
            if (!_running)
            {
                _list = CaptureDeviceList.Instance;
                Thread send = new Thread(sendRequests);
            }
        }

        public string ListInterfaces()
        {
            Logger.Log("Listed Interfaces for ArpReader", Logger.MessageType.INFORM);

            _list = CaptureDeviceList.Instance;

            string _buffer = "";

            if (_list.Count < 1)
            {
                _buffer += "No Devices";
                return _buffer;
            }

            _buffer += "The following devices are available on this machine:";
            _buffer += "\n";
            _buffer += "-----------------------------------------------------";

            foreach (ICaptureDevice dev in _list)
                _buffer += dev.ToString() + "\n";

            return _buffer;
        }

        private void sendRequests()
        {
            uint _hosts = NetworkHelper.GetHosts(subnetMask);

            byte[] _netPartBytes = netAddress.GetAddressBytes();
            if (BitConverter.IsLittleEndian)
                Array.Reverse(_netPartBytes);

            uint _netPartInt = BitConverter.ToUInt32(_netPartBytes, 0);

            ICaptureDevice _dev = _list[(int) networkInterface];
            PhysicalAddress _sourceHW = _dev.MacAddress;
            IPAddress _sourceIP = _dev.IP

            EthernetPacket _ethpac = new EthernetPacket(_sourceHW, PhysicalAddress.Parse("FF-FF-FF-FF-FF-FF"), EthernetPacketType.Arp);

            for (int i = 1; i < _hosts - 1; i++)
            {
                byte[] _targetBytes = BitConverter.GetBytes(_netPartInt + i);
                Array.Reverse(_targetBytes);

                IPAddress _target = new IPAddress(_targetBytes);

                ARPPacket _arppac = new ARPPacket(ARPOperation.Request, System.Net.NetworkInformation.PhysicalAddress.Parse("00-00-00-00-00-00"), _target, _sourceHW, IPAddress.Parse("192.168.1.114"));
            }
            
        }
    }
}
