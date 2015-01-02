using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using System.Diagnostics;

using MadNet;

namespace MadProbe
{
    class MadProbe
    {
        private Thread _thread;
        private TcpListener _listener;
        private TcpClient _client;

        private AES _aes;

        public MadProbe(string aes, int port)
        {
            _aes = new AES(aes);
            _listener = new TcpListener(new IPEndPoint(IPAddress.Loopback, port));
        }

        public void Start()
        {
            _thread = new Thread(Worker);
        }

        private void Worker()
        {
            while (true)
            {
                try
                {

                    _client = _listener.AcceptTcpClient();

                    using (ProbeInfoPacket _packet = new ProbeInfoPacket(_client.GetStream()))
                    {
                        _packet.name = MadConf.conf.probename;
                        _packet.time = DateTime.Now;

                        PerformanceCounter _cpucounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);
                        _packet.cpuusage = Convert.ToString(_cpucounter.NextValue()) + "%";

                        PerformanceCounter _ramcounter = new PerformanceCounter("Memory", "Available MBytes");
                        _packet.ramusage = Convert.ToString(_cpucounter.NextValue()) + "MBytes";

                        _packet.SendPacket(_aes);
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    _client.Close();
                }
            }
        }
    }

}
