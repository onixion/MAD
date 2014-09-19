using System;
using System.Threading;

using SharpPcap;
using PacketDotNet;

using MAD.Logging;

namespace MAD.MacFinders
{
    class ARPReader
    {
        private bool _running = false;
        private uint _networkInterface;

        public string Start()
        {
            if (!_running)
            {
                Thread send = new Thread(sendRequests);
            }
        }

        public string ListInterfaces()
        {
            Logger.Log("Listed Interfaces for ArpReader", Logger.MessageType.INFORM);

            CaptureDeviceList list = CaptureDeviceList.Instance;

            string _buffer = "";

            if (list.Count < 1)
            {
                _buffer += "No Devices";
                return _buffer;
            }

            _buffer += "The following devices are available on this machine:";
            _buffer += "\n";
            _buffer += "-----------------------------------------------------";

            foreach (ICaptureDevice dev in list)
                _buffer += dev.ToString() + "\n";

            return _buffer;

        }

        public string SetInterface(uint netInterface)
        {
            _networkInterface = netInterface;
        }

        private void sendRequests()
        {

        }


    }
}
