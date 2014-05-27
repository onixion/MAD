using System;

namespace MAD
{
    public class JobPortOptions
    {
        public System.Net.IPAddress targetAddress;
        public int port;

        public JobPortOptions(System.Net.IPAddress targetAddress, int port)
        {
            this.targetAddress = targetAddress;
            this.port = port;
        }
    }
}
