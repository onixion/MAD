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
        private CaptureDeviceList _list;
        private ICaptureDevice _dev;
        private ICaptureDevice _listenDev;

        private struct Addresses
        {
            string macAddress;
            IPAddress ipAddress;
        }

        public uint networkInterface;
        public uint subnetMask;
        public IPAddress netAddress;
        public IPAddress srcAddress;

        public void Start()
        {
            Logger.Log("Start ArpReader", Logger.MessageType.INFORM);
            _list = CaptureDeviceList.Instance;
            Thread _send = new Thread(SendRequests);
            Thread _listen = new Thread(ListenForRequests);
            _listen.Start();
            Thread.Sleep(10);
            _send.Start();
            _send.Join();
            _listen.Join();
            _dev.Close();
            Thread.Sleep(100);
            _listenDev.Close();
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
            _buffer += "\n";

            foreach (ICaptureDevice dev in _list)
                _buffer += dev.ToString() + "\n";

            return _buffer;
        }

        private void SendRequests()
        {
            Logger.Log("Sending ArpRequest flood..", Logger.MessageType.INFORM);
            uint _hosts = NetworkHelper.GetHosts(subnetMask);

            byte[] _netPartBytes = netAddress.GetAddressBytes();
            if (BitConverter.IsLittleEndian)
                Array.Reverse(_netPartBytes);

            uint _netPartInt = BitConverter.ToUInt32(_netPartBytes, 0);

            _dev = _list[(int) networkInterface];
            _dev.Open();
            PhysicalAddress _sourceHW = _dev.MacAddress;

            EthernetPacket _ethpac = new EthernetPacket(_sourceHW, PhysicalAddress.Parse("FF-FF-FF-FF-FF-FF"), EthernetPacketType.Arp);

            byte[] _targetBytes = new byte[4];

            for (int i = 1; i < _hosts - 1; i++)
            {
                _targetBytes = BitConverter.GetBytes(_netPartInt + i);
                Array.Resize(ref _targetBytes, 4);
                Array.Reverse(_targetBytes);

                IPAddress _target = new IPAddress(_targetBytes);

                ARPPacket _arppac = new ARPPacket(ARPOperation.Request, 
                                                    System.Net.NetworkInformation.PhysicalAddress.Parse("00-00-00-00-00-00"), 
                                                    _target, 
                                                    _sourceHW, 
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
        }

        private void ListenForRequests()
        {
            Logger.Log("Started listening for Responses on ArpFlood..", Logger.MessageType.INFORM);
            _listenDev = _list[(int)networkInterface];

            _listenDev.OnPacketArrival += new PacketArrivalEventHandler(ParseArpPackets);
            _listenDev.Open(DeviceMode.Normal);
            _listenDev.StartCapture();
            
        }

        private void ParseArpPackets(object sender, CaptureEventArgs packet)
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
                    ReadAddresses(data);                                                                                        //Ersetezen mit funktion zum host hinzufügen
            }
        }

        private string ReadAddresses(byte[] data)                                                                           //passend machen
        {
            byte[] HWAddress = new byte[6];
            byte[] IPAddress = new byte[4];

            uint hwOffset = 22;
            uint ipOffset = 28;

            for (int i = 0; i < 6; i++)
            {
                HWAddress[i] = data[hwOffset + i];
            }

            for (int i = 0; i < 4; i++)
            {
                IPAddress[i] = data[ipOffset + i];
            }

            string macAddress = NetworkHelper.getMacStringFromArp(HWAddress);

            IPAddress ipadr = new IPAddress(IPAddress);
            string ipAddress = ipadr.ToString();

            string buffer = macAddress + "\n" + ipAddress;

            return buffer; 
        }

        private void SyncModelHosts(Addresses addrs)
        {

        }
    }
}
