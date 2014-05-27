using System;

namespace MAD
{
    public class JobHttpOptions
    {
        public System.Net.IPAddress targetAddress;
        public int port;

        public JobHttpOptions(System.Net.IPAddress targetAddress, int port)
        {
            this.targetAddress = targetAddress;
            this.port = port;
        }
    }
}
