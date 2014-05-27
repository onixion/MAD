using System;

namespace MAD
{
    public class JobPingOptions
    {
        public System.Net.IPAddress targetAddress;
        public int ttl;

        public JobPingOptions(System.Net.IPAddress targetAddress, int ttl)
        {
            this.targetAddress = targetAddress;
            this.ttl = ttl;
        }
    }
}
