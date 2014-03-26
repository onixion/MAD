using System;
using System.Net;

namespace MAD
{
    public class JobPingOptions : JobOptions
    {
        public int ttl;

        public JobPingOptions(string jobName, JobType jobType, int delay, IPAddress targetAddress, int ttl)
        {
            this.jobName = jobName;
            this.jobType = jobType;
            this.delay = delay;
            this.targetAddress = targetAddress;
            this.ttl = ttl;
        }
    }
}
